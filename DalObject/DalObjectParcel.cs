using System;
using System.Collections.Generic;
using DO;
using Dal;
using DalApi;

namespace Dal
{
    sealed partial class DalObject : IDal
    {

        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------

        #region AddParcel
        /// <summary>
        /// Add a new parcel to the parcel list
        /// </summary>
        /// <param name="pl"></param>
        public void AddParcel(Parcel pl)
        {
            pl.ID = Configuration.RunnerIDnumber;
            DataSource.ParcelList.Add(pl);
            Configuration.RunnerIDnumber++;
        }
        #endregion

        #region RemoveParcel
        /// <summary>
        /// Removes a parcel from the parcel list
        /// </summary>
        /// <param name="p"></param>
        public void RemoveParcel(Parcel p)
        {

            for (int i = 0; i < DataSource.ParcelList.Count; i++)
            {
                if (DataSource.ParcelList[i].ID == p.ID)
                {
                    DataSource.ParcelList.RemoveAt(i);
                    break;
                }
            }
        }
        #endregion

        //-----------------------------------UPDATE-FUNCTIONS---------------------------------------------

        #region UpdateParcel
        /// <summary>
        /// Updates the details of a parcel
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateParcel(Parcel parcel)
        {
            if (!(DataSource.ParcelList.Exists(p => p.ID == parcel.ID)))
            {
                throw new ParcelException($"id {parcel.ID} is not valid!!");
            }
            int index = DataSource.ParcelList.FindIndex(item => item.ID == parcel.ID);
            DataSource.ParcelList[index] = parcel;
        }
        #endregion

        #region UpdateParFromBL
        /// <summary>
        /// Updates the details of a parcel from bl
        /// </summary>
        /// <param name="ParcelToUpdate"></param>
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
        

        //-----------------------------------GET PARCEL AND PARCEL LIST------------------------------------


        #region ParcelById
        /// <summary>
        /// Receives an id and returns the parcel which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Parcel ParcelById(int id)
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

        #region DalGetIdParcel
        /// <summary>
        /// Receives a sender and target id and finds the id of the parcel which contains these details
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="TargetId"></param>
        /// <returns></returns>
        public int DalGetIdParcel(int senderId, int TargetId)
        {
            int id = DataSource.ParcelList.Find(x => x.SenderId == senderId && x.TargetId == TargetId).ID;
            return id;
        }
        #endregion

        #region ParcelList
        /// <summary>
        /// Returns the parcels' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Parcel> IEParcelList()
        {
            List<DO.Parcel> ParcelLst = new List<DO.Parcel>();
            ParcelLst = DataSource.ParcelList;
            return ParcelLst;
        }
        #endregion
   

    }
}


