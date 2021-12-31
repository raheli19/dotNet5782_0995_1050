using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    public class DalXml : IDal
    {
        #region singleton
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        //XElement ClientRoot;
        //XElement DroneRoot;
        XElement DroneChargeRoot;
        //XElement ParcelRoot;
        //XElement StationRoot;
        public DalXml()
        {
            ////client
            //if (!File.Exists(clientPath))
            //    CreateClientFile();
            //else
            //    LoadClientData();

            ////drone
            //if (!File.Exists(dronePath))
            //    CreateDroneFile();
            //else
            //    LoadDroneData();

            //droneCharge
            if (!File.Exists(droneChargePath))
                CreateDroneChargeFile();
            else
                LoadDroneChargeData();

            ////parcel
            //if (!File.Exists(parcelPath))
            //    CreateParcelFile();
            //else
            //    LoadParcelData();

            ////station
            //if (!File.Exists(stationPath))
            //    CreateStationFile();
            //else
            //    LoadStationData();
        }

        #region CreateFiles

        //private void CreateClientFile()
        //{
        //    ClientRoot = new XElement("Clients");
        //    ClientRoot.Save(clientPath);
        //}

        //private void CreateStationFile()
        //{
        //    StationRoot = new XElement("Stations");
        //    StationRoot.Save(stationPath);
        //}

        //private void CreateParcelFile()
        //{
        //    ParcelRoot = new XElement("Parcels");
        //    ParcelRoot.Save(parcelPath);
        //}

        private void CreateDroneChargeFile()
        {
            DroneChargeRoot = new XElement("DroneCharges");
            DroneChargeRoot.Save(droneChargePath);
        }

        //private void CreateDroneFile()
        //{
        //    DroneRoot = new XElement("Drones");
        //    DroneRoot.Save(dronePath);
        //}






        #endregion

        #region LoadData

        //private void LoadStationData()
        //{
        //    try
        //    {
        //        StationRoot = XElement.Load(stationPath);
        //    }
        //    catch
        //    {
        //        throw new Exception("File upload problem");
        //    }
        //}

        //private void LoadParcelData()
        //{

        //    try
        //    {
        //        ParcelRoot = XElement.Load(parcelPath);
        //    }
        //    catch
        //    {
        //        throw new Exception("File upload problem");
        //    }
        //}

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

        //private void LoadDroneData()
        //{
        //    try
        //    {
        //        DroneRoot = XElement.Load(dronePath);
        //    }
        //    catch
        //    {
        //        throw new Exception("File upload problem");
        //    }
        //}

        //private void LoadClientData()
        //{
        //    try
        //    {
        //        ClientRoot = XElement.Load(clientPath);
        //    }
        //    catch
        //    {
        //        throw new Exception("File upload problem");
        //    }
        //}
        #endregion



        static DalXml() { }
        #endregion

        string clientPath = @"ClientXml.xml"; //XMLSerializer
        string dronePath = @"DroneXml.xml";//XMLSerializer
        string droneChargePath = @"DroneChargeXml.xml";//XElement
        string parcelPath = @"ParcelXml.xml";//XMLSerializer
        string stationPath = @"StationXml.xml";//XMLSerializer
        //string configurationPath = @"configurationXml.xml";
        //user

        
        #region DroneCharge
        public void AddDroneCharge(DroneCharge droneChargeToAdd) //XElement
        {
            XElement droneId = new XElement("DroneId", droneChargeToAdd.DroneId);
            XElement stationId = new XElement("StationId", droneChargeToAdd.StationId);
            XElement droneCharge = new XElement("DroneCharge", droneId, stationId);

            DroneChargeRoot.Add(droneCharge);
            DroneChargeRoot.Save(droneChargePath);
        }

        public IEnumerable<DroneCharge> IEDroneChargeList()
        {
            List<DroneCharge> listOfAllDronesCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            return listOfAllDronesCharge;

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
        public void AddDrone(Drone droneToAdd) //XMLSerializer
        {
            List<DO.Drone> listOfDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            if (!listOfDrones.Exists(x => x.ID == droneToAdd.ID))
            {
                listOfDrones.Add(droneToAdd);
            }
            else
            {
                throw new AlreadyExistsException("DL: Drone with the same id already exists...");
            }

            XMLTools.SaveListToXMLSerializer<Drone>(listOfDrones, dronePath);
        }


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

        public void UpdateDrone(Drone droneToUpdate)
        {
            var listOfdrones = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone myDrone = new();
            if (listOfdrones.Exists(t => t.ID == droneToUpdate.ID))
            {
                myDrone = listOfdrones.Find(x => x.ID == droneToUpdate.ID);
            }
            else
                throw new Exception("DAL: Drone with the same id not found...");

            myDrone.ID = droneToUpdate.ID;
            myDrone.Model = droneToUpdate.Model;
            myDrone.weight = droneToUpdate.weight;
            XMLTools.SaveListToXMLSerializer<DO.Drone>(dronesList, dronePath);
        }

        public IEnumerable<Drone> IEDroneList()
        {
            {
                List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
                return listOfAllDrones;
            }
        }

        #endregion

        #region Client
        public void AddClient(Client clientToAdd)
        {
            List<DO.Client> listOfClients = XMLTools.LoadListFromXMLSerializer<Client>(clientPath);

            if (!listOfClients.Exists(x => x.ID == clientToAdd.ID))
            {
                listOfClients.Add(clientToAdd);
            }
            else
            {
                throw new AlreadyExistsException("DL: Client with the same id already exists...");
            }
        }

        public DO.Client ClientById(int id)
        {
            List<DO.Client> ListOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            int index = ListOfClients.FindIndex(x => x.ID == id);
            if (index == -1)
                throw new Exception("DAL: Client with the same id not found...");
            return ListOfClients.FirstOrDefault(x => x.ID == id);
        }

        public void UpdateClient(Client clientToUpdate)
        {
            var listOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            Client myClient = new();

            int index = listOfClients.FindIndex(t => t.ID == clientToUpdate.ID);

            if (index == -1)
                throw new Exception("DAL: Client with the same id not found...");

            myClient.ID = clientToUpdate.ID;
            myClient.Name = clientToUpdate.Name;
            myClient.Longitude = clientToUpdate.Longitude;
            myClient.Latitude = clientToUpdate.Latitude;
            myClient.Phone = clientToUpdate.Phone;


            XMLTools.SaveListToXMLSerializer<DO.Client>(listOfClients, clientPath);
        }

        public IEnumerable<Client> IEClientList()
        {
            List<Client> listOfAllClients = XMLTools.LoadListFromXMLSerializer<Client>(clientPath);
            return listOfAllClients;
        }

        #endregion

        #region Parcel
        public void AddParcel(Parcel parcelToAdd)
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            if (!listOfParcels.Exists(x => x.ID == parcelToAdd.ID))
            {
                listOfParcels.Add(parcelToAdd);
            }
            else
            {
                throw new AlreadyExistsException("DL: Drone with the same id already exists...");
            }
        }

        public void UpdateParcelFromBL(Parcel ParcelToUpdate)
        {
            var listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel myParcel = new();

            int index = listOfParcels.FindIndex(t => t.ID == ParcelToUpdate.ID);

            if (index == -1)
                throw new Exception("DAL: Parcel with the same id not found...");


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

        public IEnumerable<Parcel> IEParcelList()
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            return listOfAllParcels;
        }

        public void RemoveParcel(Parcel parcelToDelete)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (parcelToDelete.Delivered == DateTime.MinValue)
                throw new Exception("Can not delete the parcel, it has not been delivered yet! ");
            Parcel temp =ParcelById(parcelToDelete.ID); // checks if the parcel is in the list
            listOfAllParcels.Remove(parcelToDelete);
            XMLTools.SaveListToXMLSerializer(listOfAllParcels, parcelPath);
        }


        ////HELPFUNCTION
        public void AddParcelFromBL(Parcel parcelToChange)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            Parcel myParcel = ParcelById(parcelToChange.ID);
            listOfAllParcels.Remove(myParcel);
            listOfAllParcels.Add(parcelToChange);
            XMLTools.SaveListToXMLSerializer(listOfAllParcels, parcelPath);
        }

        public void FindParcelAssociatedWithDrone(int droneId)
        {
            List<Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            int index = listOfParcels.FindIndex(x => x.DroneId == droneId);
            if (index == -1)
            {
                throw new ParcelException("There is no parcel which contains this droneID!");
            }
            return listOfParcels.FirstOrDefault(x => x.ID == id);

        }
        #endregion

        #region Station
        public void AddStation(Station stationToAdd)
        {
            List<DO.Station> listOfStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);

            if (!listOfStations.Exists(x => x.ID == stationToAdd.ID))
            {
                listOfStations.Add(stationToAdd);
            }
            else
            {
                throw new AlreadyExistsException("DL: Station with the same id already exists...");
            }
        }

        public DO.Station StationById(int id)
        {
            List<DO.Station> listOfStation = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            int index = listOfStation.FindIndex(x => x.ID == id);
            if (index == -1)
                throw new Exception("DAL: Student with the same id not found...");
            return listOfStation.FirstOrDefault(x => x.ID == id);

        }

        public void UpdateStation(Station stationToUpdate)
        {
            var listOfStations = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            Station myStation = new();

            int index = listOfStations.FindIndex(t => t.ID == stationToUpdate.ID);

            if (index == -1)
                throw new Exception("DAL: Station with the same id not found...");

            myStation.ID = stationToUpdate.ID;
            myStation.Name = stationToUpdate.Name;
            myStation.Longitude = stationToUpdate.Longitude;
            myStation.Latitude = stationToUpdate.Latitude;
            myStation.ChargeSlots = stationToUpdate.ChargeSlots;


            XMLTools.SaveListToXMLSerializer<DO.Station>(listOfStations, stationPath);
        }

        public IEnumerable<Station> IEStationList(Func<Station, bool> predicate = null)
        {
            List<DO.Station> listOfStations = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            var v = from item in listOfStations
                    select item; //item.Clone();
            if (predicate == null)
                return v.AsEnumerable().OrderByDescending(x => x.ID);
            return v.Where(predicate).OrderByDescending(x => x.ID);
        }

        #endregion

        #region Help
        public double FindLat(int myId)
        {
            List<DO.Client> ListOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            Client clientLat = ClientById(myId);
            return clientLat.Latitude;

        }

        public double FindLong(int myId)
        {
            List<DO.Client> ListOfClients = XMLTools.LoadListFromXMLSerializer<DO.Client>(clientPath);
            Client clientLat = ClientById(myId);
            return clientLat.Longitude;

        }

        public List<int> IdStation()
        {
            var listOfStations = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            List<int> listOfId = new List<int>();
            int sid = 0;
            foreach(var item in listOfStations)
            {
                sid = item.ID;
                listOfId.Add(sid);
            }
            return listOfId;

        }

        public List<int> clientReceivedParcel()
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);

            List<int> listId = new List<int>();
            foreach(var item in listOfParcels)
            {
                if (item.Delivered != DateTime.MinValue)
                    listId.Add(item.TargetId);
            }
            return listId;
        }

        public int DalGetIdParcel(int senderId, int TargetId)
        {
            List<DO.Parcel> listOfParcels = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);

            int id = listOfParcels.Find(x => x.SenderId == senderId && x.TargetId == TargetId).ID;
            return id;
        }

        public void AddFromBLDroneCharging(int DroneID, int StationID)
        {
            
            XElement droneId = new XElement("droneId", DroneID);
            XElement stationId = new XElement("stationId", StationID);

            XElement droneCharge = new XElement("DroneCharge", droneId, stationId);
            DroneChargeRoot.Add(droneCharge);
            DroneChargeRoot.Save(droneChargePath);
        }

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

















    }

}
