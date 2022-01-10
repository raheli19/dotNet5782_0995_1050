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

        public IEnumerable<DO.DroneCharge> IEDroneChargeList()
        {
            List<DroneCharge> DroneChargeLst = new List<DroneCharge>();
            DroneChargeLst = DataSource.DroneChargesList;
            return DroneChargeLst;
        }
        #endregion



    }
}