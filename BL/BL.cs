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
        help h = null;
       
        List<DroneDescription> DroneList = new List<DroneDescription>();

        public BL() 
        { 
            p= new DalObject.DalObject();
            //List <Drone> Dlist =new List<Drone> (p.DroneList().CopyPropertiesToNew(typeof(IBL.BO)));
            IBL.BO.DroneDescription Bldr=new IBL.BO.DroneDescription();  // drone from the bl
            IBL.BO.ParcelToClient ParcelInClient = new IBL.BO.ParcelToClient();
           

            foreach (var item in p.DroneList())
            {
                Bldr.Id = item.ID;
                Bldr.Model = item.Model;
                Bldr.battery = item.Battery;
                
                foreach (var item2 in p.ParcelList())
                {
                    if (item2.DroneId == item.ID) //gets all the parcel's info from the dal
                    {
                        ParcelInClient.ID = item2.ID;
                        ParcelInClient.weight = (WeightCategories)item2.Weight;
                        ParcelInClient.priority = (Priorities)item2.Priority;
                        if (item2.Requested != DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.requested;
                        if(item2.Scheduled!= DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.scheduled;
                        if (item2.PickedUp!= DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.pickedup;
                        if (item2.Delivered != DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.delivered;
                        
                    }
                }
                //ParcelInclient is the parcel associated to the drone
                //double parLongitude = p.ClientById(ParcelInClient.client.ID).Longitude; 
                //double parLatitude = p.ClientById(ParcelInClient.client.ID).Latitude; 
                double senderLat = default;
                double senderLong = default;
                double targetLat = default;
                double targetLong = default;
                try
                {
                    senderLat = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Latitude;
                     senderLong = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Longitude;
                     targetLat = p.ClientById((p.ParcelById(ParcelInClient.ID)).TargetId).Longitude;
                     targetLong = p.ClientById((p.ParcelById(ParcelInClient.ID)).SenderId).Latitude;

                }
                catch (IDAL.DO.ClientException ex)
                {
                    throw new IBL.BO.NotFound("Not found", ex);
                }

               
                // the drone is shipping
                if (ParcelInClient.Status != ParcelStatus.delivered)// the parcel has not been delivered yet.
                    {
                        Bldr.Status = DroneStatuses.shipping;
                        if (ParcelInClient.Status == ParcelStatus.scheduled && ParcelInClient.Status != ParcelStatus.pickedup)
                        {
                            Station s = NearestStation(Bldr.loc, true);
                            Bldr.loc = s.loc;
                        }
                        if (ParcelInClient.Status == ParcelStatus.pickedup && ParcelInClient.Status != ParcelStatus.delivered)// drone location = sender location
                        {

                            Bldr.loc.longitude = senderLong;
                            Bldr.loc.latitude = senderLat;
                        }

                        // calculates the distances for battery
                        double delDist = distance(senderLat, senderLong, targetLat, targetLong); // delivering distance
                        Localisation l = location(targetLat, targetLong);
                        Station chargeStat = NearestStation(l, false);
                        double cDist = distance(targetLat, targetLong, chargeStat.loc.latitude, chargeStat.loc.longitude);
                        double totalDistance = delDist + cDist;
                        double minBattery = BatteryAccToDistance(totalDistance);
                        Bldr.battery = h.getRandomNumber(minBattery, 100.0);
                    }
                
                // if drone isn't shipping
                else if (ParcelInClient.Status == ParcelStatus.delivered || ParcelInClient.Status == ParcelStatus.scheduled)// the drone isn't shipping
                {
                    int random = (rand.Next(0, 1))*2;
                    if (random == 0)
                    {
                        Bldr.Status = DroneStatuses.free;
                    }
                    if (random == 2)
                    {
                        Bldr.Status = DroneStatuses.shipping;
                    }
                }
                // if the drone is charging
                if (Bldr.Status==DroneStatuses.maintenance)
                {
                    // random localisation entre les differentes stations
                    List<int> helplist = p.IdStation();
                    int index = rand.Next(helplist.Count);
                    int s = helplist[index];
                    IDAL.DO.Station stat = p.StationById(s);
                    Bldr.loc = location(stat.Latitude, stat.Longitude);
                    Bldr.battery = h.getRandomNumber(0, 20);

                }
                
                //the drone is free
                else if (Bldr.Status==DroneStatuses.free)
                 {
                    
                    // location of one of the client that have received a parcel
                    List<int> helpList = p.clientReceivedParcel();
                    int index = rand.Next(helpList.Count);
                    int s = helpList[index];
                    try
                    {
                        IDAL.DO.Client c = p.ClientById(s);


                        Bldr.loc = location(c.Latitude, c.Longitude);
                    }
                    catch(IDAL.DO.ClientException ex)
                    {
                        throw new IBL.BO.NotFound("Didnt find",ex);
                    }

                    Station chargeStat = NearestStation(Bldr.loc, true);
                    double statDist = distance(chargeStat.loc.latitude, chargeStat.loc.longitude, Bldr.loc.latitude, Bldr.loc.longitude);
                    double minBattery = BatteryAccToDistance(statDist);
                    Bldr.battery = h.getRandomNumber(minBattery, 100.0);
                    
                }
                Bldr.weight = ParcelInClient.weight;
                Bldr.Status = (DroneStatuses)ParcelInClient.Status;
                

            }

        }
       
        #region GetClient
        public Client GetClient(int id)
        {
            Client c = default;
            try
            { 
                IDAL.DO.Client dalClient = p.ClientById(id);
                c.ID = dalClient.ID;
                c.Name = dalClient.Name;
                c.Phone = dalClient.Phone;
                c.ClientLoc.longitude = dalClient.Longitude;
                c.ClientLoc.latitude = dalClient.Latitude;
            }
            catch(IDAL.DO.ClientException custEX)
            {
                throw new IBL.BO.IDNotFound($"Client ID {id} was not found", custEX);
            }
            return c;
        }
        #endregion
        #region GetStation
        public Station GetStation(int id)
        {
            Station s = default;
            try
            {
                IDAL.DO.Station dalStat = p.StationById(id);
                s.ID = dalStat.ID;
                s.Name = dalStat.Name;
                s.loc.longitude = dalStat.Longitude;
                s.loc.latitude = dalStat.Latitude;
                s.ChargeSlots = dalStat.ChargeSlots;
            }
            catch (IDAL.DO.StationException statEX)
            {
                throw new IBL.BO.IDNotFound($"Station ID {id} was not found", statEX);
            }
            return s;
        }
        #endregion
        #region GetDrone
        public Drone GetDrone(int id)
        {
            Drone d = default;
            try
            {
                IDAL.DO.Drone dalDrone = p.DroneById(id);
                d.ID = dalDrone.ID;
                d.Model = dalDrone.Model;
                d.Battery = dalDrone.Battery;
               
            }
            catch (IDAL.DO.DroneException drEX)
            {
                throw new IBL.BO.IDNotFound($"Drone ID {id} was not found", drEX);
            }
            return d;

        }
        #endregion
        #region GetParcel

        public Parcel GetParcel(int id)
        {
            Parcel myParcel = default;
            try
            {
                IDAL.DO.Parcel dalParcel = p.ParcelById(id);
                myParcel.ID = dalParcel.ID;
                myParcel.Sender.ID = dalParcel.SenderId;
                myParcel.Target.ID = dalParcel.TargetId;
                myParcel.Weight = (WeightCategories)dalParcel.Weight;
                myParcel.Priority = (Priorities)dalParcel.Priority;
                myParcel.Requested = dalParcel.Requested;
                myParcel.Scheduled = dalParcel.Scheduled;
                myParcel.PickedUp = dalParcel.PickedUp;
                myParcel.Delivered = dalParcel.Delivered;
                myParcel.Drone.ID = dalParcel.DroneId;
            }
            catch (IDAL.DO.ParcelException prEX)
            {
                throw new IBL.BO.IDNotFound($"Parcel ID {id} was not found", prEX);
            }
            return myParcel;
        }


        #endregion

        //-----------------------------------ADD-FUNCTIONS----------------------------------------

        #region ADDFunctions
        public void addStation(IBL.BO.Station s)   
        {
            IDAL.DO.Station stat = new IDAL.DO.Station();
            stat.Name = s.Name;
            if (!(s.ID <= 99999999 && s.ID > 9999999))
                throw new InputNotValid("ID not valid");
            stat.ID = s.ID;
            if (s.loc.latitude< 31||s.loc.latitude>33.3)/////////////// a verifier
                throw new InputNotValid("latitude is not valid");
            stat.Latitude = s.loc.latitude;
            if (s.loc.longitude<34.3||s.loc.longitude>35.5)
                throw new InputNotValid("longitude is not valid");
            stat.Longitude = s.loc.longitude;
            stat.ChargeSlots = s.ChargeSlots;
            try
            {
                p.addStation(stat);
                //p.addStation((IDAL.DO.Station)s.CopyPropertiesToNew(typeof(IDAL.DO.Station)));
            }
            catch(IDAL.DO.StationException ex)
            {
                throw new AlreadyExist("Station already exists ", ex);
            }
            
        }

        public  void addDrone(Drone d, int StationID) 
        {
            //Add the drone To Add and check if the inputs are correct
            IDAL.DO.Drone DALdr = new IDAL.DO.Drone();
            d.Battery = rand.Next(20, 40);// initialize the battery
            Station s = GetStation(StationID);
            if (!(d.ID <= 99999999 && d.ID > 9999999)) //ID not Valid
                throw new IBL.BO.InputNotValid("ID is not valid");
            DALdr.ID = d.ID;
            if (d.MaxWeight != WeightCategories.low && d.MaxWeight != WeightCategories.middle && d.MaxWeight != WeightCategories.heavy)
               throw new IBL.BO.InputNotValid("The category of your weight is not valid");
            if (d.Status != DroneStatuses.free && d.Status != DroneStatuses.maintenance && d.Status != DroneStatuses.shipping)
                throw new IBL.BO.InputNotValid("The status of your drone is not valid");
            DALdr.Model = d.Model;
            if (d.Battery < 0 || d.Battery > 100)
                throw new IBL.BO.InputNotValid("Battery is not valid");
            DALdr.Battery = d.Battery;
            
            d.Status = DroneStatuses.maintenance;
            d.initialLoc = s.loc;// his location is the same than the station
          
            //Create a new DroneDescription and add it TO BL
            DroneDescription DP = new DroneDescription();
            DP.Id = d.ID;
            DP.Model = d.Model;
            DP.weight = d.MaxWeight;
            DP.battery = d.Battery;
            DP.Status = DroneStatuses.maintenance;
            DP.loc = d.initialLoc;
            DroneList.Add(DP);
            s.ChargeSlots--;// one more is full
            try
            {
                p.AddDrone(DALdr);//ADD TO DAL
                //p.AddDrone((IDAL.DO.Drone)d.CopyPropertiesToNew(typeof(IDAL.DO.Drone)));
            } 
            catch(IDAL.DO.DroneException ex)
            {
                throw new AlreadyExist("Can't add this drone", ex);
            }
        }

        public void addClient(Client c) 
        {
            IDAL.DO.Client cl= new IDAL.DO.Client();
            if (!(c.ID <= 99999999 && c.ID > 9999999))
                throw new IBL.BO.InputNotValid("ID not valid");
            cl.ID = c.ID;
            cl.Name = c.Name;
            if (c.Phone.Length!=11)
                throw new IBL.BO.InputNotValid("Phone not valid");
            cl.Phone = c.Phone;
            if (c.ClientLoc.latitude < 31 || c.ClientLoc.latitude > 33.3)/////////////// a verifier
                throw new IBL.BO.InputNotValid("latitude is not valid");
            cl.Latitude = c.ClientLoc.latitude;
            if (c.ClientLoc.longitude < 34.3 || c.ClientLoc.longitude > 35.5)
                throw new IBL.BO.InputNotValid("longitude is not valid");
            cl.Longitude = c.ClientLoc.longitude;
            try
            {
                p.addClient(cl);
                //p.addClient((IDAL.DO.Client)c.CopyPropertiesToNew(typeof(IDAL.DO.Client)));
            }
            catch (IDAL.DO.ClientException ex)
            {
                throw new AlreadyExist("This Client already exists", ex);
            }
            
           
        }

        public void addParcel(Parcel pack) 
        {
            IDAL.DO.Parcel DALParcel = new IDAL.DO.Parcel();
            if (!(pack.ID <= 99999999 && pack.ID > 9999999))
                throw new IBL.BO.InputNotValid("ID not valid");
            DALParcel.ID = pack.ID;
            if (!(pack.Sender.ID <= 99999999 && pack.Sender.ID > 9999999))
                throw new IBL.BO.InputNotValid("SenderID not valid");
            DALParcel.SenderId = pack.Sender.ID;
            if (!(pack.Target.ID <= 99999999 && pack.Target.ID > 9999999))
                throw new IBL.BO.InputNotValid("TargetID not valid");
            DALParcel.TargetId = pack.Target.ID;
            pack.Scheduled = DateTime.MinValue;
            pack.PickedUp = DateTime.MinValue;
            pack.Delivered = DateTime.MinValue;
            pack.Requested = DateTime.Now;
            pack.Drone = null;
            if (pack.Weight != WeightCategories.low && pack.Weight != WeightCategories.middle && pack.Weight != WeightCategories.heavy)
                throw new IBL.BO.InputNotValid("The category of your weight is not valid");
            DALParcel.Weight = (IDAL.DO.WeightCategories)pack.Weight;
            if (pack.Priority != Priorities.regular && pack.Priority != Priorities.fast && pack.Priority != Priorities.emergency)
                throw new IBL.BO.InputNotValid("The time delivery is not valid");
            DALParcel.Priority = (IDAL.DO.Priorities)pack.Priority;
            DALParcel.Scheduled = pack.Scheduled;
            DALParcel.PickedUp = pack.PickedUp;
            DALParcel.Delivered = pack.Delivered;
            DALParcel.Requested = pack.Requested;
            DALParcel.DroneId = pack.Drone.ID;
            try
            {
                p.addParcel(DALParcel);
                //p.addParcel((IDAL.DO.Parcel)pack.CopyPropertiesToNew(typeof(IDAL.DO.Parcel)));
            }
            catch(IDAL.DO.ParcelException ex)
            {
                throw new AlreadyExist("This package already exists", ex);
            }
        
        }
        #endregion

        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------
        #region UPDATING

        #region Update DroneName
        public void updateDroneName(int Id, string newModel) 
        {
            try
            {
                IDAL.DO.Drone d = p.DroneById(Id); // DALdrone
                d.Model = newModel;
                p.UpdateDrone(d); // this function update the DALlist

            }
            catch(IDAL.DO.DroneException ex)
            {
                throw new IBL.BO.IDNotFound("Can't update the name", ex);
            }
        
        }
        #endregion

        #region Update StationName
        public void updateStationName_CS(int Id, int newName = -1, int newCS = -1)
        {
            try
            {
                IDAL.DO.Station s = p.StationById(Id);
                if (newName != -1)
                    s.Name = newName;
                if (newCS != -1)
                    s.ChargeSlots = newCS;
                p.UpdateStation(s);
            }
            catch (IDAL.DO.StationException ex)
            {
                throw new IBL.BO.IDNotFound ("Can't update the station name and/or the number of chargeSlots", ex);
            }
            
            //Enlever de la list puis le remettre

        }
        #endregion

        #region Update ClientName_Phone
        public void updateClientName_Phone(int Id, string newName = " ", string newTel = " ") 
        {
            try
            {
                IDAL.DO.Client c = p.ClientById(Id);
                if (newName != " ")
                {
                    c.Name = newName;
                }
                p.UpdateClient(c);
            }
            catch(IDAL.DO.ClientException ex)
            {
                throw new IBL.BO.IDNotFound("Can't update the client name",ex);
            }
        
        }
        #endregion

        #region DroneToCharge
        public void DroneToCharge(int DroneId)
        {

            DroneDescription blDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (blDrone == null)
            {
                throw new IBL.BO.NotFound("Drone not found");
            }
            if (blDrone.Status == DroneStatuses.free)
            {
                Station nearestStation = NearestStation(blDrone.loc, true);
                double d = distance(blDrone.loc.latitude, blDrone.loc.longitude, nearestStation.loc.latitude, nearestStation.loc.longitude);
                bool canGoToCharge = false;
                if (DistanceAccToBattery(blDrone.battery) >= d)
                    canGoToCharge = true;
                else
                    throw new NotEoughBattery("Can not send the drone to the station, it doesn't have enough battery!");
                if (canGoToCharge == true) 
                {
                    blDrone.battery -= BatteryAccToDistance(DistanceAccToBattery(blDrone.battery));// substract the account of percetn from the battery to go to the nearest station
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD = blDrone;
                    tempDD.loc = nearestStation.loc;
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
                    IDAL.DO.Station s = new IDAL.DO.Station();
                    s.ID = nearestStation.ID;
                    s.Name = nearestStation.Name;
                    s.Latitude = nearestStation.loc.latitude;
                    s.Longitude = nearestStation.loc.longitude;
                    s.ChargeSlots = nearestStation.ChargeSlots -1;
                    try { 
                    p.UpdateStation(s);// update the station in the dal list
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
            DroneDescription myDr = new DroneDescription();
            myDr.Id = ID;
            myDr.Status = DroneStatuses.free; // signs if we don't find it
            foreach(var item in DroneList)// get all the information about this drone from the dronelist
            {
                if(item.Id==ID)
                {
                    myDr.Status = item.Status;
                    myDr.Model = item.Model;
                    myDr.weight = item.weight;
                    myDr.loc = item.loc;
                    myDr.DeliveredParcels = item.DeliveredParcels;
                    // don't copy the battery
                    DroneList.Remove(item);// delete the drone from the list
                }
            }
            if (myDr.Status != DroneStatuses.maintenance)// if the drone is not charging
                throw new IBL.BO.InputNotValid("This drone isn't charging");

            // upadte the drone in the bl DroneList
            myDr.Status = DroneStatuses.free;
            myDr.battery = BatteryAccToDistance(time);
            DroneList.Add(myDr);

            //update the drone in the droneList from the DAL
            try
            {
                IDAL.DO.Drone drone = p.DroneById(ID);
                drone.Battery = BatteryAccToTime(time);
                p.UpdateDrone(drone);
            }
            catch(IDAL.DO.DroneException)
            {
                throw new IBL.BO.CannotUpdate("Can't update the drone");
            }

            //update station
            IDAL.DO.Station stat = default;
            try
            {
               stat = p.StationById(ID);
            }
            catch (IDAL.DO.StationException ex)
            {
                throw new IBL.BO.IDNotFound("ID Not found", ex);
            }
            foreach (var item in p.StationList())
            {
                if (item.Longitude == myDr.loc.longitude && item.Latitude == myDr.loc.latitude)
                {
                    stat.ID = item.ID;
                    stat.Name = item.Name;
                    stat.Latitude = item.Latitude;
                    stat.Longitude = item.Longitude;
                    stat.ChargeSlots = item.ChargeSlots;
                }                   
            }
            stat.ChargeSlots++;
            try
            {
                p.UpdateStation(stat); // puts back the station with one more chargeSlot free
            }
            catch (IDAL.DO.StationException ex)
            {
                throw new IBL.BO.CannotUpdate("The station cannot be updated", ex);
            }
            foreach (var item in p.DroneChargeList())
            {
                if (item.DroneId == myDr.Id && item.StationId == stat.ID)
                    p.updateDroneChargeList(myDr.Id, stat.ID);

            }



        }
        #endregion

        #region Assignement
        public void Assignement(int ID, int i=2)
        {
            IDAL.DO.Drone Daldrone = p.DroneById(ID); //from the dal
            DroneDescription BLd = DroneList.First(x => x.Id == ID);// finds the drone
            
            if (BLd.Status != DroneStatuses.free)
                throw new NotAvailable("The drone is not free!");
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel(); // dalparcel
            bool flag = false;

            foreach (var item in p.ParcelList())
            {
                if (item.Priority != IDAL.DO.Priorities.emergency - i)
                    continue;
                if (item.Weight == (IDAL.DO.WeightCategories)BLd.weight)///////////////
                    continue;
                parcel.DroneId = BLd.Id;
                parcel.ID = item.ID;
                parcel.Priority = item.Priority;
                parcel.SenderId = item.SenderId;
                parcel.TargetId = item.TargetId;
                parcel.Weight = item.Weight;
                parcel.Requested = DateTime.Now;

                List<IDAL.DO.Parcel> parcelLst = p.ParcelList().ToList().FindAll(x => (int)x.Priority == i && x.Scheduled == DateTime.MinValue);// list with all the emergency ones
                parcelLst = parcelLst.FindAll(x => (int)x.Weight == (int)BLd.weight);  //list with all the same weights

                IDAL.DO.Parcel closestParcel = ClosestParcel(parcelLst, BLd.loc);


                //trouver la parcel la plus proche verifier toutes les conditions
                // si fais pas / rappelle pour chercher avec mishkal different
                // si trouve pas / moins urgente avec mishkal le mieux adapte
                //si trouve pas / mishkal 

                double senderLat = p.FindLat(closestParcel.SenderId);
                double senderLong = p.FindLong(closestParcel.SenderId);
                double targetLat = p.FindLat(closestParcel.TargetId);
                double targetLong = p.FindLong(closestParcel.TargetId);
                //trouver la plus proche
                // has to be able to make it
                double distToSender = distance(BLd.loc.latitude, BLd.loc.longitude, senderLat, senderLong);
                BLd.battery -= BatteryAccToDistance(distToSender);
                if (distToSender >= DistanceAccToBattery(BLd.battery))
                    continue;
                //doit repartir

                double distToTarget = distance(BLd.loc.latitude, BLd.loc.longitude, targetLat, targetLong);
                BLd.battery -= BatteryAccToDistance(distToTarget);
                if (distToTarget >= DistanceAccToBattery(BLd.battery))
                    continue;
                BLd.battery -= BatteryAccToDistance(distToTarget);
                // if drone's battery less than 25 has to go to charge
                if (BLd.battery <= 25)
                {
                    Localisation targLoc = location(targetLat, targetLong);
                    Station nearStat = NearestStation(targLoc, false); //finds the nearest station if need to charge
                    double distToStat = distance(BLd.loc.latitude, BLd.loc.longitude, nearStat.loc.latitude, nearStat.loc.longitude);
                    if (distToStat > BatteryAccToDistance(distToStat))
                        continue;
                }
                flag = true;

                if (flag == false && i == 3) // we didn't find one
                    throw new ParcelException("We didn't find a parcel");
                else if (flag == false)
                    Assignement(ID, i - 1); // calls back the function to check with an other status
                BLd.Status = DroneStatuses.shipping;
                DroneList.Add(BLd);
                p.Assignement(closestParcel.ID, BLd.Id); //update the dronelist in the dal
            }
        }
        #endregion
        #endregion
        //---------------------------------------ACTIONS------------------------------------------

        #region PickedUp
        public void PickedUp(int DroneId)
        {
            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new IBL.BO.IDNotFound("Drone not found");
            }
            try
            {
                IDAL.DO.Parcel prcel = p.FindParcelAssociatedWithDrone(DroneId);
                if ((prcel.Requested == DateTime.Now || prcel.Scheduled == DateTime.Now) && (prcel.PickedUp == DateTime.MinValue))
                {
                    int senderId = prcel.SenderId;
                    Localisation senderLoc = new Localisation();
                    senderLoc.latitude = p.FindLat(senderId);
                    senderLoc.longitude = p.FindLong(senderId);
                    double myDistance = distance(myDrone.loc.latitude, myDrone.loc.longitude, senderLoc.latitude, senderLoc.longitude);
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD = myDrone;
                    tempDD.battery -= BatteryAccToDistance(myDistance);
                    tempDD.loc = senderLoc;
                    DroneList.Remove(myDrone);
                    DroneList.Add(tempDD);
                    IDAL.DO.Parcel tempParcel = new IDAL.DO.Parcel();
                    tempParcel = prcel;
                    tempParcel.PickedUp = DateTime.Now;
                    p.AddParcelFromBL(tempParcel);
                }
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
            foreach(var item in p.DroneChargeList())
            {
                if (item.StationId == stationId)
                {
                    DronesID.Add(item.DroneId);
                }
            }
            int i = 0,j=0;
            foreach(var item in DroneList)
            {
                if (item.Id == DronesID[i])
                {
                    i++;
                    //s.DroneCharging[j].ID = item.Id;
                    //s.DroneCharging[j].battery = item.battery;
                    droneCharging[j].ID= item.Id;
                    droneCharging[j].battery = item.battery;
                    j++;
                }
             
            }
            s.DroneCharging = droneCharging;
            return s;
        }
        #endregion

        #region displayDrone
        public Drone displayDrone(int droneId) 
        {
            Drone d = GetDrone(droneId); //Copies the fields from DAL
            //the missing fields are:MaxWeight,Status,initialLoc,ParcelIndelivering
            DroneDescription tmp = DroneList.Find(x => x.Id == droneId);
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
                    if (item2.StationId == item.ID)
                        statD.fullChargeSlots++;
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
            ParcelDescription tempPar = new ParcelDescription();
            foreach(var item in p.ParcelList())
            {
                tempPar.Id = item.ID;
                tempPar.SenderName = Name(item.SenderId);
                tempPar.TargetName = Name(item.TargetId);
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
                    s.Name = item.ID;
                    s.loc.longitude = item.Longitude;
                    s.loc.latitude = item.Latitude;
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
        #endregion


    }
}
