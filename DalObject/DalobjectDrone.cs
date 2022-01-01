using System;
using System.Collections.Generic;
using DO;
using DalApi;
using Dal;


namespace Dal
{

    sealed partial class DalObject : IDal
    {

        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------

        #region AddDrone
        public void AddDrone(Drone d)// add a new drone to the dronelist
        {
            if (DataSource.DroneChargeList.Exists(drone => drone.ID == d.ID))
            {
                throw new DroneException($"id {d.ID} already exists!!");
            }

            DataSource.DroneChargeList.Add(d);
        }
        #endregion

        //#region AddParcelToDrone
        ///// <summary>
        ///// Function which associate a parcel to a drone by its id
        ///// </summary>
        ///// <param name="parcel"></param>
        //public void AddParcelToDrone(Parcel parcel, Drone d) // associate a parcel to a drone
        //{
        //    parcel.DroneId = d.ID;
        //    UpdateParcel(parcel);
        //}



        //#endregion

        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

        #region UPDATING
        #region UpdateDrone
        public void UpdateDrone(Drone droneToUpdate)
        {
            Drone myDrone = new Drone();
            myDrone.ID = -1;
            myDrone = DataSource.DroneChargeList.Find(x => x.ID == droneToUpdate.ID);

            if (myDrone.ID == -1)
            {
                throw new DroneException("This drone doesn't exist in the system.");

            }
            DataSource.DroneChargeList.Remove(myDrone);
            myDrone.ID = droneToUpdate.ID;
            myDrone.Model = droneToUpdate.Model;
            myDrone.weight = droneToUpdate.weight;
            DataSource.DroneChargeList.Add(myDrone);

        }
        #endregion

        #endregion

        //-----------------------------------ACTIONS-------------------------------------------

        #region AssignementFunction
        ///// <summary>
        ///// Function which assigns a parcelto a drone
        ///// </summary>
        ///// <param name="parcelId"></param>
        ///// <param name="droneId"></param>
        //public void Assignement(int parcelId, int droneId)
        //{
        //    DO.Parcel p = new DO.Parcel();
        //    DO.Drone d = new DO.Drone();
        //    bool flag = false, flag2 = false;
        //    foreach (var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is not
        //    {
        //        if (item.ID == parcelId)
        //        {
        //            p = item;
        //            DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
        //            flag = true; // we found it
        //            break;
        //        }
        //    }
        //    if (flag == false)
        //    {
        //        throw new ParcelException("parcel not found");
        //    }

        //    foreach (var item in DataSource.DroneChargeList)//search in the list of Drones where the ID we received is
        //    {
        //        if (item.ID == droneId)
        //        {
        //            d = item;
        //            DataSource.DroneChargeList.Remove(item);// deletes the current item from the list, and we'll add the modified one
        //            flag2 = true;
        //            break;
        //        }
        //    }
        //    if (flag2 == false)
        //    {
        //        throw new DroneException("drone not found");
        //    }
        //    p.DroneId = d.ID;
        //    p.Requested = DateTime.Now;
        //    // add the modified items into their lists
        //    AddDrone(d);
        //    AddParcel(p);

        //}
        #endregion

        #region PickedUpFunction
        /// <summary>
        ///// To pick a parcel contained in a drone and update their statuses
        ///// </summary>
        ///// <param name="parcelId"></param>
        ///// <param name="droneId"></param>
        //public void IsPickedUp(int parcelId, int droneId)
        //{
        //    if (parcelId == -1)
        //    {
        //        throw new ParcelException($"id {parcelId} does not exist !!");
        //    }

        //    Parcel p = DataSource.ParcelList[parcelId];
        //    p.DroneId = droneId;
        //    p.PickedUp = DateTime.Now;
        //    DataSource.ParcelList[parcelId] = p;

        //    if (droneId == -1)
        //    {
        //        throw new DroneException($"id {droneId} does not exist !!");
        //    }

        //    Drone d = DataSource.DroneChargeList[droneId];
        //    //Update the drone status into delivery

        //}
        #endregion

        #region DeliveredFunction
        ///// <summary>
        ///// To deliver a package to a customer
        ///// </summary>
        ///// <param name="parcelId"></param>
        //public void DeliveredToClient(int parcelId)//deliver a package to a customer
        //{
        //    DO.Parcel p = new DO.Parcel();
        //    DO.Drone d = new DO.Drone();
        //    bool flag = false, flag2 = false;
        //    foreach (var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
        //    {
        //        if (item.ID == parcelId)
        //        {
        //            flag = true;
        //            p = item;// save the current item
        //            DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
        //            break;
        //        }
        //    }
        //    if (flag == false)
        //    {
        //        throw new ParcelException("parcel not found");
        //    }

        //    foreach (var item in DataSource.DroneChargeList)//search in the list of Drones where the ID we received is
        //    {
        //        if (item.ID == p.DroneId)
        //        {
        //            flag2 = true;
        //            d = item;
        //            DataSource.DroneChargeList.Remove(item);// remove it from the list
        //            break;
        //        }
        //    }
        //    if (flag2 == false)
        //    {
        //        throw new DroneException("drone not found");
        //    }
        //    p.Delivered = DateTime.Now;// time of delivering
        //    p.DroneId = 000000;
        //    //d.Status=DroneStatuses.free; // the drone is free
        //}
        #endregion


        #region DroneById
        /// <summary>
        /// Receives an id and returns the drone which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Drone DroneById(int id)
        {
            Drone dToReturn = default;
            if (!DataSource.DroneChargeList.Exists(drone => drone.ID == id))
            {
                throw new DroneException($"id {id} doesn't exist!!");

            };
            dToReturn = DataSource.DroneChargeList.Find(d => d.ID == id);
            return dToReturn;
        }
        #endregion

        #region IENUMERABLE

        #region DroneList
        /// <summary>
        /// Returns the drones' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Drone> IEDroneList()
        {
            List<Drone> DroneLst = new List<Drone>();
            DroneLst = DataSource.DroneChargeList;
            return DroneLst;
        }
        #endregion

        #endregion



    }
}