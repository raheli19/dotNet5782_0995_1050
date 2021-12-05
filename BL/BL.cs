
//Tania: et Assignment verifier
// nouveau case dans le main pour imprimer la list des dronescharges
// tostring du drone si doit tout imprimer
// ajout d'un package dans les 2 clients concernés
// localisation a virgule
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

        private IDAL.IDal p;
        help h = new help();

        List<DroneDescription> DroneList = new List<DroneDescription>();

        public BL()
        {
            //    //constructor of the BL file
            p = new DalObject.DalObject();
            
            //initialisation de la droneList de la bl?????


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
        //                targetId1 = (p.ParcelById(ParcelInClient.ID)).SenderId;
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

        }


    }
}