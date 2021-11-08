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
            bool flag = false;
            foreach( var item in DataSource.DroneList)
            {
                if(item.Status== DroneStatuses.free)
                {
                    d = item;
                    droneId= item.ID;// save the id of the drone
                    flag = true;
                    DataSource.DroneList.Remove(item);
                    break;
                }
            }
            if (flag == false)
            {
                throw new DroneException("drone not found");
            }
            
            AddDrone(d);
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
            bool flag = false, flag2 = false;
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
                throw new ParcelException("parcel not found");
            }
              
           foreach( var item in DataSource.DroneList)//search in the list of Drones where the ID we received is
            {
                if(item.ID==droneId)
                {
                   d=item;
                  DataSource.DroneList.Remove(item);// deletes the current item from the list, and we'll add the modified one
                    flag2 = true;
                    break;
                }
            }
            if (flag2 == false)
            {
                throw new DroneException("drone not found");
            }
           p.DroneId=d.ID;
           p.Requested=DateTime.Now;
            // add the modified items into their lists
            AddDrone(d);
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
            bool flag = false;
            foreach (var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
            {
                if (item.ID == parcelId)
                {
                    p = item;
                    flag = true;
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                }
            }
            if (flag == false)
            {
                throw new ParcelException("parcel not found");
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
            bool flag = false, flag2 = false;
            foreach( var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
            {
                if(item.ID==parcelId)
                {
                    flag = true;
                    p=item;// save the current item
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                    break;
                }
            }
            if (flag == false)
            {
                throw new ParcelException("parcel not found");
            }

            foreach( var item in DataSource.DroneList)//search in the list of Drones where the ID we received is
            {
                if(item.ID==p.DroneId)
                {
                    flag2 = true;
                    d=item;
                    DataSource.DroneList.Remove(item);// remove it from the list
                    break;
                }
            }
            if (flag2 == false)
            {
                throw new DroneException("drone not found");
            }
            p.Delivered=DateTime.Now;// time of delivering
            p.DroneId = 000000;
            //d.Status=DroneStatuses.free; // the drone is free
        }
        
        /// <summary>
        /// Lead a drone to a station of chargement
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneToCharge(int droneId,int stationId)
        {
            IDAL.DO.Drone d= new IDAL.DO.Drone();
            bool flag = false,flag2=false;
            for( int i=0; i<DataSource.DroneList.Count; i++)
            {
                if(DataSource.DroneList[i].ID==droneId)
                {
                    flag = true;
                    d = DataSource.DroneList[i];
                    DataSource.DroneList.Remove(DataSource.DroneList[i]);
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
            for(int i=0; i<DataSource.StationList.Count; i++)
            {
                 if(DataSource.StationList[i].ID==stationId)
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
            bool flag = false, flag2 = false;
            foreach(var item in DataSource.DroneChargesList)
            {
                if (item.DroneId == droneId) {
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
            for (int i=0; i<DataSource.DroneList.Count; i++)
            {
                if(DataSource.DroneList[i].ID==droneId)
                {
                    flag3 = true;
                     d= DataSource.DroneList[i];// the drone is charged; he's free for shipping
                    DataSource.DroneList.Remove(DataSource.DroneList[i]);
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

        /// <summary>
        /// Receives an id and returns the station which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Station StationById(int id)
        {
            Station? temp = null;
            foreach (Station s in DataSource.StationList)
            {
                if (s.ID == id)
                {
                    temp = s;
                    break;
                }
            }
            if (temp == null)
            {
                throw new StationException("station not found");
            }
            return (Station)temp;
        }

        /// <summary>
        /// Receives an id and returns the drone which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Drone DroneById(int id)
        {
            Drone? temp = null;
            foreach (Drone d in DataSource.DroneList)
            {
                if (d.ID == id)
                {
                    temp = d;
                    break;
                }
            }
            if (temp == null)
            {
                throw new DroneException("drone not found");
            }
            return (Drone)temp;
        }

        /// <summary>
        /// Receives an id and returns the client which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client ClientById(int id)
        {
            Client? temp = null;
            foreach (Client c in DataSource.ClientList)
            {
                if (c.ID == id)
                {
                    temp = c;
                    break;
                }
            }
            if (temp == null)
            {
                throw new ClientException("client not found");
            }
            return (Client)temp;
        }

        /// <summary>
        /// Receives an id and returns the parcel which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parcel ParcelById(int id)
        {
            Parcel? temp = null;
            foreach (Parcel p in DataSource.ParcelList)
            {
                if (p.ID == id)
                {
                    temp = p;
                    break;
                }
            }
            if (temp == null)
            {
                throw new ParcelException("parcel not found");
            }
            return (Parcel)temp;
        }

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

        /// <summary>
        /// Returns the drones' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DroneList()
        {
            List<Drone> DroneLst = new List<Drone>();
            DroneLst = DataSource.DroneList;
            return DroneLst;
        }

        /// <summary>
        /// Returns the clients' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable <Client> ClientList()
        {
            List<Client> ClientLst = new List<Client>();
            ClientLst = DataSource.ClientList;
            return ClientLst;
        }

        /// <summary>
        /// Returns the parcels' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable <Parcel> ParcelList()
        {
            List<Parcel> ParcelLst = new List<Parcel>();
            ParcelLst = DataSource.ParcelList;
            return ParcelLst;
        }
    }
}