using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;





namespace BL
{
    public class BL:IBL.IBL
    {
        
        IDAL.IDal d = IDAL.DalObject();
          

        public Drone DroneById(int id)
        {
            Drone? temp = null;
            foreach (Drone d in DroneList)
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
        public Station StationById(int id)
        {
            Station? temp = null;
            foreach (Station s in StationList)
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
        public Client ClientById(int id)
        {
            Client? temp = null;
            foreach (Client c in ClientList)
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
        public Parcel ParcelById(int id)
        {
            Parcel? temp = null;
            foreach (Parcel p in ParcelList)
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

        
        public BL() { }
        


        static Random rand = new Random();
        //functions ADD

        public void addStation(Station s)   
        {
            List<Station> StationList = DAL.DalObject.DalObject.StationList();
            StationList.Add(s);
        }


       public  void addDrone(Drone d, int StationID) 
        {
            d.Battery = rand.Next(20, 40);// initialize the battery
            Station s = StationById(StationID);// find the station
            d.initialLoc = s.loc;// the drone's location is the station's one
            s.DroneCharging.Add(d);// add the drone to the station's list
            DroneList.Add(d);// add the drone to the dronelist
        }

        public void addClient(Client c) 
        {
            ClientList.Add(c);
        }

        public void addParcel(Parcel p) 
        {

        

        }

        //functions UPDATE

        public void updateDrone(int Id, string newName) { }

        public void updateStation(int Id, int newName = -1, int newCS = -1) { }

        public void updateClient(int Id, string newName = " ", string newTel = " ") { }

        public void DroneToCharge(int DroneId) { }

        public void DroneCharged(int DroneId, double timeInCharge) { }

        public void Assignement(int DroneId) { }

        public void PickedUp(int DroneId) { }

        public void delivered(int DroneId) { }


        //functions print

        public void printStation();
        public void printDrone();
        public void printClient();
        public void printParcel();
        public void printStationList();
        public void printDroneList();
        public void printClientList();
        public void printParcelList();
        public void printParcelsNotAssigned();
        public void printFreeStations();

    }
}
