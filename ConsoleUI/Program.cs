using System;
using IDAL.DO;
using DAL;
using DalObject;
namespace ConsoleUI
{
    class Program
    {
         enum option { EXIT=0 , ADD, UPDATE, PRINT,PRINT_LIST,};
        enum entities { EXIT=0, STATION, DRONE, CLIENT, PARCEL, DRONECHARGES };
        enum update { EXIT, ASSIGNEMENT, PICKEDUP, DELIVERED,CHARGING, CHARGED,};

        public static void Main(string[] args)
        {
            option options;
            entities entity;
            Console.WriteLine("Welcome to our Nebula Drone delivery System!\n");
            Console.WriteLine("To add an entity please press 1\nTo update an entity please press 2\nTo print an entity please press 3\nTo print a list please press 4\nTo exit please press 0\n");
            options= (option)int.Parse(Console.ReadLine());
            do
            {
                switch (options)
                {
                    case option.ADD:
                            Console.WriteLine("You chose to add an entity.\n ");
                            Console.WriteLine("To add a Station please press 1,\nTo add a drone please press 2,\nTo add a Client please press 3,\nTo add a Parcel please press 4\n To exit please press 0\n");
                            entity = (entities)int.Parse(Console.ReadLine());
                            switch (entity)
                            {
                            case entities.STATION:
                                IDAL.DO.Station myStation = new IDAL.DO.Station();
                                int myId;
                                int myName;
                                Console.WriteLine("You chose to add a Station,\n Please enter its Id, name, longitude, latitude:\n");
                                myId = int.Parse(Console.ReadLine());
                                myName = int.Parse(Console.ReadLine());
                                double.TryParse(Console.ReadLine(), out double myLongitude);
                                double.TryParse(Console.ReadLine(), out double myLatitude);
                                myStation.ID = myId;
                                myStation.Name = myName;
                                myStation.Longitude = myLongitude;
                                DalObject.DalObject.addStation(myStation);
                                break;

                            case entities.DRONE:
                                IDAL.DO.Drone myDrone = new IDAL.DO.Drone();
                                Console.WriteLine("You chose to add a Drone,\n Please enter its ID, Model, MaxWeight, Status, Battery:\n");
                                int.TryParse(Console.ReadLine(), out int DID); //DroneID
                                string myModel;
                                myModel = Console.ReadLine();
                                myDrone.MaxWeight = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());
                                myDrone.Status = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                                double.TryParse(Console.ReadLine(), out double battery);
                                myDrone.ID = DID;
                                myDrone.Model = myModel;
                                myDrone.Battery = battery;
                                DalObject.DalObject.addDrone(myDrone);
                                break;

                            case entities.PARCEL:
                                IDAL.DO.Parcel myParcel = new Parcel();
                                Console.WriteLine("You chose to add a Parcel,\n Please enter its ID, SenderId, TargetId, MaxWeight, Priority, Requested time, DroneId:\n");
                                int.TryParse(Console.ReadLine(), out int PID);// ParcelID
                                int.TryParse(Console.ReadLine(), out int senderId);
                                int.TryParse(Console.ReadLine(), out int targetId);
                                myParcel.Weight = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());//A revoir
                                myParcel.Priority = (IDAL.DO.Priorities)int.Parse(Console.ReadLine());
                                int.TryParse(Console.ReadLine(), out int droneId);
                                myParcel.ID = PID;
                                myParcel.SenderId = senderId;
                                myParcel.TargetId = targetId;
                                //
                                myParcel.DroneId = droneId;
                                //Les 3
                                break;
                            case entities.CLIENT:
                                IDAL.DO.Client client = new Client();
                                Console.WriteLine("You chose to add a Client,\n Please enter his ID, Name, Phone, and his location: latitude and longitude:\n");
                                int.TryParse(Console.ReadLine(), out int CID);//ClientID
                                string Cname, Cphone;
                                Cname = Console.ReadLine();
                                Cphone = Console.ReadLine();
                                double.TryParse(Console.ReadLine(), out double Clatitude);
                                double.TryParse(Console.ReadLine(), out double Clongitude);
                                client.ID = CID;
                                client.Name = Cname;
                                client.Phone = Cphone;
                                client.Latitude = Clatitude;
                                client.Longitude = Clongitude;
                                DalObject.DalObject.addClient(client);// add it to the list
                                break;
                            /*case entities.DRONECHARGES:
                                IDAL.DO.DroneCharge DC = new DroneCharge();
                                Console.WriteLine("You chose to add a DroneCharge,\n Please enter its DroneId and StationId:");
                                int.TryParse(Console.ReadLine(), out int myDroneId);
                                int.TryParse(Console.ReadLine(), out int myStationId);
                                DC.DroneId = myDroneId;
                                DC.StationId = myStationId;
                                DalObject.DalObject.addDroneCharge(DC);
                                break;*/

                            }
                        break;
                    case option.UPDATE:
                        Console.WriteLine("You chose to update an entity;\nTo assign a Parcel to a Drone please press 1,\nTo pick up a parcel please press 2,\n To plug in a Drone please press 3,\nTo remove a Drone from the ChargeSlot please press 4,\nTo exit please press 0.\n ");
                        update updating;
                        updating = (update)int.Parse(Console.ReadLine());
                        switch(updating)
                        {
                            case update.ASSIGNEMENT:
                                Console.WriteLine("You chose to assign a Parcel to a Drone.\nPlease enter the Parcel's Id and Drone's Id:\n");
                                int.TryParse(Console.ReadLine(), out int ParcelID);
                                int.TryParse(Console.ReadLine(), out int DroneID);
                                DalObject.DalObject.Assignement(ParcelID, DroneID);
                                break;
                            case update.PICKEDUP:
                                Console.WriteLine("You chose to pick up a Parcel.\nPlease enter the Parcel's Id and the Drone's Id:\n");
                                int.TryParse(Console.ReadLine(), out int ParcID);
                                int.TryParse(Console.ReadLine(), out int DronID);
                                DalObject.DalObject.IsPickedUp(ParcID, DronID);
                                break;
                            case update.DELIVERED:
                                Console.WriteLine("Your Parcel has arrived!.\nPlease enter the Parcel's Id:\n");
                                int.TryParse(Console.ReadLine(), out int myParcID);
                                DalObject.DalObject.DeliveredToClient(myParcID);
                                break;
                            case update.CHARGING:
                                Console.WriteLine("You want to plug in a Drone.\nPlease enter the Drone's and Station's Id:\n");
                                int.TryParse(Console.ReadLine(), out int myDronID);
                                int.TryParse(Console.ReadLine(), out int myStationID);
                                DalObject.DalObject.DroneToCharge(myDronID, myStationID);
                                break;
                            case update.CHARGED:
                                Console.WriteLine("Your drone has finished charging.\nPlease enter the Drone's and Station Id:\n");
                                int.TryParse(Console.ReadLine(), out int DrID);
                                int.TryParse(Console.ReadLine(), out int statID);
                                DalObject.DalObject.DroneCharged(DrID,statID);
                                break;
                        }


                        break;
                    case option.PRINT:
                        Console.WriteLine("You chose to print an entity.\n");
                        Console.WriteLine("To print a Station please press 1,\nTo print a Drone please press 2,\nTo print a Client please press 3,\nTo print a Parcel please press 4,\nTo exit please press 0\n");
                        entity = (entities)int.Parse(Console.ReadLine());
                        switch (entity)
                        {
                            case entities.STATION:
                                Console.WriteLine("You chose to print a Station, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int StationId);
                                DalObject.DalObject.PrintStation(StationId);// calls the function to print the asked station
                                break;
                            case entities.DRONE:
                                Console.WriteLine("You chose to print a Drone, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int DroneId);
                                DalObject.DalObject.PrintDrone(DroneId);// calls the function to print the asked drone
                                break;
                            case entities.PARCEL:
                                Console.WriteLine("You chose to print a Parcel, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int Pid);
                                DalObject.DalObject.PrintParcel(Pid);// calls the function to print the asked drone
                                break;
                            case entities.CLIENT:
                                Console.WriteLine("You chose to print a Client, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int ClientId);
                                DalObject.DalObject.PrintClient(ClientId);// calls the function to print the asked drone
                                
                                break;
                           
                        }

                        break;
                    case option.PRINT_LIST:
                        Console.WriteLine("You chose to print an entity's list.\n");
                        Console.WriteLine("To print a Station's list please press 1,\nTo print a Drone's list please press 2,\nTo print a Parcel's list please press 3,\nTo print a Client's list please press 4,\nTo print a DroneCharge's list please press 5,\nTo exit please press 0\n");
                        entity = (entities)int.Parse(Console.ReadLine());
                        switch (entity)
                        {
                            case entities.STATION:
                                Console.WriteLine("Stations' List:\n");
                                DalObject.DalObject.PrintStationList();
                                break;
                            case entities.DRONE:
                                Console.WriteLine("Drones' List:\n");
                                DalObject.DalObject.PrintDroneList();
                                break;
                            case entities.PARCEL:
                                Console.WriteLine("Parcels' List:\n");
                                DalObject.DalObject.PrintParcelList();
                                break;
                            case entities.CLIENT:
                                Console.WriteLine("Clients' List:\n");
                                DalObject.DalObject.PrintClientList();
                                break;
                           
                        }
                        break;
                };
            } while (options != 0);
        }
       
    
    }
}
