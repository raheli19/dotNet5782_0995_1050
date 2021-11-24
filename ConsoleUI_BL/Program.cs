using System;
using IBL.BO;
using BL;

namespace ConsoleUI_BL
{
    class Program
    {

        enum option { EXIT, ADD, UPDATE, PRINT, PRINT_LIST, };
        enum entities { EXIT, STATION, DRONE, CLIENT, PARCEL, DRONECHARGES, };
        enum update { EXIT, DRONE, STAT, CLIENT, CHARGING, CHARGED, ASSIGNEMENT, PICKEDUP, DELIVERED, };

        public static void Main()
        {
            IBL.IBL obj = new BL.BL();
            //try
            //{
                //obj= new BL.BL();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}


            option options;
            entities entity;
            Console.WriteLine("Welcome to our Nebula Drone delivery System!\n");
            Console.WriteLine("To add an entity please press 1;\nTo update an entity please press 2;\nTo print an entity please press 3;\nTo print a list please press 4;\nTo exit please press 0:");
            options = (option)int.Parse(Console.ReadLine());
            do
            {
                try
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
                                    IBL.BO.Station myStation = new IBL.BO.Station();
                                    myStation.Loc = new Localisation();
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
                                    myStation.Loc.longitude = myLongitude;
                                    myStation.Loc.latitude = myLatitude;
                                    myStation.ChargeSlots = myCs;
                                    obj.addStation(myStation);
                                    myStation.DroneCharging = null;
                                    break;

                                case entities.DRONE:
                                    IBL.BO.Drone myDrone = new IBL.BO.Drone();
                                    Console.WriteLine("You chose to add a Drone.\nPlease enter its ID, Model, MaxWeight,Number of station :");
                                    int.TryParse(Console.ReadLine(), out int DID); //DroneID
                                    string myModel;
                                    myModel = Console.ReadLine();
                                    myDrone.MaxWeight = (IBL.BO.WeightCategories)int.Parse(Console.ReadLine());
                                    int.TryParse(Console.ReadLine(), out int NumSt);
                                    myDrone.ID = DID;
                                    myDrone.Model = myModel;
                                    obj.addDrone(myDrone, NumSt);
                                    break;


                                case entities.CLIENT:
                                    IBL.BO.Client client = new Client();
                                    client.ClientLoc = new Localisation();
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
                                    client.ClientLoc.latitude = Clatitude;
                                    client.ClientLoc.longitude = Clongitude;
                                    obj.addClient(client);// add it to the list
                                    break;

                                case entities.PARCEL:
                                    IBL.BO.Parcel myParcel = new Parcel();
                                    myParcel.Sender = new ClientInParcel();
                                    myParcel.Target = new ClientInParcel();
                                    Console.WriteLine("You chose to add a Parcel.\nPlease enter its SenderId, TargetId, MaxWeight, Priority:");
                                    int.TryParse(Console.ReadLine(), out int senderId);
                                    int.TryParse(Console.ReadLine(), out int targetId);
                                    myParcel.Weight = (IBL.BO.WeightCategories)int.Parse(Console.ReadLine());
                                    myParcel.Priority = (IBL.BO.Priorities)int.Parse(Console.ReadLine());
                                    myParcel.Sender.ID = senderId;
                                    myParcel.Target.ID = targetId;
                                    myParcel.Requested = DateTime.Now; //A verifier
                                    myParcel.PickedUp = DateTime.MinValue;
                                    myParcel.Delivered = DateTime.MinValue;
                                    myParcel.Scheduled = DateTime.MinValue;
                                    int id = obj.addParcel(myParcel);
                                    Console.WriteLine($"package id is {id}");
                                    break;
                            }
                            break;
                        case option.UPDATE:
                            Console.WriteLine("You chose to update an entity.\nTo assign a Parcel to a Drone please press 1;\nTo pick up a parcel please press 2;\nTo plug in a Drone please press 3;\nTo remove a Drone from the ChargeSlot please press 4,\nTo exit please press 0:\n ");
                            update updating;
                            updating = (update)int.Parse(Console.ReadLine());
                            switch (updating)
                            {
                                case update.DRONE:
                                    Console.WriteLine("You chose to update a drone. \nPlease enter the Drone's Id and its new Model:\n");
                                    string newMod;
                                    int.TryParse(Console.ReadLine(), out int DroneID);
                                    newMod = Console.ReadLine();
                                    obj.updateDroneName(DroneID, newMod);
                                    break;
                                case update.STAT:
                                    Console.WriteLine("You chose to update a station. \nPlease enter the Station's Id, its new Name and/or chargeSlots :\n");
                                    int.TryParse(Console.ReadLine(), out int StatID);
                                    Console.WriteLine("Do you want to update the station's name? Enter yes or no\n");
                                    string ans = "yes";
                                    int newNam = 0;
                                    if (ans == Console.ReadLine())
                                    {
                                        Console.WriteLine("Enter the new name\n");
                                        int.TryParse(Console.ReadLine(), out newNam);
                                    }
                                    Console.WriteLine("Do you want to update the station's chargeSlots? Enter yes or no\n");

                                    int newCs = 0;
                                    if (ans == Console.ReadLine())
                                    {
                                        Console.WriteLine("Enter the number of chargeSlots\n");
                                        int.TryParse(Console.ReadLine(), out newCs);
                                    }
                                    obj.updateStationName_CS(StatID, newNam, newCs);
                                    break;

                                case update.CLIENT:
                                    Console.WriteLine("You choose to update a Client, please enter his ID number:\n");
                                    int.TryParse(Console.ReadLine(), out int myID);
                                    Console.WriteLine("Do you want to update the Client's name? Enter yes or no:\n");
                                    string answer = "yes", newPnumber = "n";
                                    string NewClName = "n";
                                    if (answer == Console.ReadLine())
                                    {
                                        Console.WriteLine("Please enter the new name:\n");
                                        NewClName = Console.ReadLine();
                                    }
                                    Console.WriteLine("Do you want to change the Client's PhoneNumber? Enter yes or no:\n");
                                    if (answer == Console.ReadLine())
                                    {
                                        Console.WriteLine("Please enter new PhoneNumber:\n");
                                        newPnumber = Console.ReadLine();
                                    }
                                    obj.updateClientName_Phone(myID, NewClName, newPnumber);
                                    break;


                                case update.CHARGING:
                                    Console.WriteLine("You want to plug in a Drone.\nPlease enter the Drone's Id:\n");
                                    int.TryParse(Console.ReadLine(), out int myDronID);
                                    obj.DroneToCharge(myDronID);
                                    break;

                                case update.CHARGED:
                                    Console.WriteLine("Your drone has finished charging.\nPlease enter the Drone's Id, and how long time it was charging:\n");
                                    int.TryParse(Console.ReadLine(), out int DrID);
                                    int.TryParse(Console.ReadLine(), out int time);
                                    obj.DroneCharged(DrID, time);
                                    break;


                            case update.ASSIGNEMENT:
                                Console.WriteLine("You chose to assign a Parcel to a Drone.\nPlease enter the Drone's Id:\n");               
                                int.TryParse(Console.ReadLine(), out int droneID);
                                obj.Assignement(droneID, 0);
                                break;

                                case update.PICKEDUP:
                                    Console.WriteLine("You chose to pick up a Parcel.\nPlease enter the Drone's Id:\n");
                                    int.TryParse(Console.ReadLine(), out int DronID);
                                    obj.PickedUp(DronID);
                                    break;

                                case update.DELIVERED:
                                    Console.WriteLine("Your Parcel has arrived!.\nPlease enter the Parcel's Id:\n");
                                    int.TryParse(Console.ReadLine(), out int myParcID);
                                    obj.delivered(myParcID);
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
                                    Console.WriteLine(obj.displayStation(StationId));// calls the function to print the asked station
                                    break;

                                case entities.DRONE:
                                    Console.WriteLine("You chose to print a Drone, please enter its Id:\n");
                                    int.TryParse(Console.ReadLine(), out int DroneId);
                                    Console.WriteLine(obj.displayDrone(DroneId));// calls the function to print the asked drone
                                    break;

                                case entities.PARCEL:
                                    Console.WriteLine("You chose to print a Parcel, please enter its Id:\n");
                                    int.TryParse(Console.ReadLine(), out int Pid);
                                    Console.WriteLine(obj.displayParcel(Pid));// calls the function to print the asked drone
                                    break;

                                case entities.CLIENT:
                                    Console.WriteLine("You chose to print a Client, please enter its Id:\n");
                                    int.TryParse(Console.ReadLine(), out int ClientId);
                                    Console.WriteLine(obj.displayClient(ClientId));// calls the function to print the asked drone
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
                                    foreach (var item in obj.DisplayStationList())
                                    {
                                        Console.WriteLine(item);
                                        Console.WriteLine(" ");
                                    }
                                    break;
                                case entities.DRONE:
                                    Console.WriteLine("Drones' List:\n");
                                    foreach (var item in obj.displayDroneList())
                                    {
                                        Console.WriteLine(item);
                                        Console.WriteLine(" ");
                                    }
                                    break;
                                case entities.PARCEL:
                                    Console.WriteLine("Parcels' List:\n");
                                    foreach (var item in obj.displayParcelList())
                                    {
                                        Console.WriteLine(item);
                                        Console.WriteLine(" ");
                                    }
                                    break;
                                case entities.CLIENT:
                                    Console.WriteLine("Clients' List:\n");
                                    foreach (var item in obj.displayClientList())
                                    {
                                        Console.WriteLine(item);
                                        Console.WriteLine(" ");
                                    }
                                    break;

                            }
                            break;

                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine("To add an entity please press 1\nTo update an entity please press 2\nTo print an entity please press 3\nTo print a list please press 4\nTo exit please press 0\n");
                options = (option)int.Parse(Console.ReadLine());
            
            } while (options != 0);
        }
    }
}