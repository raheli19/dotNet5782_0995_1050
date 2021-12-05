﻿


using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DalObject;
using System.Linq;
using DalObject;

namespace IBL
{

    public partial class BL : IBL
    {
        //-----------------------------------ADD-FUNCTIONS----------------------------------------

        #region addStation
        /// <summary>
        /// This function receives a BL type station, creates a new station in DAL with the same values int the commun fields and adds it in the stationList in DAL
        /// </summary>
        /// <param name="stationBL"></param>
        public void addStation(Station stationBL)
        {
            IDAL.DO.Station stationDAL = new IDAL.DO.Station();  //creates a station from DAL

            stationDAL.Name = stationBL.Name;  //copies the commun fields
            if (!(stationBL.ID <= 99999999 && stationBL.ID > 9999999))
                throw new InputNotValid("ID not valid");
            stationDAL.ID = stationBL.ID;
            if (stationBL.Loc.latitude < 31 || stationBL.Loc.latitude > 33.3)
                throw new InputNotValid("latitude is not valid");
            stationDAL.Latitude = stationBL.Loc.latitude;
            if (stationBL.Loc.longitude < 34.3 || stationBL.Loc.longitude > 35.5)
                throw new InputNotValid("longitude is not valid");
            stationDAL.Longitude = stationBL.Loc.longitude;
            stationDAL.ChargeSlots = stationBL.ChargeSlots;
            try
            {
                p.addStation(stationDAL);
                //p.addStation((IDAL.DO.Station)s.CopyPropertiesToNew(typeof(IDAL.DO.Station)));
            }
            catch (IDAL.DO.StationException ex)  //catches the exception from DAL
            {
                throw new AlreadyExist("Station already exists ", ex);  //throws a BL exception (if needed)

            }

        }
        #endregion

        #region addDrone
        /// <summary>
        /// This function receives a BL drone and a station ID. Adds the drone to the droneList in DAL, to the droneList in BL and associates the drone to its station
        /// </summary>
        /// <param name="d"></param>
        /// <param name="StationID"></param>
        public void addDrone(Drone droneBL, int StationID)

        {
            //Add the drone To Add and check if the inputs are correct
            IDAL.DO.Drone droneDAL = new IDAL.DO.Drone();  //creates a new drone (from DAL)
            double battery = rand.Next(20, 40);// initialize the battery
            Station stationBL = GetStation(StationID);  //finds the station according to its ID

            if (!(droneBL.ID <= 99999999 && droneBL.ID > 9999999)) //ID not Valid
                throw new InputNotValid("ID is not valid");
            droneDAL.ID = droneBL.ID;  //copies all the fields to the DAL drone
            if (droneBL.MaxWeight != WeightCategories.low && droneBL.MaxWeight != WeightCategories.middle && droneBL.MaxWeight != WeightCategories.heavy)
                throw new InputNotValid("The category of your weight is not valid");
            if (droneBL.Status != DroneStatuses.free && droneBL.Status != DroneStatuses.maintenance && droneBL.Status != DroneStatuses.shipping)
                throw new InputNotValid("The status of your drone is not valid");
            droneDAL.Model = droneBL.Model;
            if (battery < 0 || battery > 100)
                throw new InputNotValid("Battery is not valid");

            droneBL.Battery = battery;
            droneBL.Status = DroneStatuses.maintenance;
            droneBL.initialLoc = stationBL.Loc;// his location is the same than the station
            IDAL.DO.DroneCharge DalDC = new IDAL.DO.DroneCharge();
            DalDC.StationId = StationID;
            DalDC.DroneId = droneBL.ID;
            p.AddFromBLDroneCharging(DalDC.DroneId, DalDC.StationId);

            //Create a new DroneDescription and add it TO BL
            DroneDescription DP = new DroneDescription();
            DP.loc = new Localisation();
            DP.Id = droneBL.ID;
            DP.Model = droneBL.Model;
            DP.weight = droneBL.MaxWeight;
            DP.battery = droneBL.Battery;
            DP.Status = DroneStatuses.maintenance;
            DP.loc = droneBL.initialLoc;
            DroneList.Add(DP);
            stationBL.ChargeSlots--;// one more is full
            try
            {
                p.AddDrone(droneDAL);//ADD TO DAL
                //p.AddDrone((IDAL.DO.Drone)d.CopyPropertiesToNew(typeof(IDAL.DO.Drone)));
            }
            catch (IDAL.DO.DroneException ex)
            {
                throw new AlreadyExist("Can't add this drone", ex);
            }
            IDAL.DO.Station dalS = new IDAL.DO.Station();
            dalS = ConvertStationToDal(stationBL);
            p.UpdateStation(dalS);
        }
        #endregion

        #region addClient
        /// <summary>
        /// This function receives a BL client and add it to DAL
        /// </summary>
        /// <param name="c"></param>
        public void addClient(Client clientBL)
        {
            IDAL.DO.Client clientDAL = new IDAL.DO.Client();  //creates a DAL client
            if (!(clientBL.ID <= 99999999 && clientBL.ID > 9999999))  //check the validity of the fields: if one of them is not valid, throws an exception
                throw new InputNotValid("ID not valid");
            clientDAL.ID = clientBL.ID;  //copies all the fieldsto the DAL client
            clientDAL.Name = clientBL.Name;
            if (clientBL.Phone.Length != 10)
                throw new InputNotValid("Phone not valid");
            clientDAL.Phone = clientBL.Phone;
            if (clientBL.ClientLoc.latitude < 31 || clientBL.ClientLoc.latitude > 33.3)
                throw new InputNotValid("latitude is not valid");
            clientDAL.Latitude = clientBL.ClientLoc.latitude;
            if (clientBL.ClientLoc.longitude < 34.3 || clientBL.ClientLoc.longitude > 35.5)
                throw new InputNotValid("longitude is not valid");
            clientDAL.Longitude = clientBL.ClientLoc.longitude;
            try
            {
                p.addClient(clientDAL);  //ADD it to DAL
                //p.addClient((IDAL.DO.Client)c.CopyPropertiesToNew(typeof(IDAL.DO.Client)));
            }
            catch (IDAL.DO.ClientException ex)  //catches a DAL exception
            {
                throw new AlreadyExist("This Client already exists", ex); //throws a BL exception
            }


        }

        #endregion

        #region addParcel
        /// <summary>
        /// This function receives a BL parcel and add it to Dal
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public int addParcel(Parcel parcelBL)
        {
            IDAL.DO.Parcel DALParcel = new IDAL.DO.Parcel(); // creates a new parcel from DAL

            DALParcel.ID = p.RunnerNumber();  //copies all the fields from the parcel he received
            if (!(parcelBL.Sender.ID <= 99999999 && parcelBL.Sender.ID > 9999999))//check the validity of the fields received
                throw new InputNotValid("SenderID not valid");
            DALParcel.SenderId = parcelBL.Sender.ID;
            if (!(parcelBL.Target.ID <= 99999999 && parcelBL.Target.ID > 9999999))
                throw new InputNotValid("TargetID not valid");
            DALParcel.TargetId = parcelBL.Target.ID;
            parcelBL.Scheduled = DateTime.MinValue; //All the dateTimes are initialized with min values except from the dateTime of its creation
            parcelBL.PickedUp = DateTime.MinValue;
            parcelBL.Delivered = DateTime.MinValue;
            parcelBL.Requested = DateTime.Now;
            parcelBL.Drone = null;
            if (parcelBL.Weight != WeightCategories.low && parcelBL.Weight != WeightCategories.middle && parcelBL.Weight != WeightCategories.heavy)
                throw new InputNotValid("The category of your weight is not valid");
            DALParcel.Weight = (IDAL.DO.WeightCategories)parcelBL.Weight;
            if (parcelBL.Priority != Priorities.regular && parcelBL.Priority != Priorities.fast && parcelBL.Priority != Priorities.emergency)
                throw new InputNotValid("The time delivery is not valid");
            DALParcel.Priority = (IDAL.DO.Priorities)parcelBL.Priority;
            DALParcel.Scheduled = parcelBL.Scheduled;
            DALParcel.PickedUp = parcelBL.PickedUp;
            DALParcel.Delivered = parcelBL.Delivered;
            DALParcel.Requested = parcelBL.Requested;
            DALParcel.DroneId = 0;

            try
            {
                p.addParcel(DALParcel);  //Add it to DAL
                //p.addParcel((IDAL.DO.Parcel)pack.CopyPropertiesToNew(typeof(IDAL.DO.Parcel)));
            }
            catch (IDAL.DO.ParcelException ex)
            {
                throw new AlreadyExist("This package already exists", ex);
            }
            return DALParcel.ID;

        }

        #endregion

      
    }    
}
