using System;
using System.Collections.Generic;
using IDAL.DO;


namespace DalObject
{
    public partial class DalObject : IDAL.IDal

    {
        //creation of all the list
        internal static List<IDAL.DO.Client> ClientList = new List<Client>(); 
        internal static List<IDAL.DO.Drone> DroneChargeList = new List<Drone>();
        internal static List<IDAL.DO.Parcel> ParcelList = new List<Parcel>();
        internal static List<IDAL.DO.Station> StationList = new List<Station>();
        internal static List<IDAL.DO.DroneCharge> DroneChargesList = new List<DroneCharge>();
        

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
                    Latitude = GetrandomCoordinate(31.37),
                    Longitude = GetrandomCoordinate(35.16),

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
}