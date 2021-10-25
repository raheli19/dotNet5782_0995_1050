using System;
using IDAL.DO;
using DAL;
namespace ConsoleUI
{
    class Program
    {
         enum option { EXIT=0 , ADD, UPDATE, PRINT,PRINT_LIST,};
        enum entities { EXIT=0, STATION, DRONE, PARCEL, CLIENT, DRONECHARGES};
        enum update { EXIT, ASSOCIATION, DELIVERED,CHARGING, CHARGED,};

        public static void Main(string[] args)
        {
            option options;
            entities entity;
            Console.WriteLine("Welcome to our Nebula Drone delivery System!\n");
            Console.WriteLine("To add an entity please press 1\n To update an entity please press 2\n To print an entity please press 3\n To print a list please press 4\n To exit please press 0\n");
            options= (option)int.Parse(Console.ReadLine());
            do
            {
                switch (options)
                {
                    case option.ADD:
                        

                            Console.WriteLine("You have chosen to add an entity.\n ");
                            Console.WriteLine("To add a Station please press 1,\n To add a drone please press 2,\nTo add a Parcel please press 3,\nTo add a client please press 4\n To add a DroneCharge please press 5\n To exit please press 0\n");
                            entity = (entities)int.Parse(Console.ReadLine());
                            switch (entity)
                            { 
                                case entities.STATION:
                                   
                                        IDAL.DO.Station myStation = new IDAL.DO.Station();
                                        int myId;
                                        int myName;
                                        Console.WriteLine("Enter the Id, name, longitude, latitude of the station:");
                                        myId = int.Parse(Console.ReadLine());
                                        myName = int.Parse(Console.ReadLine());
                                        double.TryParse(Console.ReadLine(), out double myLongitude);
                                        double.TryParse(Console.ReadLine(), out double myLatitude);
                                        myStation.ID = myId;
                                        myStation.Name = myName;
                                        myStation.Longitude= myLongitude;
                                break;
                                    
                                case entities.DRONE:
                                  break;
                                case entities.PARCEL:
                                break;
                                case entities.CLIENT:
                                break;
                                case entities.DRONECHARGES:
                                break;

                            }
                        break;
                    case option.UPDATE:
                        break;
                    case option.PRINT:
                        break;
                    case option.PRINT_LIST:
                        break;
                };
            } while (options != 0);
        }
       
        /*private static void stam() 
        {
            IDAL.DO.Client myClient = new IDAL.DO.Client
            {
                ID = 113,
                Latitude = 56123,
                Longitude = 56,
                Name = "moi",
                Phone = "0585170202",

            };
            Console.WriteLine(myClient);
        }*/
    }
}
