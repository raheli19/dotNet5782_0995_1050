using System;
using System.Collections.Generic;
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
        internal static List<IDAL.DO.DroneCharge> DroneChargesList;
        static Random rand = new Random();

        public static int ID { get; private set; }
        public static int DroneId { get; private set; }

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
            for(int i=0; i<numClient; i++)
            {
                ClientList.Add(new Client()
                {
                    ID = rand.Next(1000000, 10000000),
                    Name = $"Client {Config.NumOfClients}",
                    Phone= $"0{rand.Next(50, 58)} - {rand.Next(1000000, 10000000)}",
                    //Latitude
                    //longitude

                });
            }
        }
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
        public static void InitializeDrone(int numDrone = 5)
        {

            for (int i = 0; i < numDrone; i++)
            {
                DroneList.Add(new Drone()
                { ID = rand.Next(1000000, 10000000),
                    Model = $"Nebula {i}",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3),
                    Battery = rand.Next(0, 101),

                });
            }
        }
        public static void InitializeStations (int NumStations=2)
        {

                for (int i = 0; i < NumStations; i++)
                {
                    StationList.Add(new Station()
                    { ID = rand.Next(1000000, 10000000),
                    Name = rand.Next(1000000,10000000),
                    Longitude = GetrandomCoordinate(26.2),
                    Latitude = GetrandomCoordinate(25.4),
                    ChargeSlots = rand.Next(0, 101),
                });
                }
        }
        public static void InitializeParcel(int numParcel=10)
        {

            for (int i = 0; i < numParcel; i++)
            {
                ParcelList.Add(new Parcel()
                {
                    ID = rand.Next(1000000, 1000000),
                    SenderId = rand.Next(1000000, 1000000),
                    TargetId = rand.Next(1000000, 1000000),
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                });
            }

        }
        public static void Initialize()// intialising global function
        {
            InitializeClient();
            InitializeDrone();
            InitializeParcel();
            InitializeStations();
            
        }
    }
  
 
    public class DalObject
    {
        DalObject() {DataSource.Initialize();}//constructor

        //functions ADD
        public static void addDrone(Drone drone)// add a new drone to the dronelist
        {
            DataSource.DroneList.Add(drone);
        }
        public static void addClient (Client client)
        {
            DataSource.ClientList.Add(client);
        }
        public static void addStation(Station station)
        {
            DataSource.StationList.Add(station);
        }
        public static void addParcel(Parcel parcel)
        {
            DataSource.ParcelList.Add(parcel);
        }
        public static void addDroneCharge(DroneCharge dc)
        {
            DataSource.DroneChargesList.Add(dc);
        }
        public static void AddParcelToDrone(Parcel parcel) // associate a parcel to a drone
        {
            // passer sur la liste
            //cherceh un free
            // si y en a pas, regarde ceux qui sont chargés?????????
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
        public static void GetParcelByDrone(int parcelId,int droneId)
        {
            IDAL.DO.Parcel p= new IDAL.DO.Parcel();
            IDAL.DO.Drone d= new IDAL.DO.Drone();
           foreach( var item in DataSource.ParcelList)//search in the list of Parcels where the ID we received is
            {
                if(item.ID==parcelId)
                {
                   p=item;//faire un operateur =
                    DataSource.ParcelList.Remove(item);//deletes the current item from the list, and we'll add the modified one
                }
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
        public static void DeliveryToClient(int parcelId)//deliver a package to a customer
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
            p.DroneId = 123456;
            d.Status=DroneStatuses.free; // the drone is free


        }
        
        public static void DroneToCharge(int droneId,int stationId)
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
        public static void DroneCharged(int droneId, int stationId)
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

        //functions PRINT(one entity)
        public static void PrintStation(int stationId)
        {
            for(int i=0;i<DataSource.StationList.Count;i++)
            {
                if(DataSource.StationList[i].ID==stationId)
                    {DataSource.StationList[i].ToString();
                    break;}
            }
        }
        public static void PrintDrone(int droneId)
        {
            for(int i=0; i<DataSource.DroneList.Count; i++)
            {
                if(DataSource.DroneList[i].ID==droneId)
                { DataSource.DroneList[i].ToString();
                break;
                }

            }
        }
        public static void PrintClient(int clientId)
        {
            for(int i=0;i<DataSource.ClientList.Count;i++)
            {
                if(DataSource.ClientList[i].ID==clientId)
                    {
                  DataSource.ClientList[i].ToString();
                    break;
                    }

            }
                    
        }
        public static void PrintParcel(int ParcelId)
        {
            for(int i = 0; i < DataSource.ParcelList.Count; i++)
            {
                if (DataSource.ParcelList[i].ID == ParcelId)
                {
                    DataSource.ParcelList[i].ToString();
                    break;
                }
            }

        }

        //functions PRINT(entire lists)
        public static void PrintStationList ()
        {
            for(int i=0; i< DataSource.StationList.Count; i++)
            {
               PrintStation( DataSource.StationList[i].ID);
            }
        }
        public static void PrintDroneList()
        {
            for(int i=0; i< DataSource.DroneList.Count; i++)
            {
                PrintDrone(DataSource.DroneList[i].ID);
            }
        }
        public static void PrintClientList()
        {
            for(int i=0; i<DataSource.ClientList.Count; i++)
            {
                PrintClient(DataSource.ClientList[i].ID);
            }
        }
        public static void PrintParcelList()
        {
            for(int i=0; i<DataSource.ParcelList.Count; i++)
            {
                PrintParcel(DataSource.ParcelList[i].ID);
            }
        }

    }

 }