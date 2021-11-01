using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        //creation of all the list
        internal static List<IDAL.DO.Client> ClientList = new List<Client>(); 
        internal static List<IDAL.DO.Drone> DroneList = new List<Drone>();
        internal static List<IDAL.DO.Parcel> ParcelList = new List<Parcel>();
        internal static List<IDAL.DO.Station> StationList = new List<Station>();
        internal static List<IDAL.DO.DroneCharge> DroneChargesList = new List<DroneCharge>();
        static Random rand = new Random();

        public static int ID { get; private set; }
        public static int DroneId { get; private set; }

        /// <summary>
        /// The function return random coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Double GetrandomCoordinate(double x)
        {
            return x + rand.NextDouble() /10;
        }

        /// <summary>
        /// Continue static number
        /// </summary>
        internal class Config
        {
            internal static bool statut;/// 1 if free, 0 if not
            internal static WeightCategories light;
            internal static WeightCategories middle;
            internal static WeightCategories heavy;
            internal static int charging; // chargement % per hour
  
        }
        static Random r = new Random();

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
                    ID = rand.Next(1000000, 10000000),
                    Name = $"Client {Config.NumOfClients}",
                    Phone = $"0{rand.Next(50, 58)} - {rand.Next(1000000, 10000000)}",
                    Latitude = DataSource.GetrandomCoordinate(31.37),
                    Longitude = DataSource.GetrandomCoordinate(35.16),

                }) ;
            }
          }
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

        /// <summary>
        /// function initializing a drone
        /// </summary>
        /// <param name="numDrone"></param>
        public static void InitializeDrone(int numDrone = 5)
        {
            for (int i = 0; i < numDrone; i++)
            {
                DroneList.Add(new Drone()
                { 
                    ID = rand.Next(1000000, 10000000),
                    Model = $"Nebula {i}",
                    //MaxWeight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                    Battery = rand.Next(0, 101),

                });
            }
        }

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
                    ID = Config.ParceId++,
                    SenderId = rand.Next(1000000, 1000000),
                    TargetId = rand.Next(1000000, 1000000),
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                }) ;
            }
        }


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
  
 
    public class DalObject :IDAL.IDal
        {
        public DalObject() {DataSource.Initialize();}//constructor

        //functions ADD
        public void AddDrone(Drone drone)// add a new drone to the dronelist
        {
       
            DataSource.DroneList.Add(drone);
        }
         public void addClient (Client client)
        {
            DataSource.ClientList.Add(client);
        }
        public void addStation(Station station)
        {
            DataSource.StationList.Add(station);
        }
        public void addParcel(Parcel parcel)
        {
            DataSource.ParcelList.Add(parcel);
        }
        public void addDroneCharge(DroneCharge dc)
        {
            DataSource.DroneChargesList.Add(dc);
        }

        /// <summary>
        /// Function which associate a parcel to a drone by its id
        /// </summary>
        /// <param name="parcel"></param>
        public void AddParcelToDrone(Parcel parcel) // associate a parcel to a drone
        {
            int droneId=0;
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            foreach( var item in DataSource.DroneList)
            {
                if(item.Status== DroneStatuses.free)
                {
                    d = item;
                    droneId= item.ID;// save the id of the drone
                    DataSource.DroneList.Remove(item);
                    break;
                }
            }
            d.Status = DroneStatuses.shipping; // the drone is shipping
            addDrone(d);
            parcel.DroneId= droneId;// assigns the id of the drone to the parcel
        }

        //functions UPDATE

        /// <summary>
        /// Function which assigns a parcelto a drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void Assignement(int parcelId,int droneId)
        {
            IDAL.DO.Parcel p= new IDAL.DO.Parcel();
            IDAL.DO.Drone d= new IDAL.DO.Drone();
            bool flag= false;
           foreach( var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
           {
                if(item.ID==parcelId)
                {
                   p=item;
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                    flag = true; // we found it
                    break;
                }
           }
           if(flag==false)
            {
                throw new ParcelException("parcel not found")
            }
              
           foreach( var item in DataSource.DroneList)//search in the list of Drones where the ID we received is
            {
                if(item.ID==droneId)
                {
                   d=item;
                  DataSource.DroneList.Remove(item);// deletes the current item from the list, and we'll add the modified one
                    break;
                }
            }
           d.Status=DroneStatuses.shipping;
           p.DroneId=d.ID;
           p.Requested=DateTime.Now;
            // add the modified items into their lists
            addDrone(d);
            addParcel(p);
      
        }

        /// <summary>
        /// To pick a parcel contained in a drone and update their statuses
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void IsPickedUp(int parcelId, int droneId)
        {
            IDAL.DO.Parcel p = new IDAL.DO.Parcel();
            foreach (var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
            {
                if (item.ID == parcelId)
                {
                    p = item;
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                }
            }
            p.PickedUp = DateTime.Now;// the parcel is picked up, and in delivering
        }

        /// <summary>
        /// To deliver a package to a customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void DeliveredToClient(int parcelId)//deliver a package to a customer
        {
            IDAL.DO.Parcel p= new IDAL.DO.Parcel();
            IDAL.DO.Drone d= new IDAL.DO.Drone();
            foreach( var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
            {
                if(item.ID==parcelId)
                {
                    p=item;// save the current item
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                    break;
                }
            }
            foreach( var item in DataSource.DroneList)//search in the list of Drones where the ID we received is
            {
                if(item.ID==p.DroneId)
                {
                    d=item;
                    DataSource.DroneList.Remove(item);// remove it from the list
                    break;
                }
            }
            p.Delivered=DateTime.Now;// time of delivering
            p.DroneId = 000000;
            d.Status=DroneStatuses.free; // the drone is free
        }
        
        /// <summary>
        /// Lead a drone to a station of chargement
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneToCharge(int droneId,int stationId)
        {
            IDAL.DO.Drone d= new IDAL.DO.Drone();
            for( int i=0; i<DataSource.DroneList.Count; i++)
            {
                if(DataSource.DroneList[i].ID==droneId)
                {
                    d = DataSource.DroneList[i];
                    DataSource.DroneList.Remove(DataSource.DroneList[i]);
                    break;
                }
            }
            d.Status = DroneStatuses.maintenance;// plug in the drone to charge
            addDrone(d);// add it back to the list
            IDAL.DO.Station s = new IDAL.DO.Station();
            for(int i=0; i<DataSource.StationList.Count; i++)
            {
                 if(DataSource.StationList[i].ID==stationId)
                 {
                    s = DataSource.StationList[i];
                    DataSource.StationList.Remove(DataSource.StationList[i]);
                    break;
                 }
            }
            s.ChargeSlots--;// a slot if occupied by the new drone
            addStation(s);
            IDAL.DO.DroneCharge DC= new IDAL.DO.DroneCharge();
            DC.DroneId=droneId;
            DC.StationId=stationId;
            addDroneCharge(DC);// add the linked thing to the list
           
        }

        /// <summary>
        /// The drone has finished charging. We need to let it go
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneCharged(int droneId, int stationId)
        {
            foreach(var item in DataSource.DroneChargesList)
            {
                if(item.DroneId==droneId && item.StationId==stationId)
                {
                    DataSource.DroneChargesList.Remove(item);// delete the item; the drone is not charging anymore
                }
            }
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            for (int i=0; i<DataSource.DroneList.Count; i++)
            {
                if(DataSource.DroneList[i].ID==droneId)
                {
                     d= DataSource.DroneList[i];// the drone is charged; he's free for shipping
                    DataSource.DroneList.Remove(DataSource.DroneList[i]);
                    break;
                }
                d.Status = DroneStatuses.free;
                addDrone(d);
            }
            IDAL.DO.Station s = new IDAL.DO.Station();
            for (int i = 0; i < DataSource.StationList.Count; i++)
            {
                if (DataSource.StationList[i].ID == stationId)
                {
                    s = DataSource.StationList[i];
                    DataSource.StationList.Remove(DataSource.StationList[i]);
                    break;
                }
            }
            s.ChargeSlots++;// one is free from the charged drone
            addStation(s);
        }

        /// <summary>
        /// Receives an id and returns the station which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Station StationById(int id)
        {
            for (int i = 0; i < DataSource.StationList.Count; i++)
            {
                if (DataSource.StationList[i].ID == id)
                {
                    return DataSource.StationList[i];
                }
            }
            return new Station();
        }

        /// <summary>
        /// Receives an id and returns the drone which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Drone DroneById(int id)
        {
            for (int i = 0; i < DataSource.DroneList.Count; i++)
            {
                if (DataSource.DroneList[i].ID == id)
                {
                    return DataSource.DroneList[i];
                }
            }
            return new Drone();
        }

        /// <summary>
        /// Receives an id and returns the client which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client ClientById(int id)
        {
            for (int i = 0; i < DataSource.ClientList.Count; i++)
            {
                if (DataSource.ClientList[i].ID == id)
                {
                    return DataSource.ClientList[i];
                }
            }
            return new Client();
        }

        /// <summary>
        /// Receives an id and returns the parcel which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parcel ParcelById(int id)
        {
            for (int i = 0; i < DataSource.ParcelList.Count; i++)
            {
                if (DataSource.ParcelList[i].ID == id)
                {
                    return DataSource.ParcelList[i];
                }
            }
            return new Parcel();
        }

        /// <summary>
        /// Returns the stations' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> StationList()
        {
            List<Station> StationList = new List<Station>();
            return StationList;
        }

        /// <summary>
        /// Returns the drones' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DroneList()
        {
            List<Drone> DroneList = new List<Drone>();
            return DroneList;
        }

        /// <summary>
        /// Returns the clients' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable <Client> ClientList()
        {
            List<Client> ClientList = new List<Client>();
            return ClientList;
        }

        /// <summary>
        /// Returns the parcels' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable <Parcel> ParcelList()
        {
            List<Parcel> ParcelList = new List<Parcel>();
            return ParcelList;
        }
    }
}