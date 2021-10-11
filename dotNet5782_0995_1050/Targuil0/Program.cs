using System;

namespace Targuil0
{
    class Program
    {
        static void Main(string[] args)
        {
            Welcome2833();
            Console.ReadKey();
        }

        private static void Welcome2833()
        {
            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
