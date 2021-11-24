/*
 update
region
hagrala sur une liste pour la localisation du drone
*/



using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        //creation of all the list
        internal static List<IDAL.DO.Client> ClientList = new List<Client>(); 
        internal static List<IDAL.DO.Drone> DroneChargeList = new List<Drone>();
        internal static List<IDAL.DO.Parcel> ParcelList = new List<Parcel>();
        internal static List<IDAL.DO.Station> StationList = new List<Station>();
        internal static List<IDAL.DO.DroneCharge> DroneChargesList = new List<DroneCharge>();
        static Random rand = new Random();

        public static int ID { get; private set; }
        public static int DroneId { get; private set; }

        #region Random
        /// <summary>
        /// The function return random coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Double GetrandomCoordinate(double x)
        {
            return x + rand.NextDouble() /10;
        }
        static Random r = new Random();
        #endregion

        #region ClassConfig
        /// <summary>
        /// Continue static number
        /// </summary>
        internal class Config
        {
            //internal static bool statut;/// 1 if free, 0 if not
            //internal static WeightCategories light;
            //internal static WeightCategories middle;
            //internal static WeightCategories heavy;
            //internal static int charging; // chargement % per hour
            public static int RunnerIDnumber = 1000;

            public static double BatteryFree = 0.0005; // % of battery per km.
            public static double BatteryLightWeight = 0.001;
            public static double BatteryMiddleWeight = 0.0015;
            public static double BatteryHeavyWeight = 0.002;
            public static double ChargeDroneRate = 10;


        }
        #endregion
        

        #region Initializing
        #region InitializeClient
        /// <summary>
        /// function initializing a client
        /// </summary>
        /// <param name="numClient"></param>
        public static void InitializeClient(int numClient=10)
        {
            for (int i = 0; i < numClient; i++)
            {
                ClientList.Add(new Client()
                {
                    ID = rand.Next(10000000, 100000000),
                    Name = $"Client {i}",
                    Phone = $"0{rand.Next(50, 58)} - {rand.Next(1000000, 10000000)}",
                    Latitude = DataSource.GetrandomCoordinate(31.37),
                    Longitude = DataSource.GetrandomCoordinate(35.16),

                }) ;
            }
        }
        #endregion

        #region InitializeDroneChargesList
        /// <summary>
        /// function initializing a droneCharge
        /// </summary>
        /// <param name="num"></param>
        public static void InitializeDroneChargesList(int num=1)
        {
            for(int i=0; i<num; i++)
            {
                DroneChargesList.Add(new DroneCharge()
                {
                    DroneId = rand.Next(1000000, 10000000),
                    StationId = rand.Next(1000000, 10000000),
                });
            }
        }
        #endregion

        #region InitializeDrone
        /// <summary>
        /// function initializing a drone
        /// </summary>
        /// <param name="numDrone"></param>
        public static void InitializeDrone(int numDrone = 5)
        {
            for (int i = 0; i < numDrone; i++)
            {
                DroneChargeList.Add(new Drone()
                { 
                    ID = rand.Next(1000000, 10000000),
                    Model = $"Nebula {i}",
                    //MaxWeight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                    Battery = rand.Next(0, 101),

                });
            }
        }
        #endregion

        #region InitializeStations
        /// <summary>
        /// function initializing a station
        /// </summary>
        /// <param name="NumStations"></param>
        public static void InitializeStations (int NumStations=2)
        {
                for (int i = 0; i < NumStations; i++)
                {
                    StationList.Add(new Station()
                    { ID = rand.Next(1000000, 10000000),
                    Name = rand.Next(1000000,10000000),
                    Longitude = GetrandomCoordinate(26.2),
                    Latitude = GetrandomCoordinate(25.4),
                    ChargeSlots = rand.Next(0, 10),
                });
                }
        }
        #endregion

        #region InitializeParcel
        /// <summary>
        /// function initializing a parcel
        /// </summary>
        /// <param name="numParcel"></param>
        public static void InitializeParcel(int numParcel=10)
        {
            for (int i = 0; i < numParcel; i++)
            {
                ParcelList.Add(new Parcel()
                {
                    ID = Config.RunnerIDnumber++,
                    SenderId = ClientList[rand.Next(0,10)].ID,
                    TargetId = ClientList[rand.Next(0, 10)].ID,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                    
                }) ;
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize all the entities
        /// </summary>
        public static void Initialize()// intialising global function
        {
            InitializeClient();
            InitializeDrone();
            InitializeParcel();
            InitializeStations();
        }
    }
    #endregion

    #endregion
    public class DalObject : IDAL.IDal
    {
        public DalObject() { DataSource.Initialize(); }//constructor



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

        #region addClient
        public void addClient(Client c)
        {

            if (DataSource.ClientList.Exists(client => client.ID == c.ID))
            {
                throw new ClientException($"id {c.ID} already exists!!");
            }
            DataSource.ClientList.Add(c);
        }
        #endregion

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

        #region addParcel
        public void addParcel(Parcel pl)
        {
            //if (DataSource.ParcelList.Exists(parcel => parcel.ID == pl.ID))
            //{
            //    throw new ParcelException($"id {pl.ID} already exists!!");
            //}
            DataSource.ParcelList.Add(pl);
            DataSource.Config.RunnerIDnumber++;
        }
        #endregion

        #region addDroneCharge
        public void addDroneCharge(DroneCharge dc)
        {

            if (DataSource.DroneChargesList.Exists(DroneCharge => DroneCharge.DroneId == dc.DroneId))
            {
                throw new DroneException($"id {dc.DroneId} already exists!!");
            }
            DataSource.DroneChargesList.Add(dc);
        }
        #endregion

        #region addParcelToDrone
        /// <summary>
        /// Function which associate a parcel to a drone by its id
        /// </summary>
        /// <param name="parcel"></param>
        public void AddParcelToDrone(Parcel parcel, Drone d) // associate a parcel to a drone
        {
            parcel.DroneId = d.ID;
            UpdateParcel(parcel);
        }



        public void UpdateParcel(Parcel parcel)
        {
            if (!(DataSource.ParcelList.Exists(p => p.ID == parcel.ID)))
            {
                throw new ParcelException($"id {parcel.ID} is not valid!!");
            }
            int index = DataSource.ParcelList.FindIndex(item => item.ID == parcel.ID);
            DataSource.ParcelList[index] = parcel;
        }
        #endregion

        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

        #region UPDATING
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
            myDrone.Battery = droneToUpdate.Battery;
            DataSource.DroneChargeList.Add(myDrone);

        }
        #endregion
        public void updateDroneChargeList(int droneId, int statId)
        {
            foreach (var item in DroneChargeList())
            {
                if (item.DroneId == droneId && item.StationId == statId)
                    DataSource.DroneChargesList.Remove(item);
            }
        }

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

        #region UpdateClient
        public void UpdateClient(Client ClientToUpdate)
        {
            Client myClient = new Client();
            myClient.ID = -1;
            myClient = DataSource.ClientList.Find(x => x.ID == ClientToUpdate.ID);

            if (myClient.ID == -1)
            {
                throw new ClientException("This Client doesn't exist in the system.");

            }
            DataSource.ClientList.Remove(myClient);
            myClient.ID = ClientToUpdate.ID;
            myClient.Name = ClientToUpdate.Name;
            myClient.Phone = ClientToUpdate.Phone;
            myClient.Latitude = ClientToUpdate.Latitude;
            myClient.Longitude = ClientToUpdate.Longitude;
            DataSource.ClientList.Add(myClient);
        }
        #endregion


        #endregion

        //-----------------------------------ACTIONS-------------------------------------------

        #region AssignementFunction
        /// <summary>
        /// Function which assigns a parcelto a drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void Assignement(int parcelId, int droneId)
        {
            IDAL.DO.Parcel p = new IDAL.DO.Parcel();
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            bool flag = false, flag2 = false;
            foreach (var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is not
            {
                if (item.ID == parcelId)
                {
                    p = item;
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                    flag = true; // we found it
                    break;
                }
            }
            if (flag == false)
            {
                throw new ParcelException("parcel not found");
            }

            foreach (var item in DataSource.DroneChargeList)//search in the list of Drones where the ID we received is
            {
                if (item.ID == droneId)
                {
                    d = item;
                    DataSource.DroneChargeList.Remove(item);// deletes the current item from the list, and we'll add the modified one
                    flag2 = true;
                    break;
                }
            }
            if (flag2 == false)
            {
                throw new DroneException("drone not found");
            }
            p.DroneId = d.ID;
            p.Requested = DateTime.Now;
            // add the modified items into their lists
            AddDrone(d);
            addParcel(p);

        }
        #endregion

        #region PickedUpFunction
        /// <summary>
        /// To pick a parcel contained in a drone and update their statuses
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void IsPickedUp(int parcelId, int droneId)
        {
            if (parcelId == -1)
            {
                throw new ParcelException($"id {parcelId} does not exist !!");
            }

            Parcel p = DataSource.ParcelList[parcelId];
            p.DroneId = droneId;
            p.PickedUp = DateTime.Now;
            DataSource.ParcelList[parcelId] = p;

            if (droneId == -1)
            {
                throw new DroneException($"id {droneId} does not exist !!");
            }

            Drone d = DataSource.DroneChargeList[droneId];
            //Update the drone status into delivery

        }
        #endregion

        #region DeliveredFunction
        /// <summary>
        /// To deliver a package to a customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void DeliveredToClient(int parcelId)//deliver a package to a customer
        {
            IDAL.DO.Parcel p = new IDAL.DO.Parcel();
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            bool flag = false, flag2 = false;
            foreach (var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
            {
                if (item.ID == parcelId)
                {
                    flag = true;
                    p = item;// save the current item
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                    break;
                }
            }
            if (flag == false)
            {
                throw new ParcelException("parcel not found");
            }

            foreach (var item in DataSource.DroneChargeList)//search in the list of Drones where the ID we received is
            {
                if (item.ID == p.DroneId)
                {
                    flag2 = true;
                    d = item;
                    DataSource.DroneChargeList.Remove(item);// remove it from the list
                    break;
                }
            }
            if (flag2 == false)
            {
                throw new DroneException("drone not found");
            }
            p.Delivered = DateTime.Now;// time of delivering
            p.DroneId = 000000;
            //d.Status=DroneStatuses.free; // the drone is free
        }
        #endregion

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
            for (int i = 0; i < DataSource.DroneChargeList.Count; i++)
            {
                if (DataSource.DroneChargeList[i].ID == droneId)
                {
                    flag = true;
                    d = DataSource.DroneChargeList[i];
                    DataSource.DroneChargeList.Remove(DataSource.DroneChargeList[i]);
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
            foreach (var item in DataSource.DroneChargesList)
            {
                if (item.DroneId == droneId)
                {
                    flag = true;
                    if (item.StationId == stationId)
                    {
                        flag2 = true;
                        DataSource.DroneChargesList.Remove(item);// delete the item; the drone is not charging anymore
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
            IDAL.DO.Station s = new IDAL.DO.Station();
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
            addStation(s);
        }
        #endregion

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

        #region DroneById
        /// <summary>
        /// Receives an id and returns the drone which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Drone DroneById(int id)
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

        #region ClientById
        /// <summary>
        /// Receives an id and returns the client which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client ClientById(int id)
        {
            Client cToReturn = default;
            if (!DataSource.ClientList.Exists(client => client.ID == id))
            {
                throw new ClientException($"id {id} doesn't exist!!");

            };
            cToReturn = DataSource.ClientList.Find(c => c.ID == id);
            return cToReturn;
        }
        #endregion

        #region ParcelById
        /// <summary>
        /// Receives an id and returns the parcel which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parcel ParcelById(int id)
        {
            Parcel pToReturn = default;
            if (!DataSource.ParcelList.Exists(parcel => parcel.ID == id))
            {
                throw new ParcelException($"id {id} doesn't exist!!");

            };
            pToReturn = DataSource.ParcelList.Find(p => p.ID == id);
            return pToReturn;
        }
        #endregion

        #region IENUMERABLE
        #region StationList
        /// <summary>
        /// Returns the stations' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> StationList()
        {
            List<Station> StationLst = new List<Station>();
            StationLst = DataSource.StationList;
            return StationLst;
        }
        #endregion

        #region DroneList
        /// <summary>
        /// Returns the drones' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DroneList()
        {
            List<Drone> DroneLst = new List<Drone>();
            DroneLst = DataSource.DroneChargeList;
            return DroneLst;
        }
        #endregion

        public IEnumerable<DroneCharge> DroneChargeList()
        {
            List<DroneCharge> DroneChargeLst = new List<DroneCharge>();
            DroneChargeLst = DataSource.DroneChargesList;
            return DroneChargeLst;
        }
        #region ClientList
        /// <summary>
        /// Returns the clients' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> ClientList()
        {
            List<Client> ClientLst = new List<Client>();
            ClientLst = DataSource.ClientList;
            return ClientLst;
        }
        #endregion
        #region ParcelList
        /// <summary>
        /// Returns the parcels' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> ParcelList()
        {
            List<Parcel> ParcelLst = new List<Parcel>();
            ParcelLst = DataSource.ParcelList;
            return ParcelLst;
        }
        #endregion
        #endregion

        //----------------------------HELP---------------------


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

        public void AddFromBLDroneCharging(int DroneID, int StationID)
        {
            DroneCharge DC = new DroneCharge();
            DC.DroneId = DroneID;
            DC.StationId = StationID;
            addDroneCharge(DC);


        }

        public void AddParcelFromBL(Parcel p)
        {
            Parcel myParcel = DataSource.ParcelList.Find(x => x.ID == p.ID);
            DataSource.ParcelList.Remove(myParcel);
            addParcel(p);

        }
        public List<int> IdStation()
        {
            List<int> IdStation = new List<int>();
            int sid = 0;
            foreach (var Stat in StationList())
            {
                sid = Stat.ID;
                IdStation.Add(sid);
            }
            return IdStation;

        }
        public List<int> clientReceivedParcel() // return list of id of the clients that have received 
        {
            List<int> list = new List<int>();
            //list.Add(DataSource.ParcelList.Find(x=> x.Delivered != DateTime.MinValue),x.id)
            foreach(var item in ParcelList())
            {
                if (item.Delivered != DateTime.MinValue)
                    list.Add(item.TargetId);       
            }
            return list;
        }

       
        // trouver la enieme sttion dans la li

    }

    //        public static IEnumerable<T> GetNth<T>(this IList<T> list, int n)
    //    {

    //        from var station in StationList.GetNth(i) select station;
    //        return ;
    //    }

    //}
}