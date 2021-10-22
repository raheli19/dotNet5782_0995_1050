using System;
using IDAL.DO;



namespace DalObject
{
    
    public class DataSource
    {
        //creation of all the list
        internal static List<IDAL.DO.Client> ClientList; 
        internal static List<IDAL.DO.Drone> DroneList;
        internal static List<IDAL.DO.Parcel> ParcelList;
        internal static List<IDAL.DO.Station> StationList;


        static Random rand = new Random();

        public static Double GetrandomCoordinate(double x)
        {
            return x + rand.NextDouble() /10;
        }


        internal class Config
        {
            internal static int NumOfClients = 0;
           static int count=0; // counter for all the products
           

        }

        // functions initializing each one of the lists
        public static void InitializeClient(int numClient=10)
        {
            ClientList = new List<Client>();
            for(int i=0; i<numClient; i++)
            {
              ClientList.Add(new Client())
                {
                    ID= rand.Next(1000000,10000000),
                    Name = $"Client {Config.NumOfClients}",
                    Phone = $"0{rand.Next(50,58)} - {rand.Next(1000000,10000000)}",


                }
                IndexClients++;
            }


        }
        public static void InitializeDrone(int numDrone=5)
        {
            DroneList.Add(new Drone())
                for(int i=0; i<numDrone; i++)
                {
                   ID= rand.Next(1000000,10000000),
                    Model = $"Nebula {i}",
                    MaxWeight = (WeighCategories).rand(3),
                    Status =(DroneStatuses).rand(3),
                    Battery = rand.Next(0,101),
                }
        }
        public static void InitializeStations (int NumStations=2)
        {
            StationList.Add(new Station())
                for(int i=0; i<NumStations; i++)
                {
                    ID= rand.Next(1000000,1000000),
                    Station= $"Bahnhof {i}",
                    Longitude= GetrandomCoordinate(26.2),
                    Latitude = GetrandomCoordinate(25.4),
                    ChargeSlots= rand.Next(0,101),
                }
        }
        public static void InitializeParcel(int numParcel=10)
        {
            ParcelList.Add(new Parcel())
                for(int i=0; i<numParcel; i++)
                {
                    ID=rand.Next(1000000,1000000),
                    SenderId=rand.Next(1000000,1000000),
                    TargetId=rand.Next(1000000,1000000),
                    Weight = (WeightCategories).rand(3),
                    Priority= (Priorities).rand(3),
                    Requested= (datetime)rand(3),
                }

        }

        public static Initialize()// intialising global function
        {
            InitializeClient();
            InitializeDrone();
            InitializeParcel();
            InitializeStations();
            
        }



    }
  

    public class DalObject
    {

    }



}