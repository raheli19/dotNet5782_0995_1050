﻿using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
     
        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------


        #region addStation
        public void addStation(Station s)
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
        public Station StationById(int id)
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
        public IEnumerable<Station> IEStationList()
        {
            List<Station> StationLst = new List<Station>();
            StationLst = DataSource.StationList;
            return StationLst;
        }
        #endregion

        #endregion

    }

  
}