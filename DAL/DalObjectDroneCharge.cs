using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal

    {

        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------



        #region addDroneCharge
        public void addDroneCharge(DroneCharge dc)
            {

                if (DroneChargesList.Exists(DroneCharge => DroneCharge.DroneId == dc.DroneId))
                {
                    throw new DroneException($"id {dc.DroneId} already exists!!");
                }
                DroneChargesList.Add(dc);
            }
            #endregion

            //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

            #region UPDATING
            public void updateDroneChargeList(int droneId, int statId)
            {
                foreach (var item in IEDroneChargeList())
                {
                    if (item.DroneId == droneId && item.StationId == statId)
                        DroneChargesList.Remove(item);
                }
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
                IDAL.DO.Drone d = new IDAL.DO.Drone();
                bool flag = false, flag2 = false;
                for (int i = 0; i < DroneChargeList.Count; i++)
                {
                    if (DroneChargeList[i].ID == droneId)
                    {
                        flag = true;
                        d = DroneChargeList[i];
                        DroneChargeList.Remove(DroneChargeList[i]);
                        break;
                    }
                }
                if (flag == false)
                {
                    throw new DroneException("drone not found");

                }
                //d.Status = DroneStatuses.maintenance;// plug in the drone to charge
                AddDrone(d);// add it back to the list
                IDAL.DO.Station s = new IDAL.DO.Station();
                for (int i = 0; i < StationList.Count; i++)
                {
                    if (StationList[i].ID == stationId)
                    {
                        flag2 = true;
                        s = StationList[i];
                        StationList.Remove(StationList[i]);
                        break;
                    }
                }
                if (flag2 == false)
                {
                    throw new StationException("parcel not found");
                }
                s.ChargeSlots--;// a slot if occupied by the new drone
                addStation(s);
                IDAL.DO.DroneCharge DC = new IDAL.DO.DroneCharge();
                DC.DroneId = droneId;
                DC.StationId = stationId;
                addDroneCharge(DC);// add the linked thing to the list

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
                foreach (var item in DroneChargesList)
                {
                    if (item.DroneId == droneId)
                    {
                        flag = true;
                        if (item.StationId == stationId)
                        {
                            flag2 = true;
                            DroneChargesList.Remove(item);// delete the item; the drone is not charging anymore
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
                IDAL.DO.Drone d = new IDAL.DO.Drone();
                for (int i = 0; i < DroneChargeList.Count; i++)
                {
                    if (DroneChargeList[i].ID == droneId)
                    {
                        flag3 = true;
                        d = DroneChargeList[i];// the drone is charged; he's free for shipping
                        DroneChargeList.Remove(DroneChargeList[i]);
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
                IDAL.DO.Station s = new IDAL.DO.Station();
                for (int i = 0; i < StationList.Count; i++)
                {
                    if (StationList[i].ID == stationId)
                    {
                        flag4 = true;
                        s = StationList[i];
                        StationList.Remove(StationList[i]);
                        break;
                    }
                }
                if (flag4 == false)
                {
                    throw new StationException("station not found");
                }
                s.ChargeSlots++;// one is free from the charged drone
                addStation(s);
            }
            #endregion


            #region IENUMERABLE

            public IEnumerable<DroneCharge> IEDroneChargeList()
            {
                List<DroneCharge> DroneChargeLst = new List<DroneCharge>();
                DroneChargeLst = DroneChargesList;
                return DroneChargeLst;
            }
            #endregion
      

        
    }
}