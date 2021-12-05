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
                DroneDescription droneBL = DroneList.Find(x => x.Id == droneId);
                DroneList.Remove(droneBL);
                droneDAL.Model = newModel;  //changes its model
                droneBL.Model = newModel;
                DroneList.Add(droneBL);
                p.UpdateDrone(droneDAL); // this function update the DAL drones' list
                
            }
            catch (IDAL.DO.DroneException ex)  //catches the DAL exception if there is one
            {
                throw new IDNotFound("Can't update the name", ex); //throws a BL exception
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
                IDAL.DO.Station stationDAL = p.StationById(stationId);  //finds the station according to its ID
                if (newName != "n")  //if the user wants to update the station's name
                    stationDAL.Name = newName;
                if (newCS != -1)    //if the user wants to update the station's number of chargeSlots
                {
                    stationDAL.ChargeSlots = newCS;
                    int notFreeChargeSlot = 0;
                    IEnumerable<IDAL.DO.DroneCharge> listDroneCharge = p.IEDroneChargeList();
                    foreach (var item in listDroneCharge) // calculates the charge slot that not free.
                    {
                        if (item.StationId == stationId)
                            notFreeChargeSlot++;
                    }
                    if (notFreeChargeSlot > newCS) // if new all charge slot count lower then the catch slots that alraedy exist.
                        throw new WrongDetailsUpdateException("All charge slot is to low");
                    stationDAL.ChargeSlots = newCS - notFreeChargeSlot;
                }
                p.UpdateStation(stationDAL);  // updates the station in the stations' list (DAL)
                
            }
            catch (IDAL.DO.StationException ex)   //catches the Dal exception
            {
                throw new IDNotFound("Can't update the station name and/or the number of chargeSlots", ex);  //throws a BL exception
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
                if (newName != "n")
                {
                    clientDAL.Phone = newTel;
                }
                p.UpdateClient(clientDAL);  //updates the client in dal
            }
            catch (IDAL.DO.ClientException ex)  //catches DAL exception
            {
                throw new IDNotFound("Can't update the client name", ex);  //throw a BL exception
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
                if (DistanceAccToBattery(blDrone.battery) >= d)
                    canGoToCharge = true;
                else
                    throw new NotEoughBattery("Can not send the drone to the station, it doesn't have enough battery!");
                if (canGoToCharge == true)
                {
                    blDrone.battery -= BatteryAccToDistance(DistanceAccToBattery(blDrone.battery));// substract the account of percetn from the battery to go to the nearest station
                    DroneDescription tempDD = new DroneDescription();//UPDATE DroneDescriptionLIST IN BL
                    tempDD = blDrone;
                    tempDD.loc = nearestStation.Loc;
                    tempDD.battery = blDrone.battery;
                    tempDD.Status = DroneStatuses.maintenance;
                    DroneList.Remove(blDrone);
                    DroneList.Add(tempDD);
                    try
                    {
                        p.AddFromBLDroneCharging(blDrone.Id, nearestStation.ID);//ADD The DroneCharge(drone+station) to DAL 
                    }
                    catch (IDAL.DO.DroneException ex)
                    {
                        throw new CannotAdd("The droneCharge cannot be added to DAL", ex);
                    }
                    IDAL.DO.Station stationDAL = new IDAL.DO.Station();
                    stationDAL.ID = nearestStation.ID;
                    stationDAL.Name = nearestStation.Name;
                    stationDAL.Latitude = nearestStation.Loc.latitude;
                    stationDAL.Longitude = nearestStation.Loc.longitude;
                    stationDAL.ChargeSlots = nearestStation.ChargeSlots - 1;
                    try
                    {
                        p.UpdateStation(stationDAL);// update the station in the dal list
                    }
                    catch (IDAL.DO.StationException ex)
                    {
                        throw new CannotUpdate("The station cannot be updated", ex);
                    }


                }

            }
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
            DroneList.Add(droneBL);

            //update station
            IDAL.DO.Station stationDAL = new IDAL.DO.Station();
            
            foreach (var item in p.IEStationList())
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
                throw new CannotUpdate("The station cannot be updated", ex);
            }
            bool flag = false;
            foreach (var item in p.IEDroneChargeList())
            {
                if (item.DroneId == droneBL.Id && item.StationId == stationDAL.ID)
                    flag = true;
            }
            if(flag==true)
                p.updateDroneChargeList(droneBL.Id, stationDAL.ID); // delete it from the dalDroneChargeList



        }
        #endregion

        #region Assignement
        /// <summary>
        /// connect parcel with drone if drone is free and parcel is defined.
        /// </summary>
        /// <param name="id"></param>
        public void Assignement(int id)
        {
            Drone connectDrone = new Drone();
            try
            {
                connectDrone = GetDrone(id);
            }
            catch (IDAL.DO.DroneException s)
            {
                throw new IDAL.DO.DroneException("" + s);
            }
            if (connectDrone.Status != DroneStatuses.free)
                throw new WrongDetailsUpdateException("Drone not free");
            Parcel parcelConnect = new Parcel();
            IEnumerable<ParcelDescription> parcelWithNoDrones = displayParcelsNotAssigned();
            ParcelDescription parcel = new ParcelDescription();
            List<ParcelDescription> priorityLevelList = new List<ParcelDescription>();
            List<ParcelDescription> weightLevelList = new List<ParcelDescription>();
            bool checkFoundParcel = false;
            bool checkAnyParcel = false;
            double batteryNeeded = 0;
            for (int i = (int)Priorities.emergency; i >= (int)Priorities.regular; i--)
            {
                priorityLevelList = parcelWithNoDrones.ToList().FindAll(parcel => parcel.priority == (Priorities)i);
                for (int j = (int)connectDrone.MaxWeight; j >= (int)WeightCategories.low; j--)
                {
                    weightLevelList = priorityLevelList.ToList().FindAll(parcel => parcel.weight == (WeightCategories)j);
                    while (checkFoundParcel == false && weightLevelList.Count() != 0)//stop search when you found approciate parcel, or when you didnt found - go over the next level of priority.
                    {
                        try // if the parcel exist makes calculates, if not go for next level of priority.
                        {
                            parcelConnect = ClosestParcelToLocation(connectDrone.initialLoc, weightLevelList);
                            checkAnyParcel = true;
                            double distance = Distance(connectDrone.initialLoc, GetClient(GetParcel(parcelConnect.ID).Sender.ID).ClientLoc);
                            batteryNeeded = distance * BatteryFree; // battery needed to go to sender
                            distance = Distance(GetClient(GetParcel(parcelConnect.ID).Sender.ID).ClientLoc, GetClient(GetParcel(parcelConnect.ID).Target.ID).ClientLoc);
                            if (parcelConnect.Weight == WeightCategories.heavy) //battery += battery needed to go to target.
                                batteryNeeded += distance * BatteryHeavyWeight;
                            if (parcelConnect.Weight == WeightCategories.low)
                                batteryNeeded += distance * BatteryLightWeight;
                            if (parcelConnect.Weight == WeightCategories.middle)
                                batteryNeeded += distance * BatteryMiddleWeight;
                            connectDrone.initialLoc = GetClient(GetParcel(parcelConnect.ID).Target.ID).ClientLoc; // update the dronelocation to the target location
                            IEnumerable<IDAL.DO.Station> listStationsFromIdal = p.IEStationList();
                            Localisation checkL = new Localisation(); // the location of closest station to the drone

                            try
                            {
                                {
                                    Station temp = NearestStation(connectDrone.initialLoc, true);
                                    checkL = temp.Loc;
                                }
                                distance = Distance(GetClient(GetParcel(parcelConnect.ID).Target.ID).ClientLoc, checkL); // if station exist - calculate the distance between her and the drone.
                            }
                            catch (IDAL.DO.StationException s)
                            {
                                throw new IDAL.DO.StationException("" + s); // if there is no free station to send the drone to
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
                        catch (WrongDetailsUpdateException s) // if parcel not exist, just go to next level of priority.
                        {
                            throw new WrongDetailsUpdateException("" + s);
                        }


                    }
                    if (checkFoundParcel == true)
                        break;
                }
            }
            if (checkAnyParcel == false)
                throw new WrongDetailsUpdateException("All parcels are in scheduled or weight is too high for the drone");
            if (checkFoundParcel == true) // if we found approciate parcel - do the changes
            {
                IDAL.DO.Parcel updateParcel = p.ParcelById(parcelConnect.ID); // update the droneid and scheduled time of the parcel.
                updateParcel.DroneId = connectDrone.ID;
                updateParcel.Scheduled = DateTime.Now;
                p.UpdateParcelFromBL(updateParcel);
                foreach (var droneItem in DroneList) // when we found the parcel to deliver.
                {
                    if (droneItem.Id == connectDrone.ID)
                    {
                        droneItem.Status = DroneStatuses.shipping; // change drone to be delivered
                        droneItem.Id = updateParcel.ID;
                    }

                }

            }
            else // if we didnt found approciate parcel - throw approciate message.
                throw new WrongDetailsUpdateException("Drone have not enough battery to reach to the parcels.");

        }

        #endregion

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