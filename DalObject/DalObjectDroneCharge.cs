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

            #region UPDATING
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

            #region DroneToCharge
            /// <summary>
            /// Lead a drone to a station of chargement
            /// </summary>
            /// <param name="droneId"></param>
            /// <param name="stationId"></param>
            public void DroneToCharge(int droneId, int stationId)
            {
                 DO.Drone dalDrone = new  DO.Drone(); //drone from the dal
                bool flag = false, flag2 = false;
                for (int i = 0; i < DataSource.DroneChargeList.Count; i++)
                {
                    if (DataSource.DroneChargeList[i].ID == droneId)
                    {
                        flag = true;
                        dalDrone = DataSource.DroneChargeList[i]; // gets all the info from the dal
                    DataSource.DroneChargeList.Remove(DataSource.DroneChargeList[i]);
                        break;
                    }
                }
                if (flag == false)
                {
                    throw new DroneException("drone not found");

                }
                //d.Status = DroneStatuses.maintenance;// plug in the drone to charge
                AddDrone(dalDrone);// add it back to the list
                 DO.Station s = new  DO.Station();
                for (int i = 0; i < DataSource.StationList.Count; i++)
                {
                    if (DataSource.StationList[i].ID == stationId)
                    {
                        flag2 = true;
                        s = DataSource.StationList[i];
                    DataSource.StationList.Remove(DataSource.StationList[i]);
                        break;
                    }
                }
                if (flag2 == false)
                {
                    throw new StationException("parcel not found");
                }
                s.ChargeSlots--;// a slot if occupied by the new drone
                AddStation(s);
                 DO.DroneCharge DC = new  DO.DroneCharge();
                DC.DroneId = droneId;
                DC.StationId = stationId;
                AddDroneCharge(DC);// add the linked thing to the list

            }
            #endregion

            #region DroneCharged
            /// <summary>
            /// The drone has finished charging. We need to let it go
            /// </summary>
            /// <param name="droneId"></param>
            /// <param name="stationId"></param>
            public void DroneCharged(int droneId, int stationId)
            {
                bool flag = false, flag2 = false;
                foreach (var item in DataSource.DroneChargesList)
                {
                    if (item.DroneId == droneId)
                    {
                        flag = true;
                        if (item.StationId == stationId)
                        {
                            flag2 = true;
                            UpdateDroneChargeList(item.DroneId, item.StationId);// delete the item; the drone is not charging anymore
                                         //DataSource.DroneChargesList.Remove(item);
                        }
                    }

                }

                if (flag == false)
                {
                    throw new DroneException("drone not found");
                }
                if (flag2 == false)
                {
                    throw new StationException("station not found");
                }
                bool flag3 = false;
                 DO.Drone d = new  DO.Drone();
                for (int i = 0; i < DataSource.DroneChargeList.Count; i++)
                {
                    if (DataSource.DroneChargeList[i].ID == droneId)
                    {
                        flag3 = true;
                        d = DataSource.DroneChargeList[i];// the drone is charged; he's free for shipping

                    DataSource.DroneChargeList.Remove(DataSource.DroneChargeList[i]);
                        break;
                    }

                }
                if (flag3 == false)
                {
                    throw new DroneException("drone not found");
                }
                bool flag4 = false;
                //d.Status = DroneStatuses.free;
                AddDrone(d);
                 DO.Station s = new  DO.Station();
                for (int i = 0; i < DataSource.StationList.Count; i++)
                {
                    if (DataSource.StationList[i].ID == stationId)
                    {
                        flag4 = true;
                        s = DataSource.StationList[i];
                    DataSource.StationList.Remove(DataSource.StationList[i]);
                        break;
                    }
                }
                if (flag4 == false)
                {
                    throw new StationException("station not found");
                }
                s.ChargeSlots++;// one is free from the charged drone
                AddStation(s);
            }
            #endregion


            #region IENUMERABLE

            public IEnumerable<DO.DroneCharge> IEDroneChargeList()
            {
                        List<DroneCharge> DroneChargeLst = new List<DroneCharge>();
                DroneChargeLst = DataSource.DroneChargesList;
                return DroneChargeLst;
            }
            #endregion
      

        
    }
}