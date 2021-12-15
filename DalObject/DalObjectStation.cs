using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using Dal;
using DalApi;

namespace Dal
{
    sealed partial class DalObject : IDal
    {
     
        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------


        #region AddStation
        public void AddStation(Station s)
        {

            if (DataSource.StationList.Exists(station => station.ID == s.ID))
            {
                throw new StationException($"id {s.ID} already exists!!");
            }

            DataSource.StationList.Add(s);
        }
        #endregion


        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

        #region UPDATING
        #region UpdateStation
        public void UpdateStation(Station stationToUpdate)
        {
            Station myStation = new Station();
            myStation.ID = -1;
            myStation = DataSource.StationList.Find(x => x.ID == stationToUpdate.ID);

            if (myStation.ID == -1)
            {
                throw new StationException("This station doesn't exists in the system.");
            }
            DataSource.StationList.Remove(myStation);
            myStation.ID = stationToUpdate.ID;
            myStation.Name = stationToUpdate.Name;
            myStation.Longitude = stationToUpdate.Longitude;
            myStation.Latitude = stationToUpdate.Latitude;
            myStation.ChargeSlots = stationToUpdate.ChargeSlots;
            DataSource.StationList.Add(myStation);

        }
        #endregion

        #endregion

        //-----------------------------------ACTIONS-------------------------------------------


        #region StationById
        /// <summary>
        /// Receives an id and returns the station which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Station StationById(int id)
        {
            Station sToReturn = new Station();
            if (!DataSource.StationList.Exists(station => station.ID == id))
            {
                throw new StationException($"id {id} doesn't exist!!");

            };
            sToReturn = DataSource.StationList.Find(s => s.ID == id);
            return sToReturn;
        }
        #endregion
        #region IENUMERABLE
        #region StationList
        /// <summary>
        /// Returns the stations' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Station> IEStationList(Func<Station, bool> predicate = null)
        {
            List<Station> StationLst = new List<Station>();

            if (predicate == null)
            {
                StationLst = DataSource.StationList;
                return StationLst;
            }
            return DataSource.StationList.Where(predicate).ToList();
        }
        #endregion



        #endregion

    }


}