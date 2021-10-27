using System;
using IDAL.DO;
using DAL;
using DalObject;

namespace ConsoleUI
{
    class Program
    {
         enum option { EXIT, ADD, UPDATE, PRINT,PRINT_LIST,};
        enum entities { EXIT, STATION, DRONE, CLIENT, PARCEL, DRONECHARGES };
        enum update { EXIT, ASSIGNEMENT, PICKEDUP, DELIVERED,CHARGING, CHARGED,};

        public static void Main()
        {
            new DalObject.DalObject();
            option options;
            entities entity;
            Console.WriteLine("Welcome to our Rahel and Tania Drone delivery System!\n");
            Console.WriteLine("To add an entity please press 1;\nTo update an entity please press 2;\nTo print an entity please press 3;\nTo print a list please press 4;\nTo exit please press 0:");
            options = (option)int.Parse(Console.ReadLine());
            do
            {
               
                switch (options)
                {
                    case option.ADD:
                            Console.WriteLine("You chose to add an entity.");
                            Console.WriteLine("To add a Station please press 1;\nTo add a Drone please press 2;\nTo add a Client please press 3;\nTo add a Parcel please press 4;\nTo exit please press 0:");
                            entity = (entities)int.Parse(Console.ReadLine());
                            switch (entity)
                            {
                            case entities.EXIT:
                                options = 0;
                                break;
                            case entities.STATION:
                                IDAL.DO.Station myStation = new IDAL.DO.Station();
                                int myId;
                                int myName;
                                Console.WriteLine("You chose to add a Station.\nPlease enter its Id, Name, Longitude, Latitude and ChargeSlots:");
                                myId = int.Parse(Console.ReadLine());
                                myName = int.Parse(Console.ReadLine());
                                double.TryParse(Console.ReadLine(), out double myLongitude);
                                double.TryParse(Console.ReadLine(), out double myLatitude);
                                int.TryParse(Console.ReadLine(), out int myCs);
                                myStation.ID = myId;
                                myStation.Name = myName;
                                myStation.Longitude = myLongitude;
                                myStation.ChargeSlots = myCs;
                                DalObject.DalObject.addStation(myStation);
                                break;

                            case entities.DRONE:
                                IDAL.DO.Drone myDrone = new IDAL.DO.Drone();
                                Console.WriteLine("You chose to add a Drone.\nPlease enter its ID, Model, MaxWeight, Status, Battery:");
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
                                Console.WriteLine("You chose to add a Parcel.\nPlease enter its ID, SenderId, TargetId, MaxWeight, Priority, DroneId:");
                                int.TryParse(Console.ReadLine(), out int PID);// ParcelID
                                int.TryParse(Console.ReadLine(), out int senderId);
                                int.TryParse(Console.ReadLine(), out int targetId);
                                myParcel.Weight = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());
                                myParcel.Priority = (IDAL.DO.Priorities)int.Parse(Console.ReadLine());
                                int.TryParse(Console.ReadLine(), out int droneId);
                                myParcel.ID = PID;
                                myParcel.SenderId = senderId;
                                myParcel.TargetId = targetId;
                                myParcel.DroneId = droneId;
                                myParcel.Requested = DateTime.Now; //A verifier
                                DalObject.DalObject.addParcel(myParcel);
                                break;

                            case entities.CLIENT:
                                IDAL.DO.Client client = new Client();
                                Console.WriteLine("You chose to add a Client.\nPlease enter his ID, Name, Phone, and his location: latitude and longitude:\n");
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
                            }
                        break;
                    case option.UPDATE:
                        Console.WriteLine("You chose to update an entity.\nTo assign a Parcel to a Drone please press 1;\nTo pick up a parcel please press 2;\nTo plug in a Drone please press 3;\nTo remove a Drone from the ChargeSlot please press 4,\nTo exit please press 0:\n ");
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
                        Console.WriteLine("You chose to print an entity.");
                        Console.WriteLine("To print a Station please press 1;\nTo print a Drone please press 2;\nTo print a Client please press 3;\nTo print a Parcel please press 4;\nTo exit please press 0:\n");
                        entity = (entities)int.Parse(Console.ReadLine());
                        switch (entity)
                        {
                            case entities.STATION:
                                Console.WriteLine("You chose to print a Station, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int StationId);
                                Console.WriteLine(DalObject.DalObject.StationById(StationId)) ;// calls the function to print the asked station
                                break;

                            case entities.DRONE:
                                Console.WriteLine("You chose to print a Drone, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int DroneId);
                                Console.WriteLine(DalObject.DalObject.DroneById(DroneId));// calls the function to print the asked drone
                                break;

                            case entities.PARCEL:
                                Console.WriteLine("You chose to print a Parcel, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int Pid);
                                Console.WriteLine(DalObject.DalObject.ParcelById(Pid));// calls the function to print the asked drone
                                break;

                            case entities.CLIENT:
                                Console.WriteLine("You chose to print a Client, please enter its Id:\n");
                                int.TryParse(Console.ReadLine(), out int ClientId);
                                Console.WriteLine(DalObject.DalObject.ClientById(ClientId));// calls the function to print the asked drone
                                break;
                           
                        }

                        break;
                    case option.PRINT_LIST:
                        Console.WriteLine("You chose to print an entity's list.\n");
                        Console.WriteLine("To print a Station's list please press 1;\nTo print a Drone's list please press 2;\nTo print a Client's list please press 3;\nTo print a Parcel's list please press 4;\nTo print a DroneCharge's list please press 5;\nTo exit please press 0:\n");
                        entity = (entities)int.Parse(Console.ReadLine());
                        switch (entity)
                        {
                            case entities.STATION:
                                Console.WriteLine("Stations' List:\n");
                                foreach (var item in DalObject.DalObject.StationList() )
                                {
                                    Console.WriteLine(item);
                                    Console.WriteLine(" ");
                                }
                                break;
                            case entities.DRONE:
                                Console.WriteLine("Drones' List:\n");
                                foreach (var item in DalObject.DalObject.DroneList())
                                {
                                    Console.WriteLine(item);
                                    Console.WriteLine(" ");
                                }
                                break;
                            case entities.PARCEL:
                                Console.WriteLine("Parcels' List:\n");
                                foreach (var item in DalObject.DalObject.ParcelList())
                                {
                                    Console.WriteLine(item);
                                    Console.WriteLine(" ");
                                }
                                break;
                            case entities.CLIENT:
                                Console.WriteLine("Clients' List:\n");
                                foreach (var item in DalObject.DalObject.ClientList())
                                {
                                    Console.WriteLine(item);
                                    Console.WriteLine(" ");
                                }
                                break;
                           
                        }
                        break;
                };
                Console.WriteLine("To add an entity please press 1\nTo update an entity please press 2\nTo print an entity please press 3\nTo print a list please press 4\nTo exit please press 0\n");
                options = (option)int.Parse(Console.ReadLine());
            } while (options != 0);
        }
    }
}
