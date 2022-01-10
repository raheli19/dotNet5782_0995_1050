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


        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

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


        //-----------------------------------GET DRONE AND DRONE LIST-----------------------------------------------------

 
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

       



    }
}