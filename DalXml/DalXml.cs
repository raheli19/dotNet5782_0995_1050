﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    internal class DalXml : IDal
    {
        #region singleton
        /// <summary>
        /// Allows us to create each type an single object
        /// </summary>
        private string configPath;
        private string clientPath;
        private string dronePath;
        private string droneChargePath;
        private string parcelPath;
        private string stationPath;

        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        #endregion

        XElement DroneChargeRoot;
        static DalXml() { }

        #region CTOR
        /// <summary>
        /// Ctor of the xml
        /// </summary>
        public DalXml()
        {
            string str = Assembly.GetExecutingAssembly().Location;
            string localPath = Path.GetDirectoryName(str);
            for (int i = 0; i < 2; i++)
                localPath = Path.GetDirectoryName(localPath);

            localPath += @"\Data";

            configPath = localPath + @"\config.xml";
            clientPath = localPath + @"\customer.xml"; //XMLSerializer
            dronePath = localPath + @"\drone.xml";//XMLSerializer
            droneChargePath = localPath + @"\droneCharge.xml";//XElement
            parcelPath = localPath + @"\parcel.xml";//XMLSerializer
            stationPath = localPath + @"\station.xml";//XMLSerializer


            if (!File.Exists(droneChargePath))
                CreateDroneChargeFile();
            else
                LoadDroneChargeData();


        }

        #endregion

        #region CreateFiles
        /// <summary>
        /// Creates the xelement file
        /// </summary>

        private void CreateDroneChargeFile()
        {
            DroneChargeRoot = new XElement("DroneCharges");
            DroneChargeRoot.Save(droneChargePath);
        }



        #endregion

        #region LoadData
        /// <summary>
        /// Load the xelement data
        /// </summary>
        private void LoadDroneChargeData()
        {
            try
            {
                DroneChargeRoot = XElement.Load(droneChargePath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
        #endregion
        
        #region DroneCharge
        /// <summary>
        /// Function that add a drone to charge
        /// </summary>
        /// <param name="droneChargeToAdd"></param>
        public void AddDroneCharge(DroneCharge droneChargeToAdd) //XElement
        {
            XElement droneId = new XElement("DroneId", droneChargeToAdd.DroneId);
            XElement stationId = new XElement("StationId", droneChargeToAdd.StationId);
            XElement droneCharge = new XElement("DroneCharge", droneId, stationId);

            DroneChargeRoot.Add(droneCharge);
            DroneChargeRoot.Save(droneChargePath);
        }

        /// <summary>
        /// returns the list of all the drone that are charging
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> IEDroneChargeList()
        {
            //List<DroneCharge> listOfAllDronesCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            //return listOfAllDronesCharge;

            LoadDroneChargeData();
            List<DroneCharge> listDroneCharge;
            try
            {
                listDroneCharge = (from p in DroneChargeRoot.Elements()
                                   select new DroneCharge()
                                   {
                                      DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                                       StationId = Convert.ToInt32(p.Element("StationId").Value)

                                   }).ToList();

            }
            catch
            {
                listDroneCharge = null;
            }
            return listDroneCharge;
        }


        #endregion

        #region Drone
        /// <summary>
        /// Add a new drone
        /// </summary>
        /// <param name="droneToAdd"></param>
        public void AddDrone(Drone droneToAdd) //XMLSerializer
        {
            List<DO.Drone> listOfDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            if (!listOfDrones.Exists(x => x.ID == droneToAdd.ID))
            {
                listOfDrones.Add(droneToAdd);
            }
            else
            {
                throw new DroneException("DL: Drone with the same id already exists...");
            }

            XMLTools.SaveListToXMLSerializer<Drone>(listOfDrones, dronePath);
        }

        /// <summary>
        /// returns the drone that we asked by sending its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Drone DroneById(int id)
        {
            var listOfDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            int index = listOfDrones.FindIndex(x => x.ID == id);
            if (index == -1)
            {
                throw new Exception("DAL: Drone with the same id not found...");
            }
            return listOfDrones.FirstOrDefault(x => x.ID == id);
        }

        /// <summary>
        /// update the drone // name
        /// </summary>
        /// <param name="droneToUpdate"></param>
        public void UpdateDrone(Drone droneToUpdate)
        {
            var listOfdrones = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone myDrone = new();
            if (listOfdrones.Exists(t => t.ID == droneToUpdate.ID))
            {
                myDrone = listOfdrones.Find(x => x.ID == droneToUpdate.ID);
                listOfdrones.Remove(myDrone);
            }
            else
                throw new DroneException("DAL: Drone with the same id not found...");

            myDrone.ID = droneToUpdate.ID;
            myDrone.Model = droneToUpdate.Model;
            myDrone.weight = droneToUpdate.weight;
            listOfdrones.Add(myDrone);
            XMLTools.SaveListToXMLSerializer<DO.Drone>(listOfdrones, dronePath);
        }

        /// <summary>
        /// returns list of drones in the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> IEDroneList()
        {
            {
                List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
                return listOfAllDrones;
            }
        }

        #endregion

        #region Client
        /// <summary>
        /// add a new client in the system
        /// </summary>
        /// <param name="clientToAdd"></param>
        public void AddClient(Client clientToAdd)
        {
            List<DO.Client> listOfClients = XMLTools.LoadListFromXMLSerializer<Client>(clientPath);

            if (!listOfClients.Exists(x => x.ID == clientToAdd.ID))
            {
                listOfClients.Add(clientToAdd);
            }
            else
            {
                throw new ClientException("DL: Client with the same id already exists...");
            }
            XMLTools.SaveListToXMLSerializer<Client>(listOfClients, clientPath);
        }

        /// <summary>
        /// returns the client that we asked by sending its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Client ClientById(int id)
        {
            List<DO.Client> ListOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            int index = ListOfClients.FindIndex(x => x.ID == id);
            if (index == -1)
                throw new Exception("DAL: Client with the same id not found...");
            return ListOfClients.FirstOrDefault(x => x.ID == id);
        }

        /// <summary>
        /// update the client's details
        /// </summary>
        /// <param name="clientToUpdate"></param>
        public void UpdateClient(Client clientToUpdate)
        {
            var listOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            Client myClient = new();

            int index = listOfClients.FindIndex(t => t.ID == clientToUpdate.ID);

            if (index == -1)
                throw new Exception("DAL: Client with the same id not found...");
            listOfClients.RemoveAt(index);
            myClient.ID = clientToUpdate.ID;
            myClient.Name = clientToUpdate.Name;
            myClient.Longitude = clientToUpdate.Longitude;
            myClient.Latitude = clientToUpdate.Latitude;
            myClient.Phone = clientToUpdate.Phone;
            listOfClients.Add(myClient);

            XMLTools.SaveListToXMLSerializer<DO.Client>(listOfClients, clientPath);
        }

        /// <summary>
        /// returns list of all the clients of the systems
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> IEClientList()
        {
            List<Client> listOfAllClients = XMLTools.LoadListFromXMLSerializer<Client>(clientPath);
            return listOfAllClients;
        }

        #endregion

        #region Parcel
        /// <summary>
        /// add a new parcel
        /// </summary>
        /// <param name="parcelToAdd"></param>
        public void AddParcel(Parcel parcelToAdd)
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            if (!listOfParcels.Exists(x => x.ID == parcelToAdd.ID))
            {
                listOfParcels.Add(parcelToAdd);
            }
            else
            {
                throw new DroneException("DL: Drone with the same id already exists...");
            }
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfParcels, parcelPath);
        }

        /// <summary>
        /// update the details of the parcel
        /// </summary>
        /// <param name="ParcelToUpdate"></param>
        public void UpdateParcelFromBL(Parcel ParcelToUpdate)
        {
            var listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel myParcel = new();

            int index = listOfParcels.FindIndex(t => t.ID == ParcelToUpdate.ID);

            if (index == -1)
                throw new ParcelException("DAL: Parcel with the same id not found...");


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

            XMLTools.SaveListToXMLSerializer<DO.Parcel>(listOfParcels, parcelPath);
        }

        /// <summary>
        /// returns the parcel asked by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Parcel ParcelById(int id)
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            int index = listOfParcels.FindIndex(x => x.ID == id);
            if (index == -1)
            {
                throw new Exception("DAL: Parcel with the same id not found...");
            }
            return listOfParcels.FirstOrDefault(x => x.ID == id);


        }

        /// <summary>
        /// returns the list of all the parcels of the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> IEParcelList()
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            return listOfAllParcels;
        }

        /// <summary>
        /// delete a parcel - only if it has been delivered
        /// </summary>
        /// <param name="parcelToDelete"></param>
        public void RemoveParcel(Parcel parcelToDelete)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (parcelToDelete.Delivered == DateTime.MinValue)
                throw new Exception("Can not delete the parcel, it has not been delivered yet! ");
            Parcel temp = ParcelById(parcelToDelete.ID); // checks if the parcel is in the list
            listOfAllParcels.Remove(parcelToDelete);
            XMLTools.SaveListToXMLSerializer(listOfAllParcels, parcelPath);
        }


        ////HELPFUNCTION
        /// <summary>
        /// update a parcel
        /// </summary>
        /// <param name="parcelToChange"></param>
        public void AddParcelFromBL(Parcel parcelToChange)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            Parcel myParcel = ParcelById(parcelToChange.ID);
            listOfAllParcels.Remove(myParcel);
            listOfAllParcels.Add(parcelToChange);
            XMLTools.SaveListToXMLSerializer(listOfAllParcels, parcelPath);
        }

        /// <summary>
        /// returns the parcel attached to the drone 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Parcel FindParcelAssociatedWithDrone(int droneId)
        {
            List<Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            int index = listOfParcels.FindIndex(x => x.DroneId == droneId);
            if (index == -1)
            {
                throw new ParcelException("There is no parcel which contains this droneID!");
            }
            return listOfParcels.FirstOrDefault(x => x.ID == droneId);

        }
        #endregion

        #region Station
        /// <summary>
        /// add a station to the system
        /// </summary>
        /// <param name="stationToAdd"></param>
        public void AddStation(Station stationToAdd)
        {
            List<DO.Station> listOfStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);

            if (!listOfStations.Exists(x => x.ID == stationToAdd.ID))
            {
                listOfStations.Add(stationToAdd);
            }
            else
            {
                throw new StationException("DL: Station with the same id already exists...");
            }
            XMLTools.SaveListToXMLSerializer<Station>(listOfStations, stationPath);
        }

        /// <summary>
        /// returns the station that we received its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO.Station StationById(int id)
        {
            List<DO.Station> listOfStation = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            int index = listOfStation.FindIndex(x => x.ID == id);
            if (index == -1)
                throw new Exception("DAL: Student with the same id not found...");
            return listOfStation.FirstOrDefault(x => x.ID == id);

        }

        /// <summary>
        /// update a station
        /// </summary>
        /// <param name="stationToUpdate"></param>
        public void UpdateStation(Station stationToUpdate)
        {
            var listOfStations = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            Station myStation = new();
                                                                                            
            int index = listOfStations.FindIndex(t => t.ID == stationToUpdate.ID);

            if (index == -1)
                throw new Exception("DAL: Station with the same id not found...");
            listOfStations.RemoveAt(index);
            myStation.ID = stationToUpdate.ID;
            myStation.Name = stationToUpdate.Name;
            myStation.Longitude = stationToUpdate.Longitude;
            myStation.Latitude = stationToUpdate.Latitude;
            myStation.ChargeSlots = stationToUpdate.ChargeSlots;
            listOfStations.Add(myStation);

            XMLTools.SaveListToXMLSerializer<DO.Station>(listOfStations, stationPath);
        }

        /// <summary>
        /// returns list of all the stations of the system
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Station> IEStationList(Func<Station, bool> predicate = null)
        {
            List<DO.Station> listOfStations = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            return listOfStations;
        }

        #endregion

        #region Help
        /// <summary>
        /// function that returns the latitude of the client
        /// </summary>
        /// <param name="myId"></param>
        /// <returns></returns>
        public double FindLat(int myId)
        {
            List<DO.Client> ListOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            Client clientLat = ClientById(myId);
            return clientLat.Latitude;

        }
        /// <summary>
        /// function that returns the longitude of the client
        /// </summary>
        /// <param name="myId"></param>
        /// <returns></returns>
        public double FindLong(int myId)
        {
            List<DO.Client> ListOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            Client clientLat = ClientById(myId);
            return clientLat.Longitude;

        }

        /// <summary>
        /// returns list of the stations'id 
        /// </summary>
        /// <returns></returns>
        public List<int> IdStation()
        {
            var listOfStations = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            List<int> listOfId = new List<int>();
            int sid = 0;
            foreach (var item in listOfStations)
            {
                sid = item.ID;
                listOfId.Add(sid);
            }
            return listOfId;

        }
         /// <summary>
         /// list of the parcels the client received
         /// </summary>
         /// <returns></returns>
        public List<int> clientReceivedParcel()
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);

            List<int> listId = new List<int>();
            foreach (var item in listOfParcels)
            {
                if (item.Delivered != DateTime.MinValue)
                    listId.Add(item.TargetId);
            }
            return listId;
        }

        /// <summary>
        /// returns the id of a parcel
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="TargetId"></param>
        /// <returns></returns>
        public int DalGetIdParcel(int senderId, int TargetId)
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);

            int id = listOfParcels.Find(x => x.SenderId == senderId && x.TargetId == TargetId).ID;
            return id;
        }

        /// <summary>
        /// plug in a drone to a station
        /// </summary>
        /// <param name="DroneID"></param>
        /// <param name="StationID"></param>
        public void AddFromBLDroneCharging(int DroneID, int StationID)
        {

            XElement droneId = new XElement("droneId", DroneID);
            XElement stationId = new XElement("stationId", StationID);

            XElement droneCharge = new XElement("DroneCharge", droneId, stationId);
            DroneChargeRoot.Add(droneCharge);
            DroneChargeRoot.Save(droneChargePath);
        }

        /// <summary>
        /// update the list of the dronescharge
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
            DroneChargeRoot.Elements().Where(el => el.Element("DroneId").Value == droneId.ToString() && el.Element("StationId").Value == statId.ToString()).Remove();
        }
        #endregion

        #region DalObjFunctions
        /// <summary>
        /// use of the battery
        /// </summary>
        /// <returns></returns>
        public double[] ElectricityUse()
        {
            var config = XMLTools.LoadListFromXMLSerializer<string>(configPath);
            double[] arr = new double[5];

            arr[0] = double.Parse(config[1], CultureInfo.InvariantCulture);
            arr[1] = double.Parse(config[2], CultureInfo.InvariantCulture);
            arr[2] = double.Parse(config[3], CultureInfo.InvariantCulture);
            arr[3] = double.Parse(config[4], CultureInfo.InvariantCulture);
            arr[4] = double.Parse(config[5], CultureInfo.InvariantCulture);
            return arr;

        }

        /// <summary>
        /// returns the runner number 
        /// </summary>
        /// <returns></returns>
        public int RunnerNumber()
        {
            var runnerNumber = XMLTools.LoadListFromXMLSerializer<string>(configPath);
            int real = Convert.ToInt32(runnerNumber);
            return real;
        }
        #endregion

    }

}
