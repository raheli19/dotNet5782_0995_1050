using System;
namespace IDAL
{
    namespace DO
    {
 

         public struct Client
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public String Phone { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"ID is {ID},\n";
                result += $"Name is {Name},\n";
                result += $"Phone is {Phone.Substring(0, 3) + '-' + Phone.Substring(3)},\n";
                return result;
            }
        }
    }
}