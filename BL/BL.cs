using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DAL;
using DalObject;




namespace BL
{
    
    public class BL: IBL.IBL
    {
        
        readonly IDAL.IDal p = new DalObject.DalObject();
        
        public BL() { }
        
        public Client GetClient(int id)
        {
            Client c = default;
            try
            { IDAL.DO.Client dalClient = p.ClientById(id);
            }
            catch(IDAL.DO.ClientException custEX)
            {
                throw new ClientException($"Client ID {id} was not found", custEX);
            }
            return c;
        }
       public Station GetStation(int id)
        {
            Station s = default;
            try
            {
                IDAL.DO.Station dalStat = p.StationById(id);
            }
            catch (IDAL.DO.StationException statEX)
            {
                throw new StationException($"Station ID {id} was not found", statEX);
            }
            return s;
        }
        public Drone GetDrone(int id)
        {
            Drone d = default;
            try
            {
                IDAL.DO.Drone dalDrone = p.DroneById(id);
            }
            catch (IDAL.DO.DroneException drEX)
            {
                throw new DroneException($"Drone ID {id} was not found", drEX);
            }
            return d;

        }
        static Random rand = new Random();
        //functions ADD

        public void addStation(IBL.BO.Station s)   
        {
            IDAL.DO.Station stat = new IDAL.DO.Station();
            stat.Name = s.Name;
            if (!(s.ID <= 99999999 && s.ID > 9999999))
                throw new IDException("ID not valid");
            stat.ID = s.ID;
            if (s.loc.latitude> 180)/////////////// a verifier
                throw new LatException("latitude is not valid");
            stat.Latitude = s.loc.latitude;
            if (s.loc.longitude>90)
                throw new LongException("longitude is not valid");
            stat.Longitude = s.loc.longitude;
            p.addStation(stat);
            
        }


       public  void addDrone(Drone d, int StationID) 
        {
            d.Battery = rand.Next(20, 40);// initialize the battery
            Station s = StationById(StationID);// find the station
            d.initialLoc = s.loc;// the drone's location is the station's one
            s.DroneCharging.Add(d);// add the drone to the station's list

            IDAL.DO.Drone dr = new IDAL.DO.Drone();
            if (!(d.ID <= 99999999 && d.ID > 9999999))
                throw new IDException("ID is not valid");
            dr.ID = d.ID;
           
            dr.Model = d.Model;
            if (d.Battery < 0 || d.Battery > 100)
                throw new BatException("Battery is not valid");
            dr.Battery = d.Battery;
            p.AddDrone(dr);   
        }

        public void addClient(Client c) 
        {
            IDAL.DO.Client cl= new IDAL.DO.Client();
            if (!(c.ID <= 99999999 && c.ID > 9999999))
                throw new IDException("ID not valid");
            cl.ID = c.ID;

            cl.Name = c.Name;
            if (c.Phone.Length!=11)
                throw new PhoneException("Phone not valid");
            cl.Phone = c.Phone;
            if (c.ClientLoc.latitude > 180)/////////////// a verifier
                throw new LatException("latitude is not valid");
            cl.Latitude = c.ClientLoc.latitude;
            if (c.ClientLoc.longitude > 90)
                throw new LatException("longitude is not valid");
            cl.Longitude = c.ClientLoc.longitude;
            p.addClient(cl);
           
        }

        public void addParcel(Parcel pack) 
        {
            IDAL.DO.Parcel package = new IDAL.DO.Parcel();
            if (!(pack.ID <= 99999999 && pack.ID > 9999999))
                throw new IDException("ID not valid");
            package.ID = pack.ID;
            if (!(pack.Sender.ID <= 99999999 && pack.Sender.ID > 9999999))
                throw new IDException("SenderID not valid");
            package.SenderId = pack.Sender.ID;
            if (!(pack.Target.ID <= 99999999 && pack.Target.ID > 9999999))
                throw new IDException("TargetID not valid");
            package.TargetId = pack.Target.ID;
            package.Weight =(IDAL.DO.WeightCategories) pack.Weight;
            package.Priority = (IDAL.DO.Priorities)pack.Priority;
            package.Scheduled = DateTime.MinValue;////Verifier
            package.PickedUp = DateTime.MinValue;
            package.Delivered = DateTime.MinValue;
            package.Requested = DateTime.Now;
            pack.Drone = null;
            p.addParcel(package);
        }

        //functions UPDATE

        public void updateDrone(int Id, string newModel) 
        {
            //Drone temp = new Drone();
            //Drone d = DroneById(Id);
            //temp = d;
            //temp.Model = newModel;

           IDAL.DO.Drone myDrone = p.DroneById(Id);
            myDrone.Model = newModel;
            p.AddDrone(myDrone);
        
        
        }

        public void updateStation(int Id, int newName = -1, int newCS = -1) { }

        public void updateClient(int Id, string newName = " ", string newTel = " ") { }

        public void DroneToCharge(int DroneId) { }

        public void DroneCharged(int DroneId, double timeInCharge) { }

        public void Assignement(int DroneId) { }

        public void PickedUp(int DroneId) { }

        public void delivered(int DroneId) { }


        //functions print

        public void printStation() { }
        public void printDrone() { }
        public void printClient() { }
        public void printParcel() { }
        public void printStationList() { }
        public void printDroneList() { }
        public void printClientList() { }
        public void printParcelList() { }
        public void printParcelsNotAssigned() { }
        public void printFreeStations() { }

    }
}
