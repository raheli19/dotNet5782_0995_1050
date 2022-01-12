//Tania:DroneToCharge, DroneCharged et Assignment verifier

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DalApi;
using BO;
using BLApi;

namespace BL
{

    sealed partial class BL : IBL
    {
        
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
                DO.Drone droneDAL = dal.DroneById(droneId); // find the DALdrone
                BO.DroneDescription droneBL = DroneList.Find(x => x.Id == droneId);
                DroneList.Remove(droneBL);
                droneDAL.Model = newModel;  //changes its model
                droneBL.Model = newModel;
                DroneList.Add(droneBL);
                dal.UpdateDrone(droneDAL); // this function update the DAL drones' list
                
            }
            catch (DO.DroneException ex)  //catches the DAL exception if there is one
            {
                throw new BO.IDNotFound("Can't update the name", ex); //throws a BL exception
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
        public void updateStationName_CS(int stationId, string newName = "n", int newCS = -1)
        {
            try
            {
                DO.Station stationDAL = dal.StationById(stationId);  //finds the station according to its ID
                if (newName != "n")  //if the user wants to update the station's name
                    stationDAL.Name = newName;
                if (newCS != -1)    //if the user wants to update the station's number of chargeSlots
                {
                    stationDAL.ChargeSlots = newCS;
                    int notFreeChargeSlot = 0;
                    IEnumerable<DO.DroneCharge> listDroneCharge = dal.IEDroneChargeList();
                    foreach (var item in listDroneCharge) // calculates the charge slot that not free.
                    {
                        if (item.StationId == stationId)
                            notFreeChargeSlot++;
                    }
                    if (notFreeChargeSlot > newCS) // if new all charge slot count lower then the catch slots that alraedy exist.
                        throw new BO.WrongDetailsUpdateException("All charge slot is to low");
                    stationDAL.ChargeSlots = newCS - notFreeChargeSlot;
                }
                dal.UpdateStation(stationDAL);  // updates the station in the stations' list (DAL)
                
            }
            catch (DO.StationException ex)   //catches the Dal exception
            {
                throw new BO.IDNotFound("Can't update the station name and/or the number of chargeSlots", ex);  //throws a BL exception
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
        public void updateClientName_Phone(int clientId, string newName = "", string newTel = "n")
        {
            try
            {
                DO.Client clientDAL = dal.ClientById(clientId);  //search the client in dal
                if (newName != "")  //if the user wants to update the client's name
                {
                    clientDAL.Name = newName;
                }
                if (newTel!= "n")
                {
                    clientDAL.Phone = newTel;
                }
                dal.UpdateClient(clientDAL);  //updates the client in dal
            }
            catch (DO.ClientException ex)  //catches DAL exception
            {
                throw new BO.IDNotFound("Can't update the client name", ex);  //throw a BL exception
            }

        }
        #endregion

        #region DroneToCharge
        public void DroneToCharge(int DroneId)
        {
            
            DroneDescription blDrone = new DroneDescription();
            blDrone.loc = new Localisation();
            blDrone = DroneList.Find(Drone => Drone.Id == DroneId);

            if (blDrone == null)
            {
                throw new NotFound("Drone not found");
            }
            if (blDrone.Status == DroneStatuses.free)
            {
                Station nearestStation = NearestStation(blDrone.loc, true);
                double d = distance(blDrone.loc.latitude, blDrone.loc.longitude, nearestStation.Loc.latitude, nearestStation.Loc.longitude);
                bool canGoToCharge = false;
                double distacctobatt = DistanceAccToBattery(blDrone.battery);
                if (distacctobatt >= d)
                    canGoToCharge = true;
                else
                    throw new NotEoughBattery("Can not send the drone to the station, it doesn't have enough battery!");
                if (canGoToCharge == true)
                {
                    blDrone.battery = BatteryAccToDistance(distacctobatt-d);// substract the account of percetn from the battery to go to the nearest station
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD = blDrone;
                    tempDD.loc = nearestStation.Loc;
                    tempDD.battery = blDrone.battery;
                    tempDD.Status = DroneStatuses.maintenance;
                    DroneList.Remove(blDrone);
                    DroneList.Add(tempDD);
                    try
                    {
                        dal.AddFromBLDroneCharging(blDrone.Id, nearestStation.ID);//ADD The DroneCharge(drone+station) to DAL 
                    }
                    catch (DO.DroneException ex)
                    {
                        throw new CannotAdd("The droneCharge cannot be added to DAL", ex);
                    }
                    DO.Station stationDAL = new DO.Station();
                    stationDAL.ID = nearestStation.ID;
                    stationDAL.Name = nearestStation.Name;
                    stationDAL.Latitude = nearestStation.Loc.latitude;
                    stationDAL.Longitude = nearestStation.Loc.longitude;
                    stationDAL.ChargeSlots = nearestStation.ChargeSlots - 1;
                    try
                    {
                        dal.UpdateStation(stationDAL);// update the station in the dal list
                    }
                    catch (DO.StationException ex)
                    {
                        throw new CannotUpdate("The station cannot be updated", ex);
                    }


                }

            }
            else if (blDrone.Status == DroneStatuses.maintenance)
                throw new NotAvailable("The drone is already charging!");
            else
                throw new NotAvailable("The drone isn't avaiblable!");
        }
        #endregion

        #region DroneCharged
        public void DroneCharged(int ID, double time)
        {
            DroneDescription droneBL = new DroneDescription();
            droneBL.loc = new Localisation();
            //droneBL.Id = ID;
            //droneBL.Status = DroneStatuses.free; // signs if we don't find it
            droneBL = DroneList.Find(x => ID == x.Id);
            
            if (droneBL == null)
                throw new IDNotFound("This drone doesn't exists!");
            if (droneBL.Status != DroneStatuses.maintenance)// if the drone is not charging
                throw new InputNotValid("This drone isn't charging");
            DroneList.Remove(droneBL);

            // upadte the drone in the bl DroneList
            droneBL.Status = DroneStatuses.free;
            droneBL.battery = BatteryAccToTime(time, droneBL.battery);
            if (droneBL.battery >= 100)
                droneBL.battery = 100; 
            DroneList.Add(droneBL);

            //update station
            DO.Station stationDAL = new DO.Station();
            
            foreach (var item in dal.IEStationList())
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
                dal.UpdateStation(stationDAL); // puts back the station with one more chargeSlot free
            }
            catch (DO.StationException ex)
            {
                throw new CannotUpdate("The station cannot be updated", ex);
            }
            bool flag = false;
            foreach (var item in dal.IEDroneChargeList())
            {
                if (item.DroneId == droneBL.Id && item.StationId == stationDAL.ID)
                    flag = true;
            }
            if(flag==true)
                dal.UpdateDroneChargeList(droneBL.Id, stationDAL.ID); // delete it from the dalDroneChargeList



        }
        #endregion

        #region Assignement
        /// <summary>
        /// connect parcel with drone if drone is free and parcel is defined.
        /// </summary>
        /// <param name="id"></param>
        public void Assignement(int id)
        {
            BO.Drone connectDrone;
            try
            {
                connectDrone = displayDrone(id);
            }
            catch (DO.DroneException s)
            {
                throw new DO.DroneException("" + s);
            }
            if (connectDrone.Status != BO.DroneStatuses.free)
                throw new BO.WrongDetailsUpdateException("Drone not free");
            BO.Parcel parcelConnect = new BO.Parcel();
            IEnumerable<BO.ParcelDescription> parcelWithNoDrones = displayParcelsNotAssigned();
            BO.ParcelDescription parcel = new BO.ParcelDescription();
            List<BO.ParcelDescription> priorityLevelList = new List<BO.ParcelDescription>();
            List<BO.ParcelDescription> weightLevelList = new List<BO.ParcelDescription>();
            bool checkFoundParcel = false;
            bool checkAnyParcel = false;
            double batteryNeeded = 0;
            for (int i = (int)BO.Priorities.emergency; i >= (int)BO.Priorities.regular; i--)
            {
                priorityLevelList = parcelWithNoDrones.ToList().FindAll(parcel => parcel.priority == (BO.Priorities)i && parcel.Status==BO.ParcelStatus.requested);
                for (int j = (int)connectDrone.MaxWeight; j >= (int)BO.WeightCategories.low; j--)
                {
                    weightLevelList = priorityLevelList.ToList().FindAll(parcel => parcel.weight == (BO.WeightCategories)j);
                    while (checkFoundParcel == false && weightLevelList.Count() != 0)//stop search when you found approciate parcel, or when you didnt found - go over the next level of priority.
                    {
                        try // if the parcel exist makes calculates, if not go for next level of priority.
                        {
                            parcelConnect = ClosestParcelToLocation(connectDrone.initialLoc, weightLevelList);
                            checkAnyParcel = true;
                            double distance = Distance(connectDrone.initialLoc, GetClient(GetParcel(parcelConnect.ID).Sender.ID).ClientLoc);
                            batteryNeeded = distance * BatteryFree; // battery needed to go to sender
                            distance = Distance(GetClient(GetParcel(parcelConnect.ID).Sender.ID).ClientLoc, GetClient(GetParcel(parcelConnect.ID).Target.ID).ClientLoc);
                            if (parcelConnect.Weight == BO.WeightCategories.heavy) //battery += battery needed to go to target.
                                batteryNeeded += distance * BatteryHeavyWeight;
                            if (parcelConnect.Weight == BO.WeightCategories.low)
                                batteryNeeded += distance * BatteryLightWeight;
                            if (parcelConnect.Weight == BO.WeightCategories.middle)
                                batteryNeeded += distance * BatteryMiddleWeight;

                            connectDrone.initialLoc = GetClient(GetParcel(parcelConnect.ID).Target.ID).ClientLoc; // update the dronelocation to the target location
                            IEnumerable<DO.Station> listStationsFromIdal = dal.IEStationList();
                            BO.Localisation checkL = new BO.Localisation(); // the location of closest station to the drone

                            try
                            {
                                {
                                    BO.Station temp = NearestStation(connectDrone.initialLoc, true);
                                    checkL = temp.Loc;
                                }
                                distance = Distance(GetClient(GetParcel(parcelConnect.ID).Target.ID).ClientLoc, checkL); // if station exist - calculate the distance between her and the drone.
                            }
                            catch (DO.StationException s)
                            {
                                throw new DO.StationException("" + s); // if there is no free station to send the drone to
                            }

                            batteryNeeded += distance * BatteryFree; // battery +=battery needed from target to closest free station
                            if (connectDrone.Battery >= batteryNeeded)//if the battery is enough - stop search.
                            {
                                checkFoundParcel = true;
                                break;
                            }

                            else
                                weightLevelList.RemoveAll(parcel => parcel.Id == parcelConnect.ID); // if battery is not enough - go over all the left parcels.
                        }
                        catch (BO.WrongDetailsUpdateException s) // if parcel not exist, just go to next level of priority.
                        {
                            throw new BO.WrongDetailsUpdateException("" + s);
                        }


                    }
                    if (checkFoundParcel == true)
                        break;
                }
            }
            if (checkAnyParcel == false)
                throw new BO.WrongDetailsUpdateException("All parcels are in scheduled or weight is too high for the drone");
            if (checkFoundParcel == true) // if we found approciate parcel - do the changes
            {
                DO.Parcel updateParcel = dal.ParcelById(parcelConnect.ID); // update the droneid and scheduled time of the parcel.
                updateParcel.DroneId = connectDrone.ID;
                updateParcel.Scheduled = DateTime.Now;
                dal.UpdateParcelFromBL(updateParcel);
                foreach (var droneItem in DroneList) // when we found the parcel to deliver.
                {
                    if (droneItem.Id == connectDrone.ID)
                    {
                        droneItem.Status = BO.DroneStatuses.shipping; // change drone to be delivered
                        droneItem.parcelId = updateParcel.ID;
                    }

                }

            }
            else // if we didnt found approciate parcel - throw approciate message.
                throw new BO.WrongDetailsUpdateException("Drone have not enough battery to reach to the parcels.");

        }

        #endregion

        #region PickedUp
        /// <summary>
        /// This function gets a droneId,finds the drone and this drone picks the parcel associated to it
        /// </summary>
        /// <param name="DroneId"></param>
        public void PickedUp(int droneId)
        {
            BO.Drone collectDrone = new BO.Drone();
            try
            {
                collectDrone = displayDrone(droneId);
            }
            catch (DO.DroneException ex)
            {
                throw new DO.DroneException("" + ex);
            }

            if (collectDrone.Status != BO.DroneStatuses.shipping) //if the drone is not scheduled
                throw new BO.WrongDetailsUpdateException("Drone is not scheduled to any parcel");
            if (collectDrone.myParcel.deliveringStatus == true) //if parcel is already on the drone
                throw new BO.WrongDetailsUpdateException("Drone in the middle of shipping");

            BO.DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == droneId);  //finds the drone in the droneList in BL
            int index = DroneList.FindIndex(Drone => Drone.Id == droneId);
            //myDrone.loc = new Localisation();
           // collectDrone.initialLoc = new Localisation();
            if (myDrone == null)  //the drone is not among the drone List
            {
                throw new BO.IDNotFound("Drone not found");  //throws a BL exception
            }
            try
            {
                BO.DroneDescription updateDrone = DroneList[index];
                if (updateDrone.battery - BatteryAccToDistance(distance(collectDrone.initialLoc.latitude, collectDrone.initialLoc.longitude, collectDrone.myParcel.picking.latitude, collectDrone.myParcel.picking.longitude)) > 0)
                {
                    updateDrone.battery -= BatteryAccToDistance(distance(collectDrone.initialLoc.latitude, collectDrone.initialLoc.longitude, collectDrone.myParcel.picking.latitude, collectDrone.myParcel.picking.longitude));
                    updateDrone.loc = displayClient( collectDrone.myParcel.Sender.ID).ClientLoc;

                    DroneList[index] = updateDrone;


                    DO.Parcel parcelDal = dal.ParcelById(collectDrone.myParcel.ID);
                    parcelDal.PickedUp = DateTime.Now;

                    dal.UpdateParcelFromBL(parcelDal);
                }
                else
                    throw new Exception("The drone doesn't have enough battery to collect the parcel");
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
                //    DO.Parcel tempParcel = new DO.Parcel();
                //    tempParcel = parcelDal;
                //    tempParcel.PickedUp = DateTime.Now;
                //    p.AddParcelFromBL(tempParcel);
                //}
            }
            catch (Exception ex)
            {
                throw new BO.InputNotValid( ex.Message);
            }


        }
        #endregion

        #region Delivered
        public void delivered(int DroneId)
        {
            BO.DroneDescription myDrone = DroneList.Find(Drone => Drone.Id == DroneId);
            if (myDrone == null)
            {
                throw new BO.NotFound("Drone not found");
            }
            try
            {
                DO.Parcel prcel = dal.FindParcelAssociatedWithDrone(DroneId);


                if (prcel.PickedUp !=null && prcel.Delivered == null)
                {

                    int targetId = prcel.TargetId;
                    BO.Localisation targetLoc = new BO.Localisation();
                    targetLoc.latitude = dal.FindLat(targetId);
                    targetLoc.longitude = dal.FindLong(targetId);
                    double myDistance = distance(myDrone.loc.latitude, myDrone.loc.longitude, targetLoc.latitude, targetLoc.longitude);
                    BO.DroneDescription tempDD = new BO.DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD.loc = new BO.Localisation();
                    tempDD = myDrone;
                    tempDD.battery -= BatteryAccToDistance(myDistance);
                    tempDD.loc = targetLoc;
                    tempDD.Status = BO.DroneStatuses.free;
                    tempDD.parcelId = 0;
                    DroneList.Remove(myDrone);
                    DroneList.Add(tempDD);
                    DO.Parcel tempParcel = new DO.Parcel();
                    tempParcel = prcel;
                    tempParcel.Delivered = DateTime.Now;
                    tempParcel.DroneId = 0;
                    dal.AddParcelFromBL(tempParcel);

                }
                else
                    throw new InvalidOperationException("There is no parcel to deliver");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("There is no parcel to deliver");
            }
        }
        #endregion

        public void StartSimulator(int id, Action action, Func<bool> stop) 
        {
            Simulator simulator = new Simulator(this, id, action, stop);
        }
    }
}