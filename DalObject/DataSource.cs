using System;
using System.Collections.Generic;
using DO;


namespace Dal
{
    public static class DataSource

    {
        static Random rand = new Random();

        //creation of all the list
        public static List<Client> ClientList = new();
        public static List<Drone> DroneChargeList = new List<Drone>();
        public static List<Parcel> ParcelList = new List<Parcel>();
        public static List<Station> StationList = new List<Station>();
        public static List<DroneCharge> DroneChargesList = new List<DroneCharge>();

        static DataSource() { Initialize(); } //ctor which initialize all the lists

        #region Random
        /// <summary>
        /// The function return random coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Double GetrandomCoordinate(double x)
        {
            return x + rand.NextDouble() / 10;
        }
        static Random r = new Random();
        #endregion

        public static int stationNum;


        #region Initializing
        #region InitializeClient
        /// <summary>
        /// function initializing a client
        /// </summary>
        /// <param name="numClient"></param>
        public static void InitializeClient(int numClient = 10)
        {
            for (int i = 0; i < numClient; i++)
            {
                ClientList.Add(new Client()
                {
                    ID = rand.Next(10000000, 100000000),
                    Name = $"Client {i}",
                    Phone = $"0{rand.Next(50, 58)}{rand.Next(1000000, 10000000)}",
                    Latitude = GetrandomCoordinate(31.37),
                    Longitude = GetrandomCoordinate(35.16),

                });
            }
        }
        #endregion

        #region InitializeDroneChargesList
        /// <summary>
        /// function initializing a droneCharge
        /// </summary>
        /// <param name="num"></param>
        public static void InitializeDroneChargesList(int num = 1)
        {
            for (int i = 0; i < num; i++)
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
                    weight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                });
            }
        }
        #endregion
        

        #region InitializeStations
        /// <summary>
        /// function initializing a station
        /// </summary>
        /// <param name="NumStations"></param>
        public static void InitializeStations(int NumStations = 2)
        {
            for (int i = 0; i < NumStations; i++)
            {
                StationList.Add(new Station()
                {
                    ID = rand.Next(1000000, 10000000),
                    Name = $"Bahnhof {stationNum++}",
                    Longitude = GetrandomCoordinate(26.2),
                    Latitude = GetrandomCoordinate(25.4),
                    ChargeSlots = rand.Next(1, 10),
                });
            }
        }
        #endregion

        #region InitializeParcel
        /// <summary>
        /// function initializing a parcel
        /// </summary>
        /// <param name="numParcel"></param>
        public static void InitializeParcel(int numParcel = 10)
        {
            for (int i = 0; i < numParcel; i++)
            {
                ParcelList.Add(new Parcel()
                {
                    ID = Configuration.RunnerIDnumber++,
                    SenderId = ClientList[rand.Next(0, 10)].ID,
                    TargetId = ClientList[rand.Next(0, 10)].ID,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                });
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
}