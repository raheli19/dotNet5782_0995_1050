using System;
using System.Collections.Generic;
using DO;
using Dal;
using DalApi;

namespace Dal
{
    sealed partial class DalObject : IDal
    {


        //---------------------------------------------------HELP------------------------------------------------

        #region FindParcelAssociatedWithDrone

        /// <summary>
        ///  Find Parcel Associated With Drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Parcel FindParcelAssociatedWithDrone(int droneId)
        {
            Parcel myParcel = new Parcel();

            if (DataSource.ParcelList.Exists(x => x.DroneId == droneId))
            {
                myParcel = DataSource.ParcelList.Find(x => x.DroneId == droneId);
            }
            else
            {
                throw new ParcelException("There is no parcel which contains this droneID!");
            }

            return myParcel;
        }
        #endregion

        #region FindLat
        /// <summary>
        /// Find latitude of client by its id
        /// </summary>
        /// <param name="myID"></param>
        /// <returns></returns>
        public double FindLat(int myID)
        {
            Client myClient = new Client();

            if (DataSource.ClientList.Exists(x => x.ID == myID))
            {
                myClient = DataSource.ClientList.Find(x => x.ID == myID);

            }
            else
            {
                throw new ClientException("There is not Client with such ID");
            }
            return (myClient).Latitude;
        }
        #endregion

        #region FindLong
        /// <summary>
        /// Find longitude of client by its id
        /// </summary>
        /// <param name="myID"></param>
        /// <returns></returns>
        public double FindLong(int myID)
        {
            Client myClient = new Client();

            if (DataSource.ClientList.Exists(x => x.ID == myID))
            {
                myClient = DataSource.ClientList.Find(x => x.ID == myID);

            }
            else
            {
                throw new ClientException("There is not Client with such ID");
            }
            return (myClient).Longitude;
        }
        #endregion

        #region AddFromBLDroneCharging
        /// <summary>
        /// Add a drone charge from bl
        /// </summary>
        /// <param name="DroneID"></param>
        /// <param name="StationID"></param>
        public void AddFromBLDroneCharging(int DroneID, int StationID)
        {
            DroneCharge DC = new DroneCharge();
            DC.DroneId = DroneID;
            DC.StationId = StationID;
            AddDroneCharge(DC);


        }
        #endregion

        #region AddParcelFromBL
        /// <summary>
        /// Add a parcel from bl
        /// </summary>
        /// <param name="p"></param>
        public void AddParcelFromBL(Parcel p)
        {
            Parcel myParcel = DataSource.ParcelList.Find(x => x.ID == p.ID);
            DataSource.ParcelList.Remove(myParcel);
            //AddParcel(p);
            DataSource.ParcelList.Add(p);
        }
        #endregion

        #region IdStation
        /// <summary>
        /// Returns list of station'ids
        /// </summary>
        /// <returns></returns>
        public List<int> IdStation()
        {
            List<int> IdStation = new List<int>();
            int sid = 0;
            foreach (var Stat in IEStationList())
            {
                sid = Stat.ID;
                IdStation.Add(sid);
            }
            return IdStation;

        }
        #endregion

        #region ClientReceivedParcel
        /// <summary>
        /// Returns list of id of the clients that have received 
        /// </summary>
        /// <returns></returns>
        public List<int> clientReceivedParcel() // return list of id of the clients that have received 
        {
            List<int> list = new List<int>();
            //list.Add(DataSource.ParcelList.Find(x=> x.Delivered != DateTime.MinValue),x.id)
            foreach (var item in IEParcelList())
            {
                if (item.Delivered != DateTime.MinValue)
                    list.Add(item.TargetId);
            }
            return list;
        }

        #endregion
    }


}