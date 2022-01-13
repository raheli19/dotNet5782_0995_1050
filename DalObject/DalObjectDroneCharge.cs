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

        #region AddDroneCharge
        /// <summary>
        /// Add a drone Charge to the droneChargeList
        /// </summary>
        /// <param name="dc"></param>
        public void AddDroneCharge(DroneCharge dc)
        {

            if (DataSource.DroneChargesList.Exists(DroneCharge => DroneCharge.DroneId == dc.DroneId))
            {
                throw new DroneException($"id {dc.DroneId} already exists!!");
            }
            DataSource.DroneChargesList.Add(dc);
        }
        #endregion

        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

        #region  UpdateDroneChargeList
        /// <summary>
        /// Updates details of a droneCharge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="statId"></param>
        public void UpdateDroneChargeList(int droneId, int statId)
        {
            DO.DroneCharge dalDr = new DroneCharge();
            foreach (var item in IEDroneChargeList())
            {
                if (item.DroneId == droneId && item.StationId == statId)
                {
                    dalDr.DroneId = item.DroneId;
                    dalDr.StationId = item.StationId;
                }



            }
            DataSource.DroneChargesList.Remove(dalDr);
        }

        #endregion

        //-----------------------------------ACTIONS-------------------------------------------

        #region IEDroneChargeList
        /// <summary>
        /// IEnumerable which returns the list of drones charge
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.DroneCharge> IEDroneChargeList()
        {
            List<DroneCharge> DroneChargeLst = new List<DroneCharge>();
            DroneChargeLst = DataSource.DroneChargesList;
            return DroneChargeLst;
        }
        #endregion



    }
}