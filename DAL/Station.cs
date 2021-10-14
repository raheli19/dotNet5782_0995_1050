using System;
namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int ID { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"ID is {ID},\n";
                result += $"Name is {Name},\n";
                result += $"Longitude is {Longitude},\n";
                result += $"Latitude is {Latitude},\n";
                result += $"ChargeSlots is {ChargeSlots},\n";
                return result;
            }
        }
        
    }
}