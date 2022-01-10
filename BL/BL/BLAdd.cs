using System;
using System.Collections.Generic;
using System.Text;
using BO;
using BLApi;
using System.Linq;


namespace BL
{

    sealed partial class BL : IBL
    {
        //-----------------------------------ADD-FUNCTIONS----------------------------------------

        #region AddStation
        /// <summary>
        /// This function receives a BL type station, creates a new station in DAL with the same values int the commun fields and adds it in the stationList in DAL
        /// </summary>
        /// <param name="stationBL"></param>
        public void addStation(Station stationBL)
        {
            DO.Station stationDAL = new DO.Station();  //creates a station from DAL

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
                dal.AddStation(stationDAL);
                //p.addStation((DO.Station)s.CopyPropertiesToNew(typeof(DO.Station)));
            }
            catch (DO.StationException ex)  //catches the exception from DAL
            {
                throw new AlreadyExist("Station already exists ", ex);  //throws a BL exception (if needed)

            }

        }
        #endregion

        #region AddDrone
        /// <summary>
        /// This function receives a BL drone and a station ID. Adds the drone to the droneList in DAL, to the droneList in BL and associates the drone to its station
        /// </summary>
        /// <param name="d"></param>
        /// <param name="StationID"></param>
        public void addDrone(Drone droneBL, int StationID)

        {
            //Add the drone To Add and check if the inputs are correct
            DO.Drone droneDAL = new DO.Drone();  //creates a new drone (from DAL)
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
            DO.DroneCharge DalDC = new DO.DroneCharge();
            DalDC.StationId = StationID;
            DalDC.DroneId = droneBL.ID;
            dal.AddFromBLDroneCharging(DalDC.DroneId, DalDC.StationId);

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
                dal.AddDrone(droneDAL);//ADD TO DAL
                //p.AddDrone((DO.Drone)d.CopyPropertiesToNew(typeof(DO.Drone)));
            }
            catch (DO.DroneException ex)
            {
                throw new AlreadyExist("Can't add this drone", ex);
            }
            DO.Station dalS = new DO.Station();
            dalS = ConvertStationToDal(stationBL);
            dal.UpdateStation(dalS);
        }
        #endregion

        #region AddClient
        /// <summary>
        /// This function receives a BL client and add it to DAL
        /// </summary>
        /// <param name="c"></param>
        public void addClient(Client clientBL)
        {
            DO.Client clientDAL = new DO.Client();  //creates a DAL client
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
                dal.AddClient(clientDAL);  //ADD it to DAL
                //p.addClient((DO.Client)c.CopyPropertiesToNew(typeof(DO.Client)));
            }
            catch (DO.ClientException ex)  //catches a DAL exception
            {
                throw new AlreadyExist("This Client already exists", ex); //throws a BL exception
            }


        }

        #endregion

        #region AddParcel
        /// <summary>
        /// This function receives a BL parcel and add it to Dal
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public int addParcel(Parcel parcelBL)
        {
            DO.Parcel DALParcel = new DO.Parcel(); // creates a new parcel from DAL

            DALParcel.ID = dal.RunnerNumber();  //copies all the fields from the parcel he received
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
            DALParcel.Weight = (DO.WeightCategories)parcelBL.Weight;
            if (parcelBL.Priority != Priorities.regular && parcelBL.Priority != Priorities.fast && parcelBL.Priority != Priorities.emergency)
                throw new InputNotValid("The time delivery is not valid");
            DALParcel.Priority = (DO.Priorities)parcelBL.Priority;
            DALParcel.Scheduled = parcelBL.Scheduled;
            DALParcel.PickedUp = parcelBL.PickedUp;
            DALParcel.Delivered = parcelBL.Delivered;
            DALParcel.Requested = parcelBL.Requested;
            DALParcel.DroneId = 0;

            try
            {
                dal.AddParcel(DALParcel);  //Add it to DAL
                //p.addParcel((DO.Parcel)pack.CopyPropertiesToNew(typeof(DO.Parcel)));
            }
            catch (DO.ParcelException ex)
            {
                throw new AlreadyExist("This package already exists", ex);
            }
            return DALParcel.ID;

        }

        public void RemoveParcel(Parcel parcelToRemove)
        {
            //if (parcelToRemove.Delivered == null)
            //    throw new DO.ParcelException("The parcel is not delivered!");
            DO.Parcel parcelDal = new DO.Parcel();
            parcelDal.ID = parcelToRemove.ID;
            parcelDal.SenderId = parcelToRemove.Sender.ID;
            parcelDal.TargetId = parcelToRemove.Target.ID;
            dal.RemoveParcel(parcelDal);
        }
        #endregion

      
    }    
}
