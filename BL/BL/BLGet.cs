
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Linq;
using BLApi;

namespace BL
{

    sealed partial class BL : IBL
    {
        

        //-----------------------------------RETRIEVE-FUNCTIONS--------------------------------------

        #region GetClient
        /// <summary>
        /// This function receives an ID of a client,search for this client in DAL and returns a BL type client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetClient(int id)
        {
            Client clientBL = new Client();
            try
            {
                DO.Client dalClient = dal.ClientById(id);//search it in the clients'list from DAL
                clientBL.ClientLoc = new Localisation();
                clientBL.ID = dalClient.ID; //copies all the fields 
                clientBL.Name = dalClient.Name;
                clientBL.Phone = dalClient.Phone;
                clientBL.ClientLoc.longitude = dalClient.Longitude;
                clientBL.ClientLoc.latitude = dalClient.Latitude;
            }
            catch (DO.ClientException custEX)  //catches if there is an exception in DAL
            {
                throw new IDNotFound($"Client ID {id} was not found", custEX); //throws if there is an exception in BL
            }
            return clientBL; //returns the BL type client
        }
        #endregion

        #region GetStation
        /// <summary>
        /// This function receives a Station ID, searches it in the station List from DAL and returns a BL station
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Station GetStation(int id)
        {
            Station stationBL = new Station(); //creates a new station in BL file
            stationBL.Loc = new Localisation();
            try
            {
                DO.Station dalStat = dal.StationById(id);  //search the station from DAL
                stationBL.ID = dalStat.ID;  //Copies all the fields from the DAL station
                stationBL.Name = dalStat.Name;
                stationBL.Loc.longitude = dalStat.Longitude;
                stationBL.Loc.latitude = dalStat.Latitude;
                stationBL.ChargeSlots = dalStat.ChargeSlots;

                //List<DroneCharging> droneCharging = new List<DroneCharging>();
                //List<int> DronesID = new List<int>();

                //IEnumerable<DO.DroneCharge> droneCharges = p.IEDroneChargeList();  //finds the list which contains the the drone charges from DAL
                //foreach (var item in droneCharges)
                //{
                //    if (item.StationId == stationBL.ID)  // finds the station with the ID received 
                //    {
                //        Drone droneInStation = GetDrone(item.DroneId);   // finds the drones contained in this station
                //        stationBL.DroneCharging.Add(new DroneCharging() { ID = item.DroneId, battery = droneInStation.Battery, });

                //    }
                //}
            }
            catch (DO.StationException statEX) //catches the DAL exception
            {
                throw new IDNotFound($"Station ID {id} was not found", statEX); //throws an BL exception
            }
            return stationBL;
        }
        #endregion

        #region GetDrone
        /// <summary>
        /// This function receives a drone ID, searches it in the drone list in DAL and returns a BL drone
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        public Drone GetDrone(int id)
        {
            Drone droneBL = new Drone();
            try
            {
                DO.Drone dalDrone = dal.DroneById(id);  //search the drone from DAL
                droneBL.ID = dalDrone.ID;  //copies field by field
                droneBL.Model = dalDrone.Model;
                droneBL.MaxWeight = (WeightCategories)dalDrone.weight;
                droneBL.Battery = DroneList.Find(x => x.Id == id).battery;
                droneBL.initialLoc = DroneList.Find(x => x.Id == id).loc;
                
                //if (DroneList.Find(x => x.Id == id).parcelId != 0)
                //{
                //    Parcel tempParcel = GetParcel(DroneList.Find(x => x.Id == id).parcelId);
                //    droneBL.myParcel.ID = tempParcel.ID ;
                //    if(tempParcel.PickedUp!=DateTime.MinValue) // the drone picked up the parcel
                //        droneBL.myParcel.deliveringStatus = true;  //parcel is in delivering
                //    if (tempParcel.Delivered != DateTime.MinValue)
                //        droneBL.myParcel.deliveringStatus = false;
                //    if(tempParcel.Scheduled!=DateTime.MinValue)
                //        droneBL.myParcel.deliveringStatus = false;
                //    if (tempParcel.Requested != DateTime.MinValue)
                //        droneBL.myParcel.deliveringStatus = false;
                //    droneBL.myParcel.weight = tempParcel.Weight;
                //    droneBL.myParcel.priority = tempParcel.Priority;
                //    droneBL.
                //}
            }
            catch (DO.DroneException drEX) //catches DAL exception
            {
                throw new IDNotFound($"Drone ID {id} was not found", drEX);  //throws an BL exception
            }
            return droneBL;

        }
        #endregion

        #region GetParcel
        /// <summary>
        /// This function receives a parcel ID,searches this parcel in the list from DAL and returns a BL parcel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parcel GetParcel(int id)
        {
            Parcel parcelBL = new Parcel();
            parcelBL.Sender = new ClientInParcel();
            parcelBL.Target = new ClientInParcel();
            parcelBL.Drone = new DroneWithParcel();
            try
            {
                DO.Parcel dalParcel = dal.ParcelById(id);  //search the Parcel in the list from DAL
                parcelBL.ID = dalParcel.ID;  //copies all the fields
                parcelBL.Sender.ID = dalParcel.SenderId;
                parcelBL.Target.ID = dalParcel.TargetId;
                parcelBL.Weight = (WeightCategories)dalParcel.Weight;
                parcelBL.Priority = (Priorities)dalParcel.Priority;
                parcelBL.Requested = dalParcel.Requested;
                parcelBL.Scheduled = dalParcel.Scheduled;
                parcelBL.PickedUp = dalParcel.PickedUp;
                parcelBL.Delivered = dalParcel.Delivered;
                parcelBL.Drone.ID = dalParcel.DroneId;
            }
            catch (DO.ParcelException prEX)  //catches the exception from Dal
            {
                throw new IDNotFound($"Parcel ID {id} was not found", prEX);  //throws the BL exception
            }
            return parcelBL;   //returns a BL type parcel 
        }


        #endregion


    }
}