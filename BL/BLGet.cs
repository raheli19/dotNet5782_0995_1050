
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DalObject;
using System.Linq;

namespace IBL
{

    public partial class BL : IBL
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
                IDAL.DO.Client dalClient = p.ClientById(id);//search it in the clients'list from DAL
                clientBL.ClientLoc = new Localisation();
                clientBL.ID = dalClient.ID; //copies all the fields 
                clientBL.Name = dalClient.Name;
                clientBL.Phone = dalClient.Phone;
                clientBL.ClientLoc.longitude = dalClient.Longitude;
                clientBL.ClientLoc.latitude = dalClient.Latitude;
            }
            catch (IDAL.DO.ClientException custEX)  //catches if there is an exception in DAL
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
                IDAL.DO.Station dalStat = p.StationById(id);  //search the station from DAL
                stationBL.ID = dalStat.ID;  //Copies all the fields from the DAL station
                stationBL.Name = dalStat.Name;
                stationBL.Loc.longitude = dalStat.Longitude;
                stationBL.Loc.latitude = dalStat.Latitude;
                stationBL.ChargeSlots = dalStat.ChargeSlots;
            }
            catch (IDAL.DO.StationException statEX) //catches the DAL exception
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
                IDAL.DO.Drone dalDrone = p.DroneById(id);  //search the drone from DAL
                droneBL.ID = dalDrone.ID;  //copies field by field
                droneBL.Model = dalDrone.Model;
                droneBL.Battery = dalDrone.Battery;

            }
            catch (IDAL.DO.DroneException drEX) //catches DAL exception
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
                IDAL.DO.Parcel dalParcel = p.ParcelById(id);  //search the Parcel in the list from DAL
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
            catch (IDAL.DO.ParcelException prEX)  //catches the exception from Dal
            {
                throw new IDNotFound($"Parcel ID {id} was not found", prEX);  //throws the BL exception
            }
            return parcelBL;   //returns a BL type parcel 
        }


        #endregion


    }
}