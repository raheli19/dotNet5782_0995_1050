
//Tania: et Assignment verifier
// nouveau case dans le main pour imprimer la list des dronescharges
// tostring du drone si doit tout imprimer
// ajout d'un package dans les 2 clients concernés
// id de la parcel
// banai
// toutes les help en pv

using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;

using DalObject;
using System.Linq;
using BL;
using static BL.Tools;

namespace IBL
{

    public partial class BL : IBL
    {
        static Random rand = new Random();
        double BatteryFree, BatteryLightWeight, BatteryMiddleWeight, BatteryHeavyWeight, ChargingDroneRate;

        private IDAL.IDal p;
        help h = new help();

        List<DroneDescription> DroneList = new List<DroneDescription>();

        public BL()
        {
            //constructor of the BL file
            p = new DalObject.DalObject();

            //    //initialisation de la droneList de la bl ?????


            //    DroneDescription Bldr = new DroneDescription();  // drone from the bl
            //    ParcelToClient ParcelInClient = new ParcelToClient();
            //    // les listes sont vides, il faut les reupérer de la dal, seulement elles sont vides elles aussi

            //    foreach (var item in p.IEDroneList())
            //    {
            //        Bldr.Id = item.ID;
            //        Bldr.Model = item.Model;
            //        Bldr.battery = item.Battery;

            //        foreach (var item2 in p.IEParcelList())
            //        {
            //            if (item2.DroneId == item.ID) //gets all the parcel's info from the dal
            //            {
            //                ParcelInClient.ID = item2.ID;
            //                ParcelInClient.weight = (WeightCategories)item2.Weight;
            //                ParcelInClient.priority = (Priorities)item2.Priority;
            //                if (item2.Requested != DateTime.MinValue)
            //                    ParcelInClient.Status = ParcelStatus.requested;
            //                if (item2.Scheduled != DateTime.MinValue)
            //                    ParcelInClient.Status = ParcelStatus.scheduled;
            //                if (item2.PickedUp != DateTime.MinValue)
            //                    ParcelInClient.Status = ParcelStatus.pickedup;
            //                if (item2.Delivered != DateTime.MinValue)
            //                    ParcelInClient.Status = ParcelStatus.delivered;

            //            }
            //        }
            //        //ParcelInclient is the parcel associated to the drone
            //        //double parLongitude = p.ClientById(ParcelInClient.client.ID).Longitude; 
            //        //double parLatitude = p.ClientById(ParcelInClient.client.ID).Latitude; 
            //        double senderLat = default;
            //        double senderLong = default;
            //        double targetLat = default;
            //        double targetLong = default;
            //        int senderId1 = default;
            //        int targetId1 = default;

            //        try
            //        {
            //            //senderLat = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Latitude;
            //            //senderLong = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Longitude;
            //            //targetLat = p.ClientById((p.ParcelById(ParcelInClient.ID)).TargetId).Longitude;
            //            //targetLong = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Latitude;
            //            try
            //            {
            //                senderId1 = (p.ParcelById(ParcelInClient.ID)).SenderId;
            //                targetId1 = (p.ParcelById(ParcelInClient.ID)).TargetId;
            //            }
            //            catch (IDAL.DO.ParcelException ex)
            //            {
            //                throw new IDNotFound("Not found", ex);
            //            }

            //            senderLat = p.ClientById(senderId1).Latitude;
            //            senderLong = p.ClientById(senderId1).Longitude;
            //            targetLat = p.ClientById(targetId1).Latitude;
            //            targetLong = p.ClientById(targetId1).Longitude;
            //        }
            //        catch (IDAL.DO.ClientException ex)
            //        {
            //            throw new NotFound("Not found", ex);
            //        }


            //        // the drone is shipping
            //        if (ParcelInClient.Status != ParcelStatus.delivered)// the parcel has not been delivered yet.
            //        {
            //            Bldr.Status = DroneStatuses.shipping;
            //            Bldr.loc = new Localisation();
            //            if (ParcelInClient.Status == ParcelStatus.scheduled && ParcelInClient.Status != ParcelStatus.pickedup)
            //            {
            //                Station s = NearestStation(Bldr.loc, true);
            //                Bldr.loc = s.Loc;
            //            }
            //            if (ParcelInClient.Status == ParcelStatus.pickedup && ParcelInClient.Status != ParcelStatus.delivered)// drone location = sender location
            //            {

            //                Bldr.loc.longitude = senderLong;
            //                Bldr.loc.latitude = senderLat;
            //            }

            //            // calculates the distances for battery
            //            double delDist = distance(senderLat, senderLong, targetLat, targetLong); // delivering distance
            //            Localisation l = location(targetLat, targetLong);
            //            Station chargeStat = NearestStation(l, false);
            //            double cDist = distance(targetLat, targetLong, chargeStat.Loc.latitude, chargeStat.Loc.longitude);
            //            double totalDistance = delDist + cDist;
            //            double minBattery = BatteryAccToDistance(totalDistance);
            //            Bldr.battery = h.getRandomNumber(minBattery, 100.0);
            //        }

            //        // if drone isn't shipping
            //        else if (ParcelInClient.Status == ParcelStatus.delivered || ParcelInClient.Status == ParcelStatus.scheduled)// the drone isn't shipping
            //        {
            //            int random = (rand.Next(0, 1)) * 2;
            //            if (random == 0)
            //            {
            //                Bldr.Status = DroneStatuses.free;
            //            }
            //            if (random == 2)
            //            {
            //                Bldr.Status = DroneStatuses.shipping;
            //            }
            //        }
            //        // if the drone is charging
            //        if (Bldr.Status == DroneStatuses.maintenance)
            //        {
            //            // random localisation entre les differentes stations
            //            List<int> helplist = p.IdStation();
            //            int index = rand.Next(helplist.Count);
            //            int s = helplist[index];
            //            IDAL.DO.Station stat = p.StationById(s);
            //            Bldr.loc = location(stat.Latitude, stat.Longitude);
            //            Bldr.battery = h.getRandomNumber(0, 20);

            //        }

            //        //the drone is free
            //        else if (Bldr.Status == DroneStatuses.free)
            //        {

            //            // location of one of the client that have received a parcel
            //            List<int> helpList = p.clientReceivedParcel();
            //            int index = rand.Next(helpList.Count);
            //            int s = helpList[index];
            //            try
            //            {
            //                IDAL.DO.Client c = p.ClientById(s);


            //                Bldr.loc = location(c.Latitude, c.Longitude);
            //            }
            //            catch (IDAL.DO.ClientException ex)
            //            {
            //                throw new NotFound("Didnt find", ex);
            //            }

            //            Station chargeStat = NearestStation(Bldr.loc, true);
            //            double statDist = distance(chargeStat.Loc.latitude, chargeStat.Loc.longitude, Bldr.loc.latitude, Bldr.loc.longitude);
            //            double minBattery = BatteryAccToDistance(statDist);
            //            Bldr.battery = h.getRandomNumber(minBattery, 100.0);

            //        }
            //        Bldr.weight = ParcelInClient.weight;
            //        Bldr.Status = (DroneStatuses)ParcelInClient.Status;


            //    }

           double[] BatteryUse = p.ElectricityUse();

            BatteryFree = BatteryUse[0];
            BatteryLightWeight = BatteryUse[1];
            BatteryMiddleWeight = BatteryUse[2];
            BatteryHeavyWeight = BatteryUse[3];
            ChargingDroneRate = BatteryUse[4];

            IEnumerable<IDAL.DO.Drone> listDronesFromIdal = p.IEDroneList();
            IEnumerable<IDAL.DO.Parcel> listParcelsFromIdal = p.IEParcelList();
            IEnumerable<IDAL.DO.Station> listStationsFromIdal = p.IEStationList();
            IEnumerable<IDAL.DO.Client> listCustomersFromIdal = p.IEClientList();
            Random rand = new Random();
            foreach (var itemDrone in listDronesFromIdal) // go all over the drones from the dal, restarts them and add them to dronelist in BL.
            {
                DroneDescription drone = new DroneDescription();
                drone.Id = itemDrone.ID;
                drone.Model = itemDrone.Model;
               // drone.weight = (WeightCategories)itemDrone.w;
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
                                    IDAL.DO.Station myStation = ClosestStationToLocation(customerLocation);
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
                        IDAL.DO.Station myStationForBattery = ClosestStationToLocation(customerLocationForBattery);
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
                        longitude = listStationsFromIdal.ElementAt(stationIndex).Longitude

                    };
                    foreach (var itemStation in listStationsFromIdal)
                    {
                        if (itemStation.Latitude == drone.loc.latitude && itemStation.Longitude == drone.loc.longitude)
                        {
                            IDAL.DO.DroneCharge DroneCharge = new IDAL.DO.DroneCharge() // update the new charge slot.
                            {
                                DroneId = drone.Id,
                                StationId = itemStation.ID
                            };
                            p.addDroneCharge(DroneCharge);
                            break;
                        }
                    }
                    drone.battery = 20 * rand.NextDouble();
                }
                if (drone.Status == DroneStatuses.free) // if drone is free 
                {
                    IEnumerable<IDAL.DO.Client> custumersWhoGotParcel = CustomersWithParcel(listCustomersFromIdal);
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
                    IDAL.DO.Station stationForBattery = ClosestStationToLocation(drone.loc);
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

        private IDAL.DO.Station ClosestStationToLocation(Localisation L)
        {

            double minDistance = 0;
            int checkIfFirst = 0;
            IEnumerable<IDAL.DO.Station> listStationsFromIdal = p.IEStationList();
            IDAL.DO.Station myStation = new IDAL.DO.Station();
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
        private List<IDAL.DO.Client> CustomersWithParcel(IEnumerable<IDAL.DO.Client> L)
        {
            IEnumerable<IDAL.DO.Parcel> listParcelsFromIdal = p.IEParcelList();
            List<IDAL.DO.Client> customerWithParcel = new List<IDAL.DO.Client>();
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

    }
}