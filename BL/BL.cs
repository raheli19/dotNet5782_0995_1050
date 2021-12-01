
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
        static Random rand = new Random();

        private IDAL.IDal p;
        //Help h = new Help();

        List<DroneDescription> DroneList = new List<DroneDescription>();

        public BL()
        {
            //constructor of the BL file
            p = new DalObject.DalObject();
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


        //---------------------------------------ACTIONS------------------------------------------

       



        }
        #endregion

        #region Delivered
        public void delivered(int droneId)
        {
            Drone deliverDrone = new Drone();
            try
            {
                deliverDrone = GetDrone(droneId);  //finds the drone according to its ID
            }
            catch (IDAL.DO.DroneException ex)
            {
                throw new IDAL.DO.DroneException("" + ex);
            }
            Parcel parcel = new Parcel();  //create a new parcel

            if (deliverDrone.Status != DroneStatuses.shipping)
                throw new WrongDetailsUpdateException("Drone is not even scheduled");

            foreach (var item in displayParcelList())
            {
                parcel = GetParcel(item.Id);  //finds the parcel in the parcel list
                if (item.Status != ParcelStatus.pickedup && item.Id == deliverDrone.myParcel.ID)
                    throw new WrongDetailsUpdateException("drone didnt collect the parcel");

                if (item.Id == deliverDrone.myParcel.ID)  //finds the parcel which contains the id searched (of the parcel contained in the drone)

                {
                    double Distance = distance(deliverDrone.initialLoc.latitude, deliverDrone.initialLoc.longitude, GetClient(parcel.Target.ID).ClientLoc.latitude, GetClient(parcel.Target.ID).ClientLoc.longitude);//distance betwween the loc of the drone and the target

                    if (parcel.Weight == WeightCategories.heavy)
                        deliverDrone.Battery -=BatteryAccToDistance( Distance) ;
                    if (parcel.Weight == WeightCategories.low)
                        deliverDrone.Battery -= BatteryAccToDistance(Distance);
                    if (parcel.Weight == WeightCategories.middle)
                        deliverDrone.Battery -= BatteryAccToDistance(Distance);
                    deliverDrone.initialLoc = GetClient(parcel.Target.ID).ClientLoc;
                    deliverDrone.Status = DroneStatuses.free;
                    for (int i = 0; i < DroneList.Count(); i++)
                    {
                        if (DroneList[i].Id == deliverDrone.ID)
                        {
                            DroneDescription updateDrone = DroneList[i];
                            updateDrone.battery = deliverDrone.Battery;
                            updateDrone.loc = deliverDrone.initialLoc;
                            updateDrone.Status = deliverDrone.Status;
                            DroneList[i] = updateDrone;
                        }

                    }
                    parcel.Delivered = DateTime.Now;
                    IDAL.DO.Parcel updateParcel = new IDAL.DO.Parcel();
                    updateParcel.ID = parcel.ID;
                    updateParcel.SenderId = parcel.Sender.ID;
                    updateParcel.TargetId = parcel.Target.ID;
                    updateParcel.DroneId = parcel.Drone.ID;
                    updateParcel.Weight = Enum.Parse<IDAL.DO.WeightCategories>(parcel.Weight.ToString());
                    updateParcel.Priority = Enum.Parse<IDAL.DO.Priorities>(parcel.Priority.ToString());
                    updateParcel.Requested = parcel.Requested;
                    updateParcel.Scheduled = parcel.Scheduled;
                    updateParcel.PickedUp = parcel.PickedUp;
                    updateParcel.Delivered = parcel.Delivered;
                    p.UpdateParcelFromBL(updateParcel);
                }
                
            }
        }
        #endregion

        //-----------------------------------PRINT-FUNCTIONS--------------------------------------
        #region PRINTING

        #region displayStation

        /// <summary>
        /// This function receives a station ID and returns the details of this station 
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station displayStation(int stationId) 
        {
            Station stationBL = GetStation(stationId);  //recupere les donnees de la DAL
                                                //the only field missing is the list of drones
            List<DroneCharging> droneCharging = new List<DroneCharging>();
            List<int> DronesID = new List<int>();

            IEnumerable<IDAL.DO.DroneCharge> droneCharges = p.DroneChargeList();  //finds the list which contains the the drone charges from DAL
            foreach (var item in droneCharges)
            {
                if (item.StationId == stationBL.ID)  // finds the station with the ID received 
                {
                    Drone droneInStation = GetDrone(item.DroneId);   // finds the drones contained in this station
                    stationBL.DroneCharging.Add(new DroneCharging() { ID = item.DroneId, battery = droneInStation.Battery, });
                    
                }
            }
            return stationBL;
        }
        #endregion

        #region displayDrone

        /// <summary>
        /// This function receives a drone ID and returns the details of the drone searched
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone displayDrone(int droneId) 
        {
            Drone droneBL = GetDrone(droneId); //Copies the fields from DAL
            //the missing fields are:MaxWeight,Status,initialLoc,ParcelIndelivering
            DroneDescription tmp = DroneList.Find(x => x.Id == droneId);
            droneBL.initialLoc = new Localisation();
            droneBL.MaxWeight = tmp.weight;
            droneBL.Status = tmp.Status;
            droneBL.initialLoc = tmp.loc;
            ParcelInDelivering PID = new ParcelInDelivering();
            if (droneBL.Status == DroneStatuses.shipping)
            {
                foreach(var item in p.ParcelList()) //finds the details of the parcel
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
                            PID.distance = distance(PID.picking.latitude, PID.picking.longitude,PID.delivered.latitude, PID.delivered.longitude);
                        }
                    }
                    catch (Exception ex)                                                                                                                                                                                                     
                    {
                        throw new IDNotFound("The id was not found", ex);
                    }
                }
            }
            droneBL.myParcel = PID;
            return droneBL;
        }
        #endregion

        #region displayClient

        /// <summary>
        /// This function receives a client ID and returns the details of the client searched
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Client displayClient(int clientId) 
        {
            Client clientBL = GetClient(clientId);   //Copies the fields from DAL

            List<ParcelToClient> TempParcLstFromClient = new List<ParcelToClient>();

            foreach (var parcel in p.ParcelList())
            {
                if(parcel.SenderId==clientId)  //The parcel has been sent by this client
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
                    catch (IDAL.DO.ClientException ex)
                    {
                        throw new IBL.BO.IDNotFound("The client doesnt exist", ex);
                    }
                    PCT.client = myClient;
                    TempParcLstFromClient.Add(PCT);

                }
            }
            clientBL.ParcLstFromClient = TempParcLstFromClient;


            List<ParcelToClient> TempParcLstToClient = new List<ParcelToClient>();

            foreach (var parcel in p.ParcelList())
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
                    catch (IDAL.DO.ClientException ex)
                    {
                        throw new IDNotFound("The client doesnt exist", ex);
                    }
                    PCT.client = myClient;
                    TempParcLstToClient.Add(PCT);

                }
            }

            clientBL.ParcLstToClient = TempParcLstToClient;

            return clientBL;
        
        }
        #endregion

        #region displayParcel

        /// <summary>
        /// This function receives a parcel ID and receives the details of the parcel searched
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel displayParcel(int parcelId) 
        {
            Parcel parcelBL = GetParcel(parcelId);
            //The missing fields are Sender(ClientInParcel),Target(ClientInParcel) and Drone(droneWithParcel)

            ClientInParcel tempSender = new ClientInParcel();
            ClientInParcel tempTarget = new ClientInParcel();
            DroneWithParcel tempDrone = new DroneWithParcel();

            tempDrone.departureLoc = new Localisation();
            tempSender.ID = parcelBL.Sender.ID;
            try
            {
                tempSender.name = p.ClientById(tempSender.ID).Name;
                parcelBL.Sender = tempSender;

                tempTarget.ID = parcelBL.Target.ID;
                tempTarget.name = p.ClientById(tempTarget.ID).Name;
                parcelBL.Target = tempTarget;

            }
            catch (Exception ex)
            {
                throw new IBL.BO.IDNotFound("Not found", ex);
            }
            foreach (var droneItem in DroneList) // update the details of the drone that send the parcel.
            {
                if (droneItem.Id == parcelBL.Drone.ID)
                {
                    parcelBL.Drone = new DroneWithParcel() { ID = droneItem.Id, battery = droneItem.battery, departureLoc = droneItem.loc };
                }
            }
            parcelBL.Drone = tempDrone;
            return parcelBL;

        }
        #endregion

        #region IENUMERABLE 

        #region DisplayStationList
        /// <summary>
        /// This function gets all the stations with their details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationDescription> DisplayStationList() 
        {
            IEnumerable<IDAL.DO.Station> stationsFromDal = p.StationList();   //gets the station list from DAL
            List<StationDescription> statList = new List<StationDescription>();  //creates a new list of station description
            foreach(var item in p.StationList())
            {
                //StationDescription statD = new StationDescription();
                //statD.Id = item.ID;
                //statD.name = item.Name;
                //foreach( var item2 in p.DroneChargeList())// full chargeSlots
                //{
                //    if (item2.StationId == item.ID) // if the drone is in the station
                //    { 
                //        statD.fullChargeSlots++;

                //    }

                //}

                //statD.freeChargeSlots = item.ChargeSlots;// free ones
                //statList.Add(statD);// add it to the list

                Station station = GetStation(item.ID);      //copies the fields 
                statList.Add(new StationDescription()
                {
                    Id = item.ID,
                    name = item.Name,
                    freeChargeSlots = item.ChargeSlots,
                    fullChargeSlots = station.DroneCharging.Count()
                });

            }
            return statList;
        }

        #endregion

        #region displayDroneList

        /// <summary>
        /// This function returns the list of drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneDescription> displayDroneList() 
        {
            return DroneList;

        }

        #endregion

        #region displayClientList

        /// <summary>
        /// This function returns the list of clients with their details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientActions> displayClientList() 
        {
            IEnumerable<IDAL.DO.Client> customersFromDal = p.ClientList();
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
        #endregion

        #region displayParcelList

        /// <summary>
        /// This function returns the list of parcels with their details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelDescription> displayParcelList() 
        {
            IEnumerable<IDAL.DO.Parcel> parcelsFromDal = p.ParcelList();
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
                if (item.Requested != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.requested;
                }
                else if (item.Scheduled != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.scheduled;
                }
                else if (item.PickedUp != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.pickedup;
                }
                else if (item.Delivered != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.delivered;
                }
                parList.Add(tempPar);
            }
            return parList;
        }
        #endregion
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
        void updateBlDroneList(DroneDescription droneToUpdate)
        {

        }
        #endregion


    }
}