using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            stam();
        }

        private static void stam() 
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
        }
    }
}
