//Tania: et Assignment verifier
// nouveau case dans le main pour imprimer la list des dronescharges
// tostring du drone si doit tout imprimer
// ajout d'un package dans les 2 clients concernés
// id de la parcel
// banai
// toutes les help en pv

using System;
using System.Collections.Generic;
using BO;
using BLApi;
using System.Linq;
using DalApi;
using static BL.Tools;
using System.Text;

namespace BL
{

    sealed partial class BL : IBL
    {
        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }

        static Random rand = new Random();
        double BatteryFree, BatteryLightWeight, BatteryMiddleWeight, BatteryHeavyWeight, ChargingDroneRate;

        internal IDal p=DalFactory.GetDal();
        help h = new help();

        List<DroneDescription> DroneList = new List<DroneDescription>();

        BL()
        {
            //constructor of the BL file
           
            

           double[] BatteryUse = p.ElectricityUse();

            BatteryFree = BatteryUse[0];
            BatteryLightWeight = BatteryUse[1];
            BatteryMiddleWeight = BatteryUse[2];
            BatteryHeavyWeight = BatteryUse[3];
            ChargingDroneRate = BatteryUse[4];

            IEnumerable<DO.Drone> listDronesFromIdal = p.IEDroneList();
            IEnumerable<DO.Parcel> listParcelsFromIdal = p.IEParcelList();
            IEnumerable<DO.Station> listStationsFromIdal = p.IEStationList();
            IEnumerable<DO.Client> listCustomersFromIdal = p.IEClientList();
            Random rand = new Random();
            foreach (var itemDrone in listDronesFromIdal) // go all over the drones from the dal, restarts them and add them to dronelist in BL.
            {
                DroneDescription drone = new DroneDescription();
                drone.Id = itemDrone.ID;
                drone.Model = itemDrone.Model;
                drone.weight = (WeightCategories)itemDrone.weight;
                drone.Status = DroneStatuses.free; // drone is not shipping if he has parcel that scheduled and delivered or if he never scheduled to any parcel.
                foreach (var itemParcel in listParcelsFromIdal)
                {
                    if (itemParcel.DroneId == drone.Id && itemParcel.Delivered == DateTime.MinValue) // if  drone scheduled but not delivered
                    {
                        drone.Status = DroneStatuses.shipping;
                        drone.parcelId= itemParcel.ID;
                        if (itemParcel.PickedUp == DateTime.MinValue) // if drone not ever picket up - his location is the location of the closest station to the customer
                        {
                            foreach (var itemCustomer in listCustomersFromIdal)
                            {
                                if (itemCustomer.ID == itemParcel.SenderId) // find the sender by his id.
                                {
                                    Localisation customerLocation = new Localisation(); // creat the sender's location.
                                    customerLocation.latitude = itemCustomer.Latitude;
                                    customerLocation.longitude = itemCustomer.Longitude;
                                    DO.Station myStation = ClosestStationToLocation(customerLocation);
                                    drone.loc.latitude = myStation.Latitude;
                                    drone.loc.longitude = myStation.Longitude;
                                }
                            }
                        }
                        else // if drone picked up but not delivered.
                        {
                            foreach (var itemCustomer in listCustomersFromIdal)
                            {
                                if (itemCustomer.ID == itemParcel.SenderId) // find the sender by his id.
                                {
                                    Localisation customerLocation = new Localisation(); // creat the sender's location and its the drone's location.
                                    customerLocation.latitude = itemCustomer.Latitude;
                                    customerLocation.longitude = itemCustomer.Longitude;
                                    drone.loc = customerLocation;
                                }
                            }
                        }
                        Localisation customerLocationForBattery = new Localisation();
                        foreach (var itemCustomer in listCustomersFromIdal) // calculate the battery status.
                        {
                            if (itemCustomer.ID == itemParcel.TargetId) // find the target by his id.
                            {                                // creat the target's location. // i guess the target is customer too
                                customerLocationForBattery.latitude = itemCustomer.Latitude;
                                customerLocationForBattery.longitude = itemCustomer.Longitude;
                                break;
                            }
                        }
                        double distanceToDeliver = Distance(drone.loc, customerLocationForBattery);  //distance from drone to target                      
                        DO.Station myStationForBattery = ClosestStationToLocation(customerLocationForBattery);
                        Localisation stationLocation = new Localisation(); // creat the closest station's location.
                        stationLocation.latitude = myStationForBattery.Latitude;
                        stationLocation.longitude = myStationForBattery.Longitude;
                        distanceToDeliver += Distance(customerLocationForBattery, stationLocation); // + distance from target to closest station
                        if (drone.weight == WeightCategories.heavy) // convert kilometers to percentage that the drone will waste for this distance.
                            distanceToDeliver *= (BatteryHeavyWeight + BatteryFree);
                        if (drone.weight == WeightCategories.middle)
                            distanceToDeliver *= (BatteryMiddleWeight + BatteryFree);
                        if (drone.weight == WeightCategories.low)
                            distanceToDeliver *= (BatteryLightWeight + BatteryFree);
                        drone.battery = rand.NextDouble() * (100 - distanceToDeliver) + distanceToDeliver; //rand between the percente needed to the fly, and 100%                 

                    }
                }
                if (drone.Status != DroneStatuses.shipping) // if drone not shipping - rand between maintance to free.
                    drone.Status = (DroneStatuses)rand.Next(0, 2);
                if (drone.Status == DroneStatuses.maintenance) // if drone in maintance - rand his location between the exists stations and rand his battery.
                {
                    int stationIndex = rand.Next(0, listStationsFromIdal.Count());
                    drone.loc = new Localisation()
                    {
                        latitude = listStationsFromIdal.ElementAt(stationIndex).Latitude,
                        longitude = listStationsFromIdal.ElementAt(stationIndex).Longitude,

                    };
                    foreach (var itemStation in listStationsFromIdal)
                    {
                        if (itemStation.Latitude == drone.loc.latitude && itemStation.Longitude == drone.loc.longitude)
                        {
                            DO.DroneCharge DroneCharge = new DO.DroneCharge() // update the new charge slot.
                            {
                                DroneId = drone.Id,
                                StationId = itemStation.ID
                            };
                            p.AddDroneCharge(DroneCharge);
                            break;
                        }
                    }
                    drone.battery = 20 * rand.NextDouble();
                }
                if (drone.Status == DroneStatuses.free) // if drone is free 
                {
                    IEnumerable<DO.Client> custumersWhoGotParcel = CustomersWithParcel(listCustomersFromIdal);
                    if (custumersWhoGotParcel.Count() == 0)
                        drone.loc = new Localisation() { latitude = 0, longitude = 0 };
                    else
                    {
                        int index = rand.Next(0, custumersWhoGotParcel.Count()); // drone location rand between the locations of the customers who got their parcel.
                        drone.loc = new Localisation()
                        {
                            longitude = custumersWhoGotParcel.ElementAt(index).Longitude,
                            latitude = custumersWhoGotParcel.ElementAt(index).Latitude
                        };

                    }
                    DO.Station stationForBattery = ClosestStationToLocation(drone.loc);
                    Localisation stationLocation = new Localisation(); // creat the closest station's location.
                    stationLocation.latitude = stationForBattery.Latitude;
                    stationLocation.longitude = stationForBattery.Longitude;
                    double distanceToCloseestStation = Distance(drone.loc, stationLocation);
                    distanceToCloseestStation *= BatteryFree;
                    drone.battery = rand.NextDouble() * (100 - distanceToCloseestStation) + distanceToCloseestStation; // drone battery rand between the percente need to fly to closest station, to 100%.
                }
                DroneList.Add(drone);
            }
        }

        private DO.Station ClosestStationToLocation(Localisation L)
        {

            double minDistance = 0;
            int checkIfFirst = 0;
            IEnumerable<DO.Station> listStationsFromIdal = p.IEStationList();
            DO.Station myStation = new DO.Station();
            foreach (var item in listStationsFromIdal)
            {
                Localisation stationLocation = new Localisation();
                stationLocation.latitude = item.Latitude;
                stationLocation.longitude = item.Longitude;
                checkIfFirst++;
                if (checkIfFirst == 1)
                {
                    minDistance = Distance(L, stationLocation);
                    myStation = item;
                }
                else
                {
                    if (Distance(L, stationLocation) < minDistance)
                    {
                        minDistance = Distance(L, stationLocation);
                        myStation = item;
                    }
                }

            }
            return myStation;
        }

        /// <summary>
        /// The function gets a location and the list of parcels, and calculates the closest parcel to this location.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="parcelList"></param>
        /// <returns></returns> the closest parcel to the given location.
        private Parcel ClosestParcelToLocation(Localisation L, List<ParcelDescription> parcelList)
        {
            double minDistance = 0;
            int checkIfFirst = 0;
            Parcel closestParcel = new Parcel();
            bool checkExist = false;
            foreach (var item in parcelList) // go all over the parcel.
            {
                var sender = GetClient(GetParcel(item.Id).Sender.ID);
                if (item.Status == ParcelStatus.requested) //if parcel is exist but not associate - find the closest one to L - input location.
                {
                    checkExist = true;
                    checkIfFirst++;
                    if (checkIfFirst == 1)
                    {
                        minDistance = Distance(L, sender.ClientLoc);
                        closestParcel = GetParcel(item.Id);
                    }
                    else
                    {
                        if (Distance(L, sender.ClientLoc) < minDistance)
                        {
                            minDistance = Distance(L, sender.ClientLoc);
                            closestParcel = GetParcel(item.Id);
                        }
                    }
                }
            }
            if (checkExist == true)
                return closestParcel;
            else
                throw new WrongDetailsUpdateException("No parcel exist for shipping");
        }
        #region distance between2Locations
        /// <summary>
        /// The function gets to location and calculates the distance between them
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Returns the distance between the 2 locations</returns>
        private double Distance(Localisation from, Localisation to)
        {
            
            int R = 6371 * 1000;
            double phi1 = from.latitude * Math.PI / 180;
            double phi2 = to.latitude * Math.PI / 180;
            double deltaPhi = (to.latitude - from.latitude) * Math.PI / 180;
            double deltaLambda = (to.longitude - from.longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000;
            return d;
        }
        #endregion

        #region clientInParcel
        /// <summary>
        /// Gets the list of customers, and create list of the customers with parcels
        /// </summary>
        /// <param name="L"></param>
        /// <returns> List of customers with parcel.</returns>
        private List<DO.Client> CustomersWithParcel(IEnumerable<DO.Client> L)
        {
            IEnumerable<DO.Parcel> listParcelsFromIdal = p.IEParcelList();
            List<DO.Client> customerWithParcel = new List<DO.Client>();
            foreach (var item in L)
            {
                foreach (var item1 in listParcelsFromIdal)
                {
                    if (item1.TargetId == item.ID && item1.Delivered != DateTime.MinValue)
                    {
                        customerWithParcel.Add(item);
                    }
                }
            }
            return customerWithParcel;
        }
        #endregion

        public List<ParcelToClient> FindParcelsToClient(int clientId)
        {
            List<ParcelToClient> TempParcLstFromClient = new List<ParcelToClient>();

            foreach (var parcel in p.IEParcelList())
            {
                if (parcel.SenderId == clientId)  //The parcel has been sent by this client so get the info 
                {
                    ParcelToClient PCT = new ParcelToClient();
                    PCT.ID = parcel.ID;
                    PCT.weight = (WeightCategories)parcel.Weight;
                    PCT.priority = (Priorities)parcel.Priority;
                    if (parcel.Requested != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.requested;
                    }
                    else if (parcel.Scheduled != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.scheduled;
                    }
                    else if (parcel.PickedUp != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.pickedup;
                    }
                    else if (parcel.Delivered != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.delivered;
                    }
                    ClientInParcel myClient = new ClientInParcel();
                    myClient.ID = clientId;
                    try //checks if the clients exists
                    {
                        myClient.name = p.ClientById(clientId).Name;
                    }
                    catch (DO.ClientException ex)
                    {
                        throw new IDNotFound("The client doesnt exist", ex);
                    }
                    PCT.client = myClient;
                    TempParcLstFromClient.Add(PCT);

                }
            }
            return TempParcLstFromClient;
        }

        public List<ParcelToClient> FindParcelsFromClient(int clientId)
        {
            List<ParcelToClient> TempParcLstToClient = new List<ParcelToClient>();

            foreach (var parcel in p.IEParcelList())
            {
                if (parcel.TargetId == clientId)  //The parcel has been sent by this client
                {
                    ParcelToClient PCT = new ParcelToClient();
                    PCT.ID = parcel.ID;
                    PCT.weight = (WeightCategories)parcel.Weight;
                    PCT.priority = (Priorities)parcel.Priority;
                    if (parcel.Requested != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.requested;
                    }
                    else if (parcel.Scheduled != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.scheduled;
                    }
                    else if (parcel.PickedUp != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.pickedup;
                    }
                    else if (parcel.Delivered != DateTime.MinValue)
                    {
                        PCT.Status = ParcelStatus.delivered;
                    }
                    ClientInParcel myClient = new ClientInParcel();
                    myClient.ID = clientId;
                    try
                    {
                        myClient.name = p.ClientById(clientId).Name;
                    }
                    catch (DO.ClientException ex)
                    {
                        throw new IDNotFound("The client doesnt exist", ex);
                    }
                    PCT.client = myClient;
                    TempParcLstToClient.Add(PCT);

                }
            }
            return TempParcLstToClient;
        }

        public string parcelsFromCLiList(int clientID)
        {
            String result = "";
            StringBuilder parcelsFromClientStr = new StringBuilder();
            foreach (var elementInCharge in FindParcelsFromClient(clientID))
                parcelsFromClientStr.Append(elementInCharge).Append(", ");
            result += $"Parcels from client: {parcelsFromClientStr.ToString()},\n";
            return result;
        }
        public string parcelsToCliList(int clientID)
        {
            String result = "";
            StringBuilder parcelsToClientStr = new StringBuilder();
            foreach (var elementInCharge in FindParcelsToClient(clientID))
                parcelsToClientStr.Append(elementInCharge).Append(", ");
            result += $"Parcel to client: {parcelsToClientStr.ToString()},\n";
            return result;
        }

        public List<int> AllSenders_Id()
        {
            List<int> idList = new List<int>();
            foreach(var item in p.IEParcelList())
            {
                idList.Add(item.SenderId);
            }
            List<int> idListWithoutDuplicates = idList.Distinct().ToList();
            return idListWithoutDuplicates;
        }
        public List<int> AllTargets_Id()
        {
            List<int> idList = new List<int>();
            foreach (var item in p.IEParcelList())
            {
                idList.Add(item.TargetId);
            }
            List<int> idListWithoutDuplicates = idList.Distinct().ToList();
            return idListWithoutDuplicates;
        }

    }
}