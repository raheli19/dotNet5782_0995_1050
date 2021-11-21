﻿using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DAL;
using DalObject;




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
            IBL.BO.DroneDescription dr=new IBL.BO.DroneDescription();
            IBL.BO.ParcelToClient ParcelInClient = new IBL.BO.ParcelToClient();
           

            foreach (var item in p.DroneList())
            {
                dr.Id = item.ID;
                dr.Model = item.Model;
                dr.battery = item.Battery;
                
                foreach (var elmtParcel in p.ParcelList())
                {
                    if (elmtParcel.DroneId == item.ID)
                    {
                        ParcelInClient.ID = elmtParcel.ID;
                        ParcelInClient.weight = (WeightCategories)elmtParcel.Weight;
                        ParcelInClient.priority = (Priorities)elmtParcel.Priority;
                        if (elmtParcel.Requested != DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.requested;
                        if(elmtParcel.Scheduled!= DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.scheduled;
                        if (elmtParcel.PickedUp!= DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.pickedup;
                        if (elmtParcel.Delivered != DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.delivered;
                        
                    }
                }


                if (ParcelInClient.Status != ParcelStatus.delivered)// the parcel has not been delivered yet.
                {
                    dr.Status = DroneStatuses.shipping;
                    if (ParcelInClient.Status == ParcelStatus.scheduled && ParcelInClient.Status != ParcelStatus.pickedup)
                    {// la station la plus proche

                    }
                }
                else if (ParcelInClient.Status == ParcelStatus.delivered || ParcelInClient.Status == ParcelStatus.scheduled)// the drone isn't shipping
                {
                    int random = (rand.Next(0, 1))*2;
                    if (random == 0)
                    {
                        dr.Status = DroneStatuses.free;
                    }
                    if (random == 2)
                    {
                        dr.Status = DroneStatuses.shipping;
                    }
                }
                if(dr.Status==DroneStatuses.maintenance)// if the drone is charging
                {
                    // random localisation entre les differentes stations
                    List<int> helplist = p.IdStation();
                    int index = rand.Next(helplist.Count);
                    Station s = p.StationList()[index];
                    // recuperer la station selon son makom
                    // prendre sa loc et la mettre dans le drone

                    dr.battery = h.getRandomNumber(0, 20);

                }
                else if (dr.Status==DroneStatuses.free)
                 {
                        // localisation entre des clients qui ont deja recu des colis
                        // batterie min pour faire une livraisonx
                 }
                dr.weight = ParcelInClient.weight;
                dr.Status = (DroneStatuses)ParcelInClient.Status;
                
                //dr.loc.latitude
                //dr.loc.longitude
                
                

                //DroneList.Add(dr);
                
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
                throw new ClientException($"Client ID {id} was not found", custEX);
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
                throw new StationException($"Station ID {id} was not found", statEX);
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
                throw new DroneException($"Drone ID {id} was not found", drEX);
            }
            return d;

        }
        #endregion

        //-----------------------------------ADD-FUNCTIONS----------------------------------------

        public void addStation(IBL.BO.Station s)   
        {
            //IDAL.DO.Station stat = new IDAL.DO.Station();
            //stat.Name = s.Name;
            if (!(s.ID <= 99999999 && s.ID > 9999999))
                throw new IDException("ID not valid");
            //stat.ID = s.ID;
            if (s.loc.latitude< 31||s.loc.latitude>33.3)/////////////// a verifier
                throw new LatException("latitude is not valid");
            //stat.Latitude = s.loc.latitude;
            if (s.loc.longitude<34.3||s.loc.longitude>35.5)
                throw new LongException("longitude is not valid");
            //stat.Longitude = s.loc.longitude;
            try
            {
                p.addStation((IDAL.DO.Station)s.CopyPropertiesToNew(typeof(IDAL.DO.Station)));
            }
            catch(StationException ex)
            {
                throw new StationException("Station already exists ", ex);
            }
            
        }

        public  void addDrone(Drone d, int StationID) 
        {
            d.Battery = rand.Next(20, 40);// initialize the battery
            //Station s = StationById(StationID);// find the station
                
            Station s = GetStation(StationID);
            //IDAL.DO.Drone dr = new IDAL.DO.Drone();
            if (!(d.ID <= 99999999 && d.ID > 9999999)) //ID not Valid
                throw new IDException("ID is not valid");
            //dr.ID = d.ID;
            if (d.MaxWeight != WeightCategories.low && d.MaxWeight != WeightCategories.middle && d.MaxWeight != WeightCategories.heavy)
                throw new WeightException("The category of your weight is not valid");
            if (d.Status != DroneStatuses.free && d.Status != DroneStatuses.maintenance && d.Status != DroneStatuses.shipping)
                throw new StatusException("The status of your drone is not valid");
                //dr.Model = d.Model;
            if (d.Battery < 0 || d.Battery > 100)
                throw new BatException("Battery is not valid");
            //dr.Battery = d.Battery;
            
            d.initialLoc = s.loc;// his location is the same than the station
            s.DroneCharging.Add(d);// add the drone to the station's list (BL)
            s.ChargeSlots--;// one more is full
            try
            {
                p.AddDrone((IDAL.DO.Drone)d.CopyPropertiesToNew(typeof(IDAL.DO.Drone)));
            } 
            catch(Exception ex)
            {
                throw new DroneException("Can't add this drone", ex);
            }
        }

        public void addClient(Client c) 
        {
            //IDAL.DO.Client cl= new IDAL.DO.Client();
            if (!(c.ID <= 99999999 && c.ID > 9999999))
                throw new IDException("ID not valid");
            //cl.ID = c.ID;
            //cl.Name = c.Name;
            if (c.Phone.Length!=11)
                throw new PhoneException("Phone not valid");
            //cl.Phone = c.Phone;
            if (c.ClientLoc.latitude < 31 || c.ClientLoc.latitude > 33.3)/////////////// a verifier
                throw new LatException("latitude is not valid");
            //cl.Latitude = c.ClientLoc.latitude;
            if (c.ClientLoc.longitude < 34.3 || c.ClientLoc.longitude > 35.5)
                throw new LatException("longitude is not valid");

            try
            {
                p.addClient((IDAL.DO.Client)c.CopyPropertiesToNew(typeof(IDAL.DO.Client)));
            }
            catch (ClientException ex)
            {
                throw new ClientException("This Client already exists", ex);
            }
            
           
        }

        public void addParcel(Parcel pack) 
        {

            if (!(pack.ID <= 99999999 && pack.ID > 9999999))
                throw new IDException("ID not valid");
           
            if (!(pack.Sender.ID <= 99999999 && pack.Sender.ID > 9999999))
                throw new IDException("SenderID not valid");
    
            if (!(pack.Target.ID <= 99999999 && pack.Target.ID > 9999999))
                throw new IDException("TargetID not valid");
            
            pack.Scheduled = DateTime.MinValue;
            pack.PickedUp = DateTime.MinValue;
            pack.Delivered = DateTime.MinValue;
            pack.Requested = DateTime.Now;
            pack.Drone = null;
            if (pack.Weight != WeightCategories.low && pack.Weight != WeightCategories.middle && pack.Weight != WeightCategories.heavy)
                throw new WeightException("The category of your weight is not valid");
            if (pack.Priority != Priorities.regular && pack.Priority != Priorities.fast && pack.Priority != Priorities.emergency)
                throw new PriorityException("The time delivery is not valid");
            try
            {
                p.addParcel((IDAL.DO.Parcel)pack.CopyPropertiesToNew(typeof(IDAL.DO.Parcel)));
            }
            catch(Exception ex)
            {
                throw new ParcelException("This package already exists", ex);
            }
        
        }


        //-----------------------------------UPDATE-FUNCTIONS----------------------------------------

        public void updateDroneName(int Id, string newModel) 
        {
            try
            {
                IDAL.DO.Drone d = p.DroneById(Id);
                d.Model = newModel;
                p.UpdateDrone(d);

            }
            catch(DroneException ex)
            {
                throw new DroneException("Can't update the name", ex);
            }
        
        }
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
            catch (StationException ex)
            {
                throw new StationException("Can't update the station name and/or the number of chargeSlots", ex);
            }
            
            //Enlever de la list puis le remettre

        }
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
            catch(ClientException ex)
            {
                throw new ClientException("Can't update the client name",ex);
            }
        
        }
        public void DroneToCharge(int DroneId)
        {

            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new DroneException("Drone not found");
            }
            if (myDrone.Status == DroneStatuses.free)
            {
                Station nearestStation = NearestStation(myDrone.loc, true);
                double d = distance(myDrone.loc.latitude, myDrone.loc.longitude, nearestStation.loc.latitude, nearestStation.loc.longitude);
                bool canGoToCharge = false;
                if (DistanceAccToBattery(myDrone.battery) >= d)
                    canGoToCharge = true;
                if(canGoToCharge==true)
                {
                    myDrone.battery -= BatteryAccToDistance(DistanceAccToBattery(myDrone.battery));// substract the account of percetn from the battery to go to the nearest station
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD = myDrone;
                    tempDD.loc = nearestStation.loc;
                    tempDD.Status = DroneStatuses.maintenance;
                    DroneList.Remove(myDrone);
                    DroneList.Add(tempDD);

                    p.AddFromBLDroneCharging(myDrone.Id, nearestStation.ID);//ADD The DroneCharge(drone+station) to DAL 
                    DroneCharge CD = new DroneCharge();
                    CD.DroneId = DroneId;
                    CD.StationId = nearestStation.ID;
                    nearestStation.DroneCharging.Add(CD);
                }
            }
        }


        public void DroneCharged(int ID, double time)
        {
            DroneDescription myDr = new DroneDescription();
            myDr.Id = ID;
            myDr.Status = DroneStatuses.free; // signs if we don't find it
            foreach(var item in DroneList)// get al the information about this drone from the dronelist
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
                throw new DroneChargedException("This drone isn't charging");

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
            catch
            {
                throw new DroneChargedException("Can't update the drone");
            }

            //update station
            IDAL.DO.Station stat = p.StationById(ID);
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
            p.UpdateStation(stat); // puts back the station with one more chargeSlot free
            foreach (var item in stat.)



        }

        public void Assignement(int ID)
        {
            IDAL.DO.Drone Daldrone = p.DroneById(ID); //from the dal
            DroneDescription BLd = new DroneDescription(); //from the bl
            foreach (var item in DroneList)
            {
                if (item.Id == ID)
                {

                    BLd.Id = item.Id;
                    BLd.Model = item.Model;
                    BLd.weight = item.weight;
                    BLd.battery = item.battery;
                    BLd.Status = item.Status;
                    BLd.loc = item.loc;
                    BLd.DeliveredParcels = item.DeliveredParcels;
                    DroneList.Remove(item);
                }
            }
            if (BLd.Status != DroneStatuses.free)
                throw new DroneException("The drone is not free!");
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            bool flag = false;
            foreach (var item in p.ParcelList())
            {
                double distToSender = distance(BLd.loc.latitude, BLd.loc.longitude, loc lat, lock long) ;
                if (distToSender > DistanceAccToBattery(BLd.battery))
                    continue;
                BLd.battery -= BatteryAccToDistance(distToSender);
                double distToTarget = distance(BLd.loc.latitude, BLd.loc.longitude, loc lat, lock long)
                if (distToTarget > DistanceAccToBattery(BLd.battery))
                    continue;
                BLd.battery -= BatteryAccToDistance(distToTarget);
                // besoin de recharger a partir de combien????
                // recup loc de la parcel
                // trouve la station la plus proche

                if (item.Priority != IDAL.DO.Priorities.emergency)
                    continue;
                if (item.Weight != (IDAL.DO.WeightCategories)BLd.weight)
                    continue;
                parcel.DroneId = BLd.Id;
                parcel.ID = item.ID;
                parcel.Priority = item.Priority;
                parcel.SenderId = item.SenderId;
                parcel.TargetId = item.TargetId;
                parcel.Weight = item.Weight;
                parcel.Requested = DateTime.Now;
                flag = true;
            }
            if (flag == false) // we didn't find one
                throw new ParcelException("We didn't find a parcel");
            BLd.Status = DroneStatuses.free;
            DroneList.Add(BLd);
            p.Assignement(parcel.ID, BLd.Id); //
            

            // attiver chez le sender, arriver jusqu(au target, voir si il a besoin de recharger arriver a une station la plus proche;
            
        }
        //---------------------------------------ACTIONS------------------------------------------------
        #region Dronecharged
        public void DroneCharged(int DroneId, double timeInCharge) { }
        #endregion

        #region Assignement
        public void Assignement(int DroneId) { }
        #endregion

        #region PickedUp
        public void PickedUp(int DroneId)
        {
            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new DroneException("Drone not found");
            }
            IDAL.DO.Parcel prcel = p.FindParcelAssociatedWithDrone(DroneId);
            if ((prcel.Requested == DateTime.Now || prcel.Scheduled == DateTime.Now) && (prcel.PickedUp == DateTime.MinValue)) 
            {
                int senderId = prcel.SenderId;
                Localisation senderLoc=new Localisation();
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
        #endregion

        #region Delivered
        public void delivered(int DroneId)
        {
            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new DroneException("Drone not found");
            }
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
        #endregion

        //-----------------------------------PRINT-FUNCTIONS----------------------------------------
        #region PRINTING
        public Station displayStation(int stationId) 
        {
            Station s = GetStation(stationId);  //recupere les donnees de la DAL
             //the only missing is the list of drones
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
                    s.DroneCharging[j].ID = item.Id;
                    s.DroneCharging[j].battery = item.battery;
                }
            }
            
            return s;
        }
        public void displayDrone() { }
        public void displayClient() { }
        public void displayParcel() { }
        public void printStationList() { }
        public void printDroneList() { }
        public void printClientList() { }
        public void printParcelList() { }
        public void printParcelsNotAssigned() { }
        public void printFreeStations() { }
        #endregion

        //Distance
        static double distance(double lat1, double lon1, double lat2, double lon2)
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
                if(flag==true)
                   // verifier les chargeslots 
                tempDistance = distance(lat1, long1, item.Latitude, item.Longitude);


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
            //Le drone perd 1% en 7 min  et la vitesse du drone  de 50 km/h
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
            double batteryLost = time / (7 / 60);
            return batteryLost;
        }
    }
}
