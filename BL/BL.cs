
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

        readonly IDAL.IDal p;
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
            int index = DroneList.FindIndex(Drone => Drone.Id == droneId);
            myDrone.loc = new Localisation();
            collectDrone.initialLoc = new Localisation();
            if (myDrone == null)  //the drone is not among the drone List
            {
                throw new IDNotFound("Drone not found");  //throws a BL exception
            }
            try
            {
                DroneDescription updateDrone = DroneList[index];
                updateDrone.battery -= BatteryAccToDistance(distance(collectDrone.initialLoc.latitude, collectDrone.initialLoc.longitude, collectDrone.myParcel.picking.latitude, collectDrone.myParcel.picking.longitude));
                updateDrone.loc = collectDrone.myParcel.picking;
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
            catch (Exception ex)
            {
                throw new InputNotValid("The parcel doesn't fit the requirements", ex);
            }


        }
        #endregion

        #region Delivered
        public void delivered(int DroneId)
        {
            DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new NotFound("Drone not found");
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
            catch (Exception ex)
            {
                throw new InputNotValid("The drone doesn't fit the requirements", ex);
            }
        }
        #endregion



   

    }
}