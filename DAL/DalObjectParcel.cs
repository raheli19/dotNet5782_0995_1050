using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        //public static int ID { get; private set; }
        //public static int DroneId { get; private set; }  

        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------


        #region addParcel
        public void addParcel(Parcel pl)
        {
            //if (DataSource.ParcelList.Exists(parcel => parcel.ID == pl.ID))
            //{
            //    throw new ParcelException($"id {pl.ID} already exists!!");
            //}
            pl.ID = DataSource.Config.RunnerIDnumber;
            DataSource.ParcelList.Add(pl);
            DataSource.Config.RunnerIDnumber++;
        }
        #endregion

        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

        public void UpdateParcel(Parcel parcel)
        {
            if (!(DataSource.ParcelList.Exists(p => p.ID == parcel.ID)))
            {
                throw new ParcelException($"id {parcel.ID} is not valid!!");
            }
            int index = DataSource.ParcelList.FindIndex(item => item.ID == parcel.ID);
            DataSource.ParcelList[index] = parcel;
        }
        #region UPDATING

        #region UpdateParFromBL
        public void UpdateParcelFromBL(Parcel ParcelToUpdate)
        {
            Parcel myParcel = new Parcel();
            myParcel.ID = -1;
            myParcel = DataSource.ParcelList.Find(x => x.ID == ParcelToUpdate.ID);
            if (myParcel.ID == -1)
            {
                throw new ParcelException("This Parcel doesn't exist in the system.");

            }
            DataSource.ParcelList.Remove(myParcel);
            myParcel.ID = ParcelToUpdate.ID;
            myParcel.SenderId = ParcelToUpdate.SenderId;
            myParcel.TargetId = ParcelToUpdate.TargetId;
            myParcel.Weight = ParcelToUpdate.Weight;
            myParcel.Priority = ParcelToUpdate.Priority;
            myParcel.Requested = ParcelToUpdate.Requested;
            myParcel.DroneId = ParcelToUpdate.DroneId;
            myParcel.Scheduled = ParcelToUpdate.Scheduled;
            myParcel.PickedUp = ParcelToUpdate.PickedUp;
            myParcel.Delivered = ParcelToUpdate.Delivered;
            DataSource.ParcelList.Add(myParcel);
        }
        #endregion
        #endregion

        //-----------------------------------ACTIONS-------------------------------------------

        //3e vides pck aussi dans la drone
        #region AssignementFunction
        ///// <summary>
        ///// Function which assigns a parcelto a drone
        ///// </summary>
        ///// <param name="parcelId"></param>
        ///// <param name="droneId"></param>
        //public void Assignement(int parcelId, int droneId)
        //{
        //    IDAL.DO.Parcel p = new IDAL.DO.Parcel();
        //    IDAL.DO.Drone d = new IDAL.DO.Drone();
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
        //    addParcel(p);

        //}
        #endregion

        #region PickedUpFunction
        ///// <summary>
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
        //    IDAL.DO.Parcel p = new IDAL.DO.Parcel();
        //    IDAL.DO.Drone d = new IDAL.DO.Drone();
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

        #region ParcelById
        /// <summary>
        /// Receives an id and returns the parcel which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parcel ParcelById(int id)
        {
            Parcel pToReturn = default;
            if (!DataSource.ParcelList.Exists(parcel => parcel.ID == id))
            {
                throw new ParcelException($"id {id} doesn't exist!!");

            };
            pToReturn = DataSource.ParcelList.Find(p => p.ID == id);
            return pToReturn;
        }
        #endregion

        #region IENUMERABLE
        #region ParcelList
        /// <summary>
        /// Returns the parcels' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> IEParcelList()
        {
            List<Parcel> ParcelLst = new List<Parcel>();
            ParcelLst = DataSource.ParcelList;
            return ParcelLst;
        }
        #endregion
        #endregion

        //----------------------------HELP---------------------
    }
}

       
       