
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using BO;
using DalApi;
using System.Linq;
using BLApi;

namespace BL
{

   sealed partial class BL : IBL
    {

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

            IEnumerable<DO.DroneCharge> droneCharges = p.IEDroneChargeList();  //finds the list which contains the the drone charges from DAL
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
            Drone droneBL = new Drone();
            droneBL = GetDrone(droneId); //Copies the fields from DAL
            //the missing fields are:MaxWeight,Status,initialLoc,ParcelIndelivering
            DroneDescription tmp = new DroneDescription();
            tmp.loc = new Localisation();
           //DO.DroneCharge droneChargeDAL= p.IEDroneChargeList().ToList().Find(x => x.DroneId == droneId);
           // droneChargeDAL
            tmp = DroneList.Find(x => x.Id == droneId);
            droneBL.MaxWeight = tmp.weight;
            droneBL.Status = tmp.Status;
            droneBL.initialLoc = tmp.loc;
            ParcelInDelivering PID = new ParcelInDelivering();
            if (droneBL.Status == DroneStatuses.shipping)
            {
                foreach (var item in p.IEParcelList()) //finds the details of the parcel
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
                            PID.distance = distance(PID.picking.latitude, PID.picking.longitude, PID.delivered.latitude, PID.delivered.longitude);
                            if (item.PickedUp != null)
                                PID.deliveringStatus = true;
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

            clientBL.ParcLstFromClient = FindParcelsToClient(clientId); // copies the lists which contains the parcels he sent into the field

            clientBL.ParcLstToClient =FindParcelsFromClient(clientId); // gets the lists withall the parcels the client received

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
                throw new IDNotFound("Not found", ex);
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


        public DroneCharging displayDroneCharging(int stationId)
        {
            DroneCharging droneCharging = new DroneCharging();

            IEnumerable<DO.DroneCharge> droneCharges = p.IEDroneChargeList();  //finds the list which contains the the drone charges from DAL
            foreach (var item in droneCharges)
            {
                Drone droneInStation = GetDrone(item.DroneId);
                if (item.StationId == stationId)  // finds the station with the ID received 
                {
                    droneCharging.ID = item.DroneId;
                    droneCharging.battery = droneInStation.Battery;

                }


            }
            return droneCharging;
        }
        #endregion

    }
}