
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


            IEnumerable<IDAL.DO.DroneCharge> droneCharges = p.IEDroneChargeList();
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
                foreach (var item in p.IEParcelList())
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

            foreach (var item in p.IEParcelList())
            {
                if (item.SenderId == clientId)  //The parcel has been sent by this client
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
                    TempParcLstFromClient.Add(PCT);

                }
            }
            c.ParcLstFromClient = TempParcLstFromClient;


            List<ParcelToClient> TempParcLstToClient = new List<ParcelToClient>();

            foreach (var item in p.IEParcelList())
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
                throw new IDNotFound("Not found", ex);
            }
            foreach (var item in DroneList)
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

        #endregion

    }
}