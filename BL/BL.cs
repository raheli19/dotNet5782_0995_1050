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
        static Random rand = new Random();

        readonly IDAL.IDal p;
        help h = null;
       
        List<DroneDescription> DroneList = new List<DroneDescription>();

        public BL() 
        { 
            p= new DalObject.DalObject();
            //List <Drone> Dlist =new List<Drone> (p.DroneList().CopyPropertiesToNew(typeof(IBL.BO)));
            IBL.BO.DroneDescription dr=new IBL.BO.DroneDescription();
            IBL.BO.ParcelToClient ParcelInClient = new IBL.BO.ParcelToClient();
           

            foreach (var item in p.DroneList())
            {
                dr.Id = item.ID;
                dr.Model = item.Model;
                dr.battery = item.Battery;
                
                foreach (var elmtParcel in p.ParcelList())
                {
                    if (elmtParcel.DroneId == item.ID)
                    {
                        ParcelInClient.ID = elmtParcel.ID;
                        ParcelInClient.weight = (WeightCategories)elmtParcel.Weight;
                        ParcelInClient.priority = (Priorities)elmtParcel.Priority;
                        if (elmtParcel.Requested != DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.requested;
                        if(elmtParcel.Scheduled!= DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.scheduled;
                        if (elmtParcel.PickedUp!= DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.pickedup;
                        if (elmtParcel.Delivered != DateTime.MinValue)
                            ParcelInClient.Status = ParcelStatus.delivered;
                        
                    }
                }


                if (ParcelInClient.Status != ParcelStatus.delivered)// the parcel has not been delivered yet.
                {
                    dr.Status = DroneStatuses.shipping;
                    if (ParcelInClient.Status == ParcelStatus.scheduled && ParcelInClient.Status != ParcelStatus.pickedup)
                    {// la station la plus proche

                    }
                }
                else if (ParcelInClient.Status == ParcelStatus.delivered || ParcelInClient.Status == ParcelStatus.scheduled)// the drone isn't shipping
                {
                    int random = (rand.Next(0, 1))*2;
                    if (random == 0)
                    {
                        dr.Status = DroneStatuses.free;
                    }
                    if (random == 2)
                    {
                        dr.Status = DroneStatuses.shipping;
                    }
                }
                if(dr.Status==DroneStatuses.maintenance)// if the drone is charging
                {
                    // random localisation entre les differentes stations
                    
                    
                    dr.battery = h.getRandomNumber(0, 20);

                }
                else if (dr.Status==DroneStatuses.free)
                 {
                        // localisation entre des clients qui ont deja recu des colis
                        // batterie min pour faire une livraisonx
                 }
                dr.weight = ParcelInClient.weight;
                dr.Status = (DroneStatuses)ParcelInClient.Status;
                
                //dr.loc.latitude
                //dr.loc.longitude
                
                

                //DroneList.Add(dr);
                
            }

        }


        public Client GetClient(int id)
        {
            Client c = default;
            try
            { 
                IDAL.DO.Client dalClient = p.ClientById(id);
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


        //-----------------------------------ADD-FUNCTIONS----------------------------------------
        
        public void addStation(IBL.BO.Station s)   
        {
            //IDAL.DO.Station stat = new IDAL.DO.Station();
            //stat.Name = s.Name;
            if (!(s.ID <= 99999999 && s.ID > 9999999))
                throw new IDException("ID not valid");
            //stat.ID = s.ID;
            if (s.loc.latitude< 31||s.loc.latitude>33.3)/////////////// a verifier
                throw new LatException("latitude is not valid");
            //stat.Latitude = s.loc.latitude;
            if (s.loc.longitude<34.3||s.loc.longitude>35.5)
                throw new LongException("longitude is not valid");
            //stat.Longitude = s.loc.longitude;
            try
            {
                p.addStation((IDAL.DO.Station)s.CopyPropertiesToNew(typeof(IDAL.DO.Station)));
            }
            catch(StationException ex)
            {
                throw new StationException("Station already exists ", ex);
            }
            
        }

        public  void addDrone(Drone d, int StationID) 
        {
            d.Battery = rand.Next(20, 40);// initialize the battery
            //Station s = StationById(StationID);// find the station
                
            Station s = GetStation(StationID);
            //IDAL.DO.Drone dr = new IDAL.DO.Drone();
            if (!(d.ID <= 99999999 && d.ID > 9999999)) //ID not Valid
                throw new IDException("ID is not valid");
            //dr.ID = d.ID;
            if (d.MaxWeight != WeightCategories.low && d.MaxWeight != WeightCategories.middle && d.MaxWeight != WeightCategories.heavy)
                throw new WeightException("The category of your weight is not valid");
            if (d.Status != DroneStatuses.free && d.Status != DroneStatuses.maintenance && d.Status != DroneStatuses.shipping)
                throw new StatusException("The status of your drone is not valid");
                //dr.Model = d.Model;
            if (d.Battery < 0 || d.Battery > 100)
                throw new BatException("Battery is not valid");
            //dr.Battery = d.Battery;
            
            d.initialLoc = s.loc;// his location is the same than the station
            s.DroneCharging.Add(d);// add the drone to the station's list (BL)
            s.ChargeSlots--;// one more is full
            try
            {
                p.AddDrone((IDAL.DO.Drone)d.CopyPropertiesToNew(typeof(IDAL.DO.Drone)));
            } 
            catch(Exception ex)
            {
                throw new DroneException("Can't add this drone", ex);
            }
        }

        public void addClient(Client c) 
        {
            //IDAL.DO.Client cl= new IDAL.DO.Client();
            if (!(c.ID <= 99999999 && c.ID > 9999999))
                throw new IDException("ID not valid");
            //cl.ID = c.ID;
            //cl.Name = c.Name;
            if (c.Phone.Length!=11)
                throw new PhoneException("Phone not valid");
            //cl.Phone = c.Phone;
            if (c.ClientLoc.latitude < 31 || c.ClientLoc.latitude > 33.3)/////////////// a verifier
                throw new LatException("latitude is not valid");
            //cl.Latitude = c.ClientLoc.latitude;
            if (c.ClientLoc.longitude < 34.3 || c.ClientLoc.longitude > 35.5)
                throw new LatException("longitude is not valid");

            try
            {
                p.addClient((IDAL.DO.Client)c.CopyPropertiesToNew(typeof(IDAL.DO.Client)));
            }
            catch (ClientException ex)
            {
                throw new ClientException("This Client already exists", ex);
            }
            
           
        }

        public void addParcel(Parcel pack) 
        {

            if (!(pack.ID <= 99999999 && pack.ID > 9999999))
                throw new IDException("ID not valid");
           
            if (!(pack.Sender.ID <= 99999999 && pack.Sender.ID > 9999999))
                throw new IDException("SenderID not valid");
    
            if (!(pack.Target.ID <= 99999999 && pack.Target.ID > 9999999))
                throw new IDException("TargetID not valid");
            
            pack.Scheduled = DateTime.MinValue;
            pack.PickedUp = DateTime.MinValue;
            pack.Delivered = DateTime.MinValue;
            pack.Requested = DateTime.Now;
            pack.Drone = null;
            if (pack.Weight != WeightCategories.low && pack.Weight != WeightCategories.middle && pack.Weight != WeightCategories.heavy)
                throw new WeightException("The category of your weight is not valid");
            if (pack.Priority != Priorities.regular && pack.Priority != Priorities.fast && pack.Priority != Priorities.emergency)
                throw new PriorityException("The time delivery is not valid");
            try
            {
                p.addParcel((IDAL.DO.Parcel)pack.CopyPropertiesToNew(typeof(IDAL.DO.Parcel)));
            }
            catch(Exception ex)
            {
                throw new ParcelException("This package already exists", ex);
            }
        
        }


        //-----------------------------------UPDATE-FUNCTIONS----------------------------------------

        public void updateDrone(int Id, string newModel) 
        {
            //foreach va chercher dans la liste selon le id
            Drone d = GetDrone(Id);
            
        
        
        }

        public void updateStation(int Id, int newName = -1, int newCS = -1)
        {
            Station s = GetStation(Id);
            if (newName != -1)
            {
                s.Name = newName;
            }

            if (newCS != -1)
            {
                s.ChargeSlots = newCS;
            }
            
            //Enlever de la list puis le remettre

        }
        public void updateClient(int Id, string newName = " ", string newTel = " ") 
        {
            Client c = GetClient(Id);
            if(newName!=" ")
            {
                c.Name = newName;
            }
        
        }

        
        public void DroneToCharge(int DroneId) { }

        public void DroneCharged(int DroneId, double timeInCharge) { }

        public void Assignement(int DroneId) { }

        public void PickedUp(int DroneId) { }

        public void delivered(int DroneId) { }


        //-----------------------------------PRINT-FUNCTIONS----------------------------------------

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
