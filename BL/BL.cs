//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DAL;
using DalObject;
using System.Linq;

namespace BL
{
    
    public class BL: IBL.IBL
    {
        static Random rand = new Random();

        readonly IDAL.IDal p;
        help h = new help();
       
        List<DroneDescription> DroneList = new List<DroneDescription>();

        public BL() 
        { 
            //constructor of the BL file
            p= new DalObject.DalObject();
            ////List <Drone> Dlist =new List<Drone> (p.DroneList().CopyPropertiesToNew(typeof(IBL.BO)));
            //IBL.BO.DroneDescription Bldr=new IBL.BO.DroneDescription();  // drone from the bl
            //IBL.BO.ParcelToClient ParcelInClient = new IBL.BO.ParcelToClient();
           

            //foreach (var item in p.DroneList())
            //{
            //    Bldr.Id = item.ID;
            //    Bldr.Model = item.Model;
            //    Bldr.battery = item.Battery;
                
            //    foreach (var item2 in p.ParcelList())
            //    {
            //        if (item2.DroneId == item.ID) //gets all the parcel's info from the dal
            //        {
            //            ParcelInClient.ID = item2.ID;
            //            ParcelInClient.weight = (WeightCategories)item2.Weight;
            //            ParcelInClient.priority = (Priorities)item2.Priority;
            //            if (item2.Requested != DateTime.MinValue)
            //                ParcelInClient.Status = ParcelStatus.requested;
            //            if(item2.Scheduled!= DateTime.MinValue)
            //                ParcelInClient.Status = ParcelStatus.scheduled;
            //            if (item2.PickedUp!= DateTime.MinValue)
            //                ParcelInClient.Status = ParcelStatus.pickedup;
            //            if (item2.Delivered != DateTime.MinValue)
            //                ParcelInClient.Status = ParcelStatus.delivered;
                        
            //        }
            //    }
            //    //ParcelInclient is the parcel associated to the drone
            //    //double parLongitude = p.ClientById(ParcelInClient.client.ID).Longitude; 
            //    //double parLatitude = p.ClientById(ParcelInClient.client.ID).Latitude; 
            //    double senderLat = default;
            //    double senderLong = default;
            //    double targetLat = default;
            //    double targetLong = default;
            //    int senderId1 = default;
            //    int targetId1 = default;
                
            //    try
            //    {
            //        //senderLat = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Latitude;
            //        //senderLong = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Longitude;
            //        //targetLat = p.ClientById((p.ParcelById(ParcelInClient.ID)).TargetId).Longitude;
            //        //targetLong = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Latitude;
            //        try
            //        {
            //            senderId1=( p.ParcelById(ParcelInClient.ID)).SenderId;
            //            targetId1 = (p.ParcelById(ParcelInClient.ID)).SenderId;
            //        }
            //        catch(IDAL.DO.ParcelException ex)
            //        {
            //            throw new IBL.BO.IDNotFound("Not found", ex);
            //        }

            //        senderLat = p.ClientById(senderId1).Latitude;
            //        senderLong = p.ClientById(senderId1).Longitude;
            //        targetLat = p.ClientById(targetId1).Latitude;
            //        targetLong = p.ClientById(targetId1).Longitude;
            //    }
            //    catch (IDAL.DO.ClientException ex)
            //    {
            //        /throw new IBL.BO.NotFound("Not found", ex);
            //    }

               
            //    // the drone is shipping
            //    if (ParcelInClient.Status != ParcelStatus.delivered)// the parcel has not been delivered yet.
            //        {
            //            Bldr.Status = DroneStatuses.shipping;
            //        Bldr.loc = new Localisation();
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
                
            //    // if drone isn't shipping
            //    else if (ParcelInClient.Status == ParcelStatus.delivered || ParcelInClient.Status == ParcelStatus.scheduled)// the drone isn't shipping
            //    {
            //        int random = (rand.Next(0, 1))*2;
            //        if (random == 0)
            //        {
            //            Bldr.Status = DroneStatuses.free;
            //        }
            //        if (random == 2)
            //        {
            //            Bldr.Status = DroneStatuses.shipping;
            //        }
            //    }
            //    // if the drone is charging
            //    if (Bldr.Status==DroneStatuses.maintenance)
            //    {
            //        // random localisation entre les differentes stations
            //        List<int> helplist = p.IdStation();
            //        int index = rand.Next(helplist.Count);
            //        int s = helplist[index];
            //        IDAL.DO.Station stat = p.StationById(s);
            //        Bldr.loc = location(stat.Latitude, stat.Longitude);
            //        Bldr.battery = h.getRandomNumber(0, 20);

            //    }
                
            //    //the drone is free
            //    else if (Bldr.Status==DroneStatuses.free)
            //     {
                    
            //        // location of one of the client that have received a parcel
            //        List<int> helpList = p.clientReceivedParcel();
            //        int index = rand.Next(helpList.Count);
            //        int s = helpList[index];
            //        try
            //        {
            //            IDAL.DO.Client c = p.ClientById(s);


            //            Bldr.loc = location(c.Latitude, c.Longitude);
            //        }
            //        catch(IDAL.DO.ClientException ex)
            //        {
            //            throw new IBL.BO.NotFound("Didnt find",ex);
            //        }

            //        Station chargeStat = NearestStation(Bldr.loc, true);
            //        double statDist = distance(chargeStat.Loc.latitude, chargeStat.Loc.longitude, Bldr.loc.latitude, Bldr.loc.longitude);
            //        double minBattery = BatteryAccToDistance(statDist);
            //        Bldr.battery = h.getRandomNumber(minBattery, 100.0);
                    
            //    }
            //    Bldr.weight = ParcelInClient.weight;
            //    Bldr.Status = (DroneStatuses)ParcelInClient.Status;
                

            //}

        }
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
            catch(IDAL.DO.ClientException custEX)  //catches if there is an exception in DAL
            {
                throw new IBL.BO.IDNotFound($"Client ID {id} was not found", custEX); //throws if there is an exception in BL
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
                throw new IBL.BO.IDNotFound($"Station ID {id} was not found", statEX); //throws an BL exception
            }
            return stationBL;
        }
        #endregion

        #region GetDrone
        /// <summary>
        /// This function receives a drone ID, searches it in the drone list in DAL and returns a BL drone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                throw new IBL.BO.IDNotFound($"Drone ID {id} was not found", drEX);  //throws an BL exception
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
                throw new IBL.BO.IDNotFound($"Parcel ID {id} was not found", prEX);  //throws the BL exception
            }
            return parcelBL;   //returns a BL type parcel 
        }


        #endregion

        //-----------------------------------ADD-FUNCTIONS----------------------------------------

        #region addStation
        /// <summary>
        /// This function receives a BL type station, creates a new station in DAL with the same values int the commun fields and adds it in the stationList in DAL
        /// </summary>
        /// <param name="stationBL"></param>
        public void addStation(IBL.BO.Station stationBL)   
        {
            IDAL.DO.Station stationDAL = new IDAL.DO.Station();  //creates a station from DAL

            stationDAL.Name = stationBL.Name;  //copies the commun fields
            if (!(stationBL.ID <= 99999999 && stationBL.ID > 9999999))
                throw new IBL.BO.InputNotValid("ID not valid");
            stationDAL.ID = stationBL.ID;
            if (stationBL.Loc.latitude< 31|| stationBL.Loc.latitude>33.3)
                throw new IBL.BO.InputNotValid("latitude is not valid");
            stationDAL.Latitude = stationBL.Loc.latitude;
            if (stationBL.Loc.longitude<34.3|| stationBL.Loc.longitude>35.5)
                throw new IBL.BO.InputNotValid("longitude is not valid");
            stationDAL.Longitude = stationBL.Loc.longitude;
            stationDAL.ChargeSlots = stationBL.ChargeSlots;
            try
            {
                p.addStation(stationDAL);
                //p.addStation((IDAL.DO.Station)s.CopyPropertiesToNew(typeof(IDAL.DO.Station)));
            }
            catch(IDAL.DO.StationException ex)  //catches the exception from DAL
            {
                throw new IBL.BO.AlreadyExist("Station already exists ", ex);  //throws a BL exception (if needed)

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
            droneBL.Battery = rand.Next(20, 40);// initialize the battery
            Station stationBL = GetStation(StationID);  //finds the station according to its ID
            
            if (!(droneBL.ID <= 99999999 && droneBL.ID > 9999999)) //ID not Valid
                throw new IBL.BO.InputNotValid("ID is not valid");
            droneDAL.ID = droneBL.ID;  //copies all the fields to the DAL drone
            if (droneBL.MaxWeight != WeightCategories.low && droneBL.MaxWeight != WeightCategories.middle && droneBL.MaxWeight != WeightCategories.heavy)
               throw new IBL.BO.InputNotValid("The category of your weight is not valid");
            if (droneBL.Status != DroneStatuses.free && droneBL.Status != DroneStatuses.maintenance && droneBL.Status != DroneStatuses.shipping)
                throw new IBL.BO.InputNotValid("The status of your drone is not valid");
            droneDAL.Model = droneBL.Model;
            if (droneBL.Battery < 0 || droneBL.Battery > 100)
                throw new IBL.BO.InputNotValid("Battery is not valid");
            droneDAL.Battery = droneBL.Battery;

            droneBL.Status = DroneStatuses.maintenance;
            droneBL.initialLoc = stationBL.Loc;// his location is the same than the station
            IDAL.DO.DroneCharge DalDC = new IDAL.DO.DroneCharge();
            DalDC.StationId = StationID;
            DalDC.DroneId = droneBL.ID;
            p.AddFromBLDroneCharging(DalDC.DroneId,DalDC.StationId);
          
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
            catch(IDAL.DO.DroneException ex)
            {
                throw new AlreadyExist("Can't add this drone", ex);
            }
            IDAL.DO.Station dalS= new IDAL.DO.Station();
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
            IDAL.DO.Client clientDAL= new IDAL.DO.Client();  //creates a DAL client
            if (!(clientBL.ID <= 99999999 && clientBL.ID > 9999999))  //check the validity of the fields: if one of them is not valid, throws an exception
                throw new IBL.BO.InputNotValid("ID not valid");
            clientDAL.ID = clientBL.ID;  //copies all the fieldsto the DAL client
            clientDAL.Name = clientBL.Name;
            if (clientBL.Phone.Length!=11)
                throw new IBL.BO.InputNotValid("Phone not valid");
            clientDAL.Phone = clientBL.Phone;
            if (clientBL.ClientLoc.latitude < 31 || clientBL.ClientLoc.latitude > 33.3)
                throw new IBL.BO.InputNotValid("latitude is not valid");
            clientDAL.Latitude = clientBL.ClientLoc.latitude;
            if (clientBL.ClientLoc.longitude < 34.3 || clientBL.ClientLoc.longitude > 35.5)
                throw new IBL.BO.InputNotValid("longitude is not valid");
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
            DALParcel.ID = parcelBL.ID;  //copies all the fields from the parcel he received
            if (!(parcelBL.Sender.ID <= 99999999 && parcelBL.Sender.ID > 9999999))//check the validity of the fields received
                throw new IBL.BO.InputNotValid("SenderID not valid");
            DALParcel.SenderId = parcelBL.Sender.ID;
            if (!(parcelBL.Target.ID <= 99999999 && parcelBL.Target.ID > 9999999))
                throw new IBL.BO.InputNotValid("TargetID not valid");
            DALParcel.TargetId = parcelBL.Target.ID;
            parcelBL.Scheduled = DateTime.MinValue; //All the dateTimes are initialized with min values except from the dateTime of its creation
            parcelBL.PickedUp = DateTime.MinValue;
            parcelBL.Delivered = DateTime.MinValue;
            parcelBL.Requested = DateTime.Now;
            parcelBL.Drone = null;
            if (parcelBL.Weight != WeightCategories.low && parcelBL.Weight != WeightCategories.middle && parcelBL.Weight != WeightCategories.heavy)
                throw new IBL.BO.InputNotValid("The category of your weight is not valid");
            DALParcel.Weight = (IDAL.DO.WeightCategories)parcelBL.Weight;
            if (parcelBL.Priority != Priorities.regular && parcelBL.Priority != Priorities.fast && parcelBL.Priority != Priorities.emergency)
                throw new IBL.BO.InputNotValid("The time delivery is not valid");
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
            catch(IDAL.DO.ParcelException ex)
            {
                throw new AlreadyExist("This package already exists", ex);
            }
            return DALParcel.ID;
        
        }

        #endregion

        //----------------------------------UPDATE-FUNCTIONS-------------------------------------
       
        #region Update DroneName
        /// <summary>
        /// This function receives a drone ID and a new Model. Associates this new model to the drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newModel"></param>
        public void updateDroneName(int droneId, string newModel) 
        {
            try
            {
                IDAL.DO.Drone droneDAL = p.DroneById(droneId); // find the DALdrone
                droneDAL.Model = newModel;  //changes its model
                p.UpdateDrone(droneDAL); // this function update the DAL drones' list

            }
            catch(IDAL.DO.DroneException ex)  //catches the DAL exception if there is one
            {
                throw new IBL.BO.IDNotFound("Can't update the name", ex); //throws a BL exception
            }
        
        }
        #endregion

        #region Update StationName
        /// <summary>
        /// This function receives a station ID, a new name and a new number of chargeSlots. Updates one or both of them according to the user
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newName"></param>
        /// <param name="newCS"></param>
        public void updateStationName_CS(int stationId, int newName = -1, int newCS = -1)
        {
            try
            {
                IDAL.DO.Station stationDAL = p.StationById(stationId);  //finds the station according to its ID
                if (newName != -1)  //if the user wants to update the station's name
                    stationDAL.Name = newName;
                if (newCS != -1)    //if the user wants to update the station's number of chargeSlots
                    stationDAL.ChargeSlots = newCS;
                int notFreeChargeSlot = 0;
                IEnumerable<IDAL.DO.DroneCharge> listDroneCharge = p.DroneChargeList();
                foreach(var item in listDroneCharge) // calculates the charge slot that not free.
                {
                    if (item.StationId == stationId)
                        notFreeChargeSlot++;
                }
                if (notFreeChargeSlot > newCS) // if new all charge slot count lower then the catch slots that alraedy exist.
                    throw new IBL.BO.WrongDetailsUpdateException("All charge slot is to low");
                stationDAL.ChargeSlots = newCS - notFreeChargeSlot;

                p.UpdateStation(stationDAL);  // updates the station in the stations' list (DAL)
            }
            catch (IDAL.DO.StationException ex)   //catches the Dal exception
            {
                throw new IBL.BO.IDNotFound ("Can't update the station name and/or the number of chargeSlots", ex);  //throws a BL exception
            }
            

        }
        #endregion

        #region Update ClientName_Phone
        /// <summary>
        /// This function receives a client ID, a new name and a new phone number. The user decides if he want to update those fields or not
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newName"></param>
        /// <param name="newTel"></param>
        public void updateClientName_Phone(int clientId, string newName = "n", string newTel = "n") 
        {
            try
            {
                IDAL.DO.Client clientDAL = p.ClientById(clientId);  //search the client in dal
                if (newName != "n")  //if the user wants to update the client's name
                {
                    clientDAL.Name = newName;
                }
                p.UpdateClient(clientDAL);  //updates the client in dal
            }
            catch(IDAL.DO.ClientException ex)  //catches DAL exception
            {
                throw new IBL.BO.IDNotFound("Can't update the client name",ex);  //throw a BL exception
            }
        
        }
        #endregion

        #region DroneToCharge
        public void DroneToCharge(int DroneId)
        {

            DroneDescription blDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            blDrone.loc = new Localisation();

            if (blDrone == null)
            {
                throw new IBL.BO.NotFound("Drone not found");
            }
            if (blDrone.Status == DroneStatuses.free)
            {
                Station nearestStation = NearestStation(blDrone.loc, true);
                double d = distance(blDrone.loc.latitude, blDrone.loc.longitude, nearestStation.Loc.latitude, nearestStation.Loc.longitude);
                bool canGoToCharge = false;
                if (DistanceAccToBattery(blDrone.battery) >= d)
                    canGoToCharge = true;
                else
                    throw new IBL.BO.NotEoughBattery("Can not send the drone to the station, it doesn't have enough battery!");
                if (canGoToCharge == true) 
                {
                    blDrone.battery -= BatteryAccToDistance(DistanceAccToBattery(blDrone.battery));// substract the account of percetn from the battery to go to the nearest station
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD = blDrone;
                    tempDD.loc = nearestStation.Loc;
                    tempDD.Status = DroneStatuses.maintenance;
                    DroneList.Remove(blDrone);
                    DroneList.Add(tempDD);
                    try
                    { 
                        p.AddFromBLDroneCharging(blDrone.Id, nearestStation.ID);//ADD The DroneCharge(drone+station) to DAL 
                    }
                    catch(IDAL.DO.DroneException ex)
                    {
                        throw new IBL.BO.CannotAdd("The droneCharge cannot be added to DAL", ex);
                    }
                    IDAL.DO.Station stationDAL = new IDAL.DO.Station();
                    stationDAL.ID = nearestStation.ID;
                    stationDAL.Name = nearestStation.Name;
                    stationDAL.Latitude = nearestStation.Loc.latitude;
                    stationDAL.Longitude = nearestStation.Loc.longitude;
                    stationDAL.ChargeSlots = nearestStation.ChargeSlots -1;
                    try { 
                    p.UpdateStation(stationDAL);// update the station in the dal list
                     }
                    catch(IDAL.DO.StationException ex)
                    {
                        throw new IBL.BO.CannotUpdate("The station cannot be updated", ex);
                    }

                    
                }

            }
            else
                throw new IBL.BO.NotAvailable("The drone isn't avaiblable!");
        }
        #endregion

        #region DroneCharged
        public void DroneCharged(int ID, double time)
        {
            DroneDescription droneBL = new DroneDescription();
            droneBL.loc = new Localisation();
            droneBL.Id = ID;
            droneBL.Status = DroneStatuses.free; // signs if we don't find it
            foreach(var item in DroneList)// get all the information about this drone from the dronelist
            {
                if(item.Id==ID)
                {
                    droneBL.Status = item.Status;
                    droneBL.Model = item.Model;
                    droneBL.weight = item.weight;
                    droneBL.loc = item.loc;
                    droneBL.DeliveredParcels = item.DeliveredParcels;
                    // don't copy the battery
                    //DroneList.Remove(item);// delete the drone from the list
                }
            }
            if (droneBL.Status != DroneStatuses.maintenance)// if the drone is not charging
                throw new IBL.BO.InputNotValid("This drone isn't charging");

            // upadte the drone in the bl DroneList
            droneBL.Status = DroneStatuses.free;
            droneBL.battery = BatteryAccToTime(time);
            //DroneList.updateBlDroneList(myDr);

            //update the drone in the droneList from the DAL
            try
            {
                IDAL.DO.Drone droneDAL = p.DroneById(ID);
                droneDAL.Battery = BatteryAccToTime(time);
                p.UpdateDrone(droneDAL);
            }
            catch(IDAL.DO.DroneException)
            {
                throw new IBL.BO.CannotUpdate("Can't update the drone");
            }

            //update station
            IDAL.DO.Station stationDAL = new IDAL.DO.Station();
            try
            {
               stationDAL = p.StationById(ID);
            }
            catch (IDAL.DO.StationException ex)
            {
                throw new IBL.BO.IDNotFound("ID Not found", ex);
            }
            foreach (var item in p.StationList())
            {
                if (item.Longitude == droneBL.loc.longitude && item.Latitude == droneBL.loc.latitude)
                {
                    stationDAL.ID = item.ID;
                    stationDAL.Name = item.Name;
                    stationDAL.Latitude = item.Latitude;
                    stationDAL.Longitude = item.Longitude;
                    stationDAL.ChargeSlots = item.ChargeSlots;
                }                   
            }
            stationDAL.ChargeSlots++;
            try
            {
                p.UpdateStation(stationDAL); // puts back the station with one more chargeSlot free
            }
            catch (IDAL.DO.StationException ex)
            {
                throw new IBL.BO.CannotUpdate("The station cannot be updated", ex);
            }
            foreach (var item in p.DroneChargeList())
            {
                if (item.DroneId == droneBL.Id && item.StationId == stationDAL.ID)
                    p.updateDroneChargeList(droneBL.Id, stationDAL.ID);

            }



        }
        #endregion

        #region Assignement
        public void Assignement(int ID, int i=2)
        {
            IDAL.DO.Drone droneDAL = p.DroneById(ID); //from the dal
            DroneDescription droneBL = DroneList.First(x => x.Id == ID);// finds the drone
            droneBL.loc = new Localisation();

            if (droneBL.Status != DroneStatuses.free)
                throw new NotAvailable("The drone is not free!");
            IDAL.DO.Parcel parcelDAL = new IDAL.DO.Parcel(); // dalparcel
            bool flag = false;

            foreach (var item in p.ParcelList())
            {
                if (item.Priority != IDAL.DO.Priorities.emergency - i)
                    continue;
                if (item.Weight == (IDAL.DO.WeightCategories)droneBL.weight)///////////////
                    continue;
                parcelDAL.DroneId = droneBL.Id;
                parcelDAL.ID = item.ID;
                parcelDAL.Priority = item.Priority;
                parcelDAL.SenderId = item.SenderId;
                parcelDAL.TargetId = item.TargetId;
                parcelDAL.Weight = item.Weight;
                parcelDAL.Requested = DateTime.Now;

                List<IDAL.DO.Parcel> parcelLst = p.ParcelList().ToList().FindAll(x => (int)x.Priority == i && x.Scheduled == DateTime.MinValue);// list with all the emergency ones
                parcelLst = parcelLst.FindAll(x => (int)x.Weight == (int)droneBL.weight);  //list with all the same weights

                IDAL.DO.Parcel closestParcel = ClosestParcel(parcelLst, droneBL.loc); 

                double senderLat = p.FindLat(closestParcel.SenderId);
                double senderLong = p.FindLong(closestParcel.SenderId);
                double targetLat = p.FindLat(closestParcel.TargetId);
                double targetLong = p.FindLong(closestParcel.TargetId);
                //trouver la plus proche
                // has to be able to make it
                double distToSender = distance(droneBL.loc.latitude, droneBL.loc.longitude, senderLat, senderLong);
                droneBL.battery -= BatteryAccToDistance(distToSender);
                if (distToSender >= DistanceAccToBattery(droneBL.battery))
                    continue;
                //doit repartir

                double distToTarget = distance(droneBL.loc.latitude, droneBL.loc.longitude, targetLat, targetLong);
                droneBL.battery -= BatteryAccToDistance(distToTarget);
                if (distToTarget >= DistanceAccToBattery(droneBL.battery))
                    continue;
                droneBL.battery -= BatteryAccToDistance(distToTarget);
                // if drone's battery less than 25 has to go to charge
                if (droneBL.battery <= 25)
                {
                    Localisation targLoc = location(targetLat, targetLong);
                    Station nearStat = NearestStation(targLoc, false); //finds the nearest station if need to charge
                    double distToStat = distance(droneBL.loc.latitude, droneBL.loc.longitude, nearStat.Loc.latitude, nearStat.Loc.longitude);
                    if (distToStat > BatteryAccToDistance(distToStat))
                        continue;
                }
                flag = true;

                if (flag == false && i == 3) // we didn't find one
                    throw new ParcelException("We didn't find a parcel");
                else if (flag == false)
                    Assignement(ID, i - 1); // calls back the function to check with an other status
                droneBL.Status = DroneStatuses.shipping;
                DroneList.Add(droneBL);
                p.Assignement(closestParcel.ID, droneBL.Id); //update the dronelist in the dal
            }
        }
        #endregion
        
        //---------------------------------------ACTIONS------------------------------------------

        #region PickedUp
        /// <summary>
        /// This function gets a droneId,finds the drone and this drone picks the parcel associated to it
        /// </summary>
        /// <param name="DroneId"></param>
        public void PickedUp(int droneId)
        {
            Drone collectDrone = new Drone();
            try
            {
                collectDrone = GetDrone(droneId);
            }
            catch (IDAL.DO.DroneException ex)
            {
                throw new IDAL.DO.DroneException("" + ex);
            }

            if (collectDrone.Status != DroneStatuses.shipping) //if the drone is not scheduled
                throw new WrongDetailsUpdateException("Drone is not scheduled to any parcel");
            if (collectDrone.myParcel.deliveringStatus == true) //if parcel is already on the drone
                throw new WrongDetailsUpdateException("Drone in the middle of shipping");

            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == droneId);  //finds the drone in the droneList in BL
            int index= DroneList.FindIndex(Drone => Drone.Id == droneId);
            myDrone.loc = new Localisation();
            collectDrone.initialLoc = new Localisation();
            if (myDrone == null)  //the drone is not among the drone List
            {
                throw new IBL.BO.IDNotFound("Drone not found");  //throws a BL exception
            }
            try
            {
                DroneDescription updateDrone = DroneList[index];
                updateDrone.battery -= BatteryAccToDistance(distance(collectDrone.initialLoc.latitude, collectDrone.initialLoc.longitude,collectDrone.myParcel.picking.latitude,collectDrone.myParcel.picking.longitude) );
                updateDrone.loc= collectDrone.myParcel.picking;
                DroneList[index] = updateDrone;

                IDAL.DO.Parcel parcelDal = p.ParcelById(collectDrone.myParcel.ID);
                parcelDal.PickedUp = DateTime.Now;
                p.UpdateParcelFromBL(parcelDal);
                //if ((parcelDal.Requested == DateTime.Now || parcelDal.Scheduled == DateTime.Now) && (parcelDal.PickedUp == DateTime.MinValue))
                //{
                //    int senderId = parcelDal.SenderId;
                //    Localisation senderLoc = new Localisation();
                //    senderLoc.latitude = p.FindLat(senderId);
                //    senderLoc.longitude = p.FindLong(senderId);
                //    double myDistance = distance(myDrone.loc.latitude, myDrone.loc.longitude, senderLoc.latitude, senderLoc.longitude);
                //    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                //    tempDD.loc = new Localisation();
                //    tempDD = myDrone;
                //    tempDD.battery -= BatteryAccToDistance(myDistance);
                //    tempDD.loc = senderLoc;
                //    DroneList.Remove(myDrone);
                //    DroneList.Add(tempDD);
                //    IDAL.DO.Parcel tempParcel = new IDAL.DO.Parcel();
                //    tempParcel = parcelDal;
                //    tempParcel.PickedUp = DateTime.Now;
                //    p.AddParcelFromBL(tempParcel);
                //}
            }
            catch(Exception ex)
            {
                throw new IBL.BO.InputNotValid("The parcel doesn't fit the requirements",ex);
            }


        }
        #endregion

        #region Delivered
        public void delivered(int DroneId)
        {
            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new IBL.BO.NotFound("Drone not found");
            }
            try
            {
                IDAL.DO.Parcel prcel = p.FindParcelAssociatedWithDrone(DroneId);


                if (prcel.PickedUp == DateTime.Now && prcel.Delivered == DateTime.MinValue)
                {

                    int targetId = prcel.TargetId;
                    Localisation targetLoc = new Localisation();
                    targetLoc.latitude = p.FindLat(targetId);
                    targetLoc.longitude = p.FindLong(targetId);
                    double myDistance = distance(myDrone.loc.latitude, myDrone.loc.longitude, targetLoc.latitude, targetLoc.longitude);
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD.loc = new Localisation();
                    tempDD = myDrone;
                    tempDD.battery -= BatteryAccToDistance(myDistance);
                    tempDD.loc = targetLoc;
                    tempDD.Status = DroneStatuses.free;
                    DroneList.Remove(myDrone);
                    DroneList.Add(tempDD);
                    IDAL.DO.Parcel tempParcel = new IDAL.DO.Parcel();
                    tempParcel = prcel;
                    tempParcel.Delivered = DateTime.Now;
                    p.AddParcelFromBL(tempParcel);

                }

            }
            catch(Exception ex)
            {
                throw new IBL.BO.InputNotValid("The drone doesn't fit the requirements",ex);
            }
        }
        #endregion

        //-----------------------------------PRINT-FUNCTIONS--------------------------------------
        #region PRINTING

        #region displayStation
        public Station displayStation(int stationId) 
        {
            Station s = GetStation(stationId);  //recupere les donnees de la DAL
                                                //the only field missing is the list of drones
            List<DroneCharging> droneCharging = new List<DroneCharging>();
            List<int> DronesID = new List<int>();
           
            //List<IDAL.DO.DroneCharge> DalList = new List<IDAL.DO.DroneCharge>();
            //foreach (var item in DalList)
            //{
            //    if (item.StationId == stationId)
            //    {
            //        DroneCharging tempDC = new DroneCharging();
            //        tempDC.ID = item.DroneId;
            //        try
            //        {
            //            tempDC.battery = (p.DroneById(item.DroneId)).Battery;
            //        }
            //        catch (IDAL.DO.DroneException ex)
            //        {
            //            throw new IBL.BO.IDNotFound("Not found", ex);
            //        }
            //        droneCharging.Add(tempDC);
            //    }
            //}


            IEnumerable<IDAL.DO.DroneCharge> droneCharges = p.DroneChargeList();
            foreach (var item in droneCharges)
            {
                if (item.StationId == s.ID)
                {
                    Drone drone = GetDrone(item.DroneId);
                    s.DroneCharging.Add(new DroneCharging() { ID = item.DroneId, battery = drone.Battery, });
                    

                }
            }
            return s;
        }
        #endregion

        #region displayDrone
        public Drone displayDrone(int droneId) 
        {
            Drone d = GetDrone(droneId); //Copies the fields from DAL
            //the missing fields are:MaxWeight,Status,initialLoc,ParcelIndelivering
            DroneDescription tmp = DroneList.Find(x => x.Id == droneId);
            d.initialLoc = new Localisation();
            d.MaxWeight = tmp.weight;
            d.Status = tmp.Status;
            d.initialLoc = tmp.loc;
            ParcelInDelivering PID = new ParcelInDelivering();
            if (d.Status == DroneStatuses.shipping)
            {
                foreach(var item in p.ParcelList())
                {
                    try
                    {
                        if (item.DroneId == droneId)
                        {
                            PID.ID = item.ID;
                            PID.weight = (WeightCategories)item.Weight;
                            PID.priority = (Priorities)item.Priority;
                            ClientInParcel sender = new ClientInParcel();
                            ClientInParcel target = new ClientInParcel();
                            sender.ID = item.SenderId;
                            sender.name = p.ClientById(item.SenderId).Name;
                            target.ID = item.TargetId;
                            target.name = p.ClientById(item.TargetId).Name;
                            PID.Sender = sender;
                            PID.Target = target;
                            Localisation pickLoc = new Localisation();
                            Localisation delLoc = new Localisation();
                            pickLoc.latitude = p.ClientById(item.SenderId).Latitude;
                            pickLoc.longitude = p.ClientById(item.SenderId).Longitude;
                            delLoc.latitude = p.ClientById(item.TargetId).Latitude;
                            delLoc.longitude = p.ClientById(item.TargetId).Longitude;
                            PID.picking = pickLoc;
                            PID.delivered = delLoc;
                            //calculer la distance de transport
                        }
                    }
                    catch (Exception ex)                                                                                                                                                                                                     
                    {
                        throw new IDNotFound("The id was not found", ex);
                    }
                }
            }
            d.myParcel = PID;
            return d;
        }
        #endregion

        #region displayClient
        public Client displayClient(int clientId) 
        {
            Client c = GetClient(clientId);   //Copies the fields from DAL

            List<ParcelToClient> TempParcLstFromClient = new List<ParcelToClient>();

            foreach (var item in p.ParcelList())
            {
                if(item.SenderId==clientId)  //The parcel has been sent by this client
                {
                    ParcelToClient PCT = new ParcelToClient();
                    PCT.ID = item.ID;
                    PCT.weight = (WeightCategories)item.Weight;
                    PCT.priority = (Priorities)item.Priority;
                    if (item.Requested == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.requested;
                    }
                    else if (item.Scheduled == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.scheduled;
                    }
                    else if (item.PickedUp == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.pickedup;
                    }
                    else if (item.Delivered == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.delivered;
                    }
                    ClientInParcel myClient = new ClientInParcel();
                    myClient.ID = clientId;
                    try
                    {
                        myClient.name = p.ClientById(clientId).Name;
                    }
                    catch (IDAL.DO.ClientException ex)
                    {
                        throw new IBL.BO.IDNotFound("The client doesnt exist", ex);
                    }
                    PCT.client = myClient;
                    TempParcLstFromClient.Add(PCT);

                }
            }
            c.ParcLstFromClient = TempParcLstFromClient;


            List<ParcelToClient> TempParcLstToClient = new List<ParcelToClient>();

            foreach (var item in p.ParcelList())
            {
                if (item.TargetId == clientId)  //The parcel has been sent by this client
                {
                    ParcelToClient PCT = new ParcelToClient();
                    PCT.ID = item.ID;
                    PCT.weight = (WeightCategories)item.Weight;
                    PCT.priority = (Priorities)item.Priority;
                    if (item.Requested == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.requested;
                    }
                    else if (item.Scheduled == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.scheduled;
                    }
                    else if (item.PickedUp == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.pickedup;
                    }
                    else if (item.Delivered == DateTime.Now)
                    {
                        PCT.Status = ParcelStatus.delivered;
                    }
                    ClientInParcel myClient = new ClientInParcel();
                    myClient.ID = clientId;
                    try
                    {
                        myClient.name = p.ClientById(clientId).Name;
                    }
                    catch (IDAL.DO.ClientException ex)
                    {
                        throw new IDNotFound("The client doesnt exist", ex);
                    }
                    PCT.client = myClient;
                    TempParcLstToClient.Add(PCT);

                }
            }

            c.ParcLstToClient = TempParcLstToClient;

            return c;
        
        }
        #endregion

        #region displayParcel
        public Parcel displayParcel(int parcelId) 
        {
            Parcel prcl = GetParcel(parcelId);
            //The missing fields are Sender(ClientInParcel),Target(ClientInParcel) and Drone(droneWithParcel)

            ClientInParcel tempSender = new ClientInParcel();
            ClientInParcel tempTarget = new ClientInParcel();
            DroneWithParcel tempDrone = new DroneWithParcel();

            tempDrone.departureLoc = new Localisation();
            tempSender.ID = prcl.Sender.ID;
            try
            {
                tempSender.name = p.ClientById(tempSender.ID).Name;
                prcl.Sender = tempSender;

                tempTarget.ID = prcl.Target.ID;
                tempTarget.name = p.ClientById(tempTarget.ID).Name;
                prcl.Target = tempTarget;

                tempDrone.ID = prcl.Drone.ID;
                tempDrone.battery = p.DroneById(tempDrone.ID).Battery;
            }
            catch (Exception ex)
            {
                throw new IBL.BO.IDNotFound("Not found", ex);
            }
            foreach(var item in DroneList)
            {
                if (item.Id == tempDrone.ID)
                {
                    tempDrone.departureLoc = item.loc;
                }
            }
            prcl.Drone = tempDrone;
            return prcl;

        }
        #endregion

        #region IENUMERABLE 
        public IEnumerable<StationDescription> DisplayStationList() 
        {
            
            List<StationDescription> statList = new List<StationDescription>();
            foreach(var item in p.StationList())
            {
                StationDescription statD = new StationDescription();
                statD.Id = item.ID;
                statD.name = item.Name;
                foreach( var item2 in p.DroneChargeList())// full chargeSlots
                {
                    if (item2.StationId == item.ID) // if the drone is in the station
                    { 
                        statD.fullChargeSlots++;
                        
                    }

                }

                statD.freeChargeSlots = item.ChargeSlots;// free ones
                statList.Add(statD);// add it to the list

            }
            return statList;
        }
        public  IEnumerable<DroneDescription> displayDroneList() 
        {
            return DroneList;

        }
        public IEnumerable<ClientActions> displayClientList() 
        {
            List<ClientActions> LstCA = new List<ClientActions>();
            
            foreach(var item in p.ClientList())
            {
                ClientActions tempCA = new ClientActions();
                tempCA.Id = item.ID;
                tempCA.name = item.Name;
                tempCA.phone = item.Phone;
                IEnumerable<IDAL.DO.Parcel> sent_and_delivLst = p.ParcelList().Where(x=>x.SenderId==item.ID&&x.Delivered!=DateTime.MinValue);
                IEnumerable<IDAL.DO.Parcel> sent_and_notDelivLst = p.ParcelList().Where(x => x.SenderId == item.ID && x.Delivered == DateTime.MinValue);
                IEnumerable<IDAL.DO.Parcel> receivLst = p.ParcelList().Where(x => x.TargetId == item.ID && x.Delivered != DateTime.MinValue);
                IEnumerable<IDAL.DO.Parcel> receivingLst = p.ParcelList().Where(x => x.TargetId == item.ID && x.Delivered == DateTime.Now);
                int sent_and_delivLstCount = sent_and_delivLst.Count();
                int sent_and_notDelivLstCount = sent_and_notDelivLst.Count();
                int receivLstCount = receivLst.Count();
                int receivingLstCount = receivingLst.Count();
                tempCA.deliveredParcels = sent_and_delivLstCount;
                tempCA.deliveringParcels = sent_and_notDelivLstCount;
                tempCA.receivedParcels = receivLstCount;
                tempCA.receivingParcels = receivingLstCount;
                LstCA.Add(tempCA);
            }
            return LstCA;
        }
        public IEnumerable<ParcelDescription> displayParcelList() 
        {
            //id 
            //sendername
            //targetname
            //weight
            //time
            //matsav havila
            List<ParcelDescription> parList = new List<ParcelDescription>();
            
            foreach(var item in p.ParcelList())
            {
                ParcelDescription tempPar = new ParcelDescription();
                tempPar.Id = item.ID;
                try
                {
                    tempPar.SenderName = p.ClientById(item.SenderId).Name;
                    tempPar.TargetName = p.ClientById(item.TargetId).Name;
                }
                catch (IDAL.DO.ClientException ex)
                {
                    throw new IBL.BO.IDNotFound("not found", ex);
                }
                tempPar.weight = (WeightCategories)item.Weight;
                tempPar.priority = (Priorities)item.Priority;
                if (item.Requested == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.requested;
                }
                else if (item.Scheduled == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.scheduled;
                }
                else if (item.PickedUp == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.pickedup;
                }
                else if (item.Delivered == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.delivered;
                }
                parList.Add(tempPar);
            }
            return parList;
        }
        public void printParcelsNotAssigned() { }
        public void printFreeStations() 
        {
            List<StationDescription> statList = new List<StationDescription>();
            StationDescription tempStat = new StationDescription();
            foreach(var item in p.StationList())
            {
                if(item.ChargeSlots>0)
                {
                    tempStat.Id = item.ID;
                    tempStat.name = item.Name;
                    tempStat.freeChargeSlots = item.ChargeSlots;
                    foreach (var item2 in p.DroneChargeList())// full chargeSlots
                    {
                        if (item2.StationId == item.ID)
                            tempStat.fullChargeSlots++;
                    }
                }
                statList.Add(tempStat);
            }
        
        }
        public IEnumerable<ParcelDescription> displayParcelsNotAssigned()
        {
            List<ParcelDescription> PDList = new List<ParcelDescription>();
            foreach (var item in p.ParcelList())
            {
                if (item.Scheduled == DateTime.Now)
                {
                    ParcelDescription tempPD = new ParcelDescription();
                    tempPD.Id = item.ID;
                    tempPD.SenderName = Name(item.SenderId);
                    tempPD.TargetName= Name(item.TargetId);
                    tempPD.weight = (WeightCategories)item.Weight;
                    if (item.Requested == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.requested;
                    }
                    else if (item.Scheduled == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.scheduled;
                    }
                    else if (item.PickedUp == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.pickedup;
                    }
                    else if (item.Delivered == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.delivered;
                    }
                    PDList.Add(tempPD);
                }
            }
            return PDList;
        }
        #endregion
        #endregion



        //------------------------------------------HELP------------------------------------------
        //Distance
        #region helpFunctions
        public double distance(double lat1, double lon1, double lat2, double lon2)
        {
            var myPI = 0.017453292519943295;    // Math.PI / 180
            var a = 0.5 - Math.Cos((lat2 - lat1) * myPI) / 2 +
                    Math.Cos(lat1 * myPI) * Math.Cos(lat2 * myPI) *
                    (1 - Math.Cos((lon2 - lon1) * myPI)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
        public Station NearestStation(Localisation l,bool flag)
        {
            Station s = new Station();
            s.Loc = new Localisation();
            double lat1 = l.latitude;
            double long1 = l.longitude;
            double minDistance = 99999999;
            double tempDistance = 0;

            foreach(var item in p.StationList())
            {
                if (flag == true)
                {
                    if (item.ChargeSlots > 0)// if theres a slot available
                    { tempDistance = distance(lat1, long1, item.Latitude, item.Longitude); }
                    else
                        continue;
                }

                if (minDistance > tempDistance)
                {
                    minDistance = tempDistance;// keeps the closest one
                    s.ID = item.ID;
                    s.Name = item.Name;
                    s.Loc.longitude = item.Longitude;
                    s.Loc.latitude = item.Latitude;
                    s.ChargeSlots = item.ChargeSlots;
                }
            }
            return s;
        }
        public double DistanceAccToBattery(double battery)
        {
            //Le drone perd 1% en 7 min  et la vitesse du drone de 50 km/h
            // le drone gagne 1% en 7 min
           double timeInHours = 7 / 60;
           double speed = 50;  //50km/h
           double totalTime = timeInHours * battery;
            double distance = totalTime * speed;
            return distance;

        }
        public double BatteryAccToTime(double time)
        {
            double batt = time * 7;
            return batt;

        }
        public double BatteryAccToDistance(double distance)
        {
            double time = distance / 50;
            double batteryLost = time / (7 / 60) ;
            return batteryLost;
        }
        public string Name(int id)
        {
            try
            {
                IDAL.DO.Client c = p.ClientById(id);
                return c.Name;
            }
            catch (IDAL.DO.ClientException custEX)
            {
                throw new NotFound("Didn't find",custEX);
            
            }

                
        }
        public Localisation location(double lat1, double long1)
        {
            Localisation l = new Localisation();
            l.longitude = long1;
            l.latitude = lat1;
            return l;
        }
        IDAL.DO.Parcel ClosestParcel(List<IDAL.DO.Parcel> list, Localisation droneLoc)
        {
            double minDist = double.MaxValue;
            IDAL.DO.Parcel tempParcel = new IDAL.DO.Parcel();
            foreach(var item in list)
            {
                double dist = distance(p.ClientById(item.SenderId).Latitude, p.ClientById(item.SenderId).Longitude, droneLoc.latitude, droneLoc.longitude);
                if (dist < minDist)
                { 
                    minDist = dist;
                    tempParcel = item;
                }

            }
            return tempParcel;
        }
        IDAL.DO.Station ConvertStationToDal (IBL.BO.Station s)
        {
            IDAL.DO.Station stat = new IDAL.DO.Station();
            stat.ID = s.ID;
            stat.Name = s.Name;
            stat.Latitude = s.Loc.latitude;
            stat.Longitude = s.Loc.longitude;
            stat.ChargeSlots = s.ChargeSlots;
            return stat;

        }
        bool CheckId(int id)
        {
            int i = 0;
            while(id>0)
            {
                ++i;
                id /= 10;

            }
            if (i == 8)
                return true;
            return false;
        }
        
        #endregion


    }
}
