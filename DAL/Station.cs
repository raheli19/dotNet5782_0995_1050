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
                result += $"ID is: {ID},\n";
                result += $"Name is: {Name},\n";
                result += $"Longitude is: {(int)(this.Longitude)}°{(int)((this.Longitude - (int)(this.Longitude)) * 60)}' {((this.Longitude - (int)(this.Longitude)) * 60 - (int)((this.Longitude - (int)(this.Longitude)) * 60)) * 60}'',\n";
                result += $"Latitude is: {(int)(this.Latitude)}°{(int)((this.Latitude - (int)(this.Latitude)) * 60)}' {((this.Latitude - (int)(this.Latitude)) * 60 - (int)((this.Latitude - (int)(this.Latitude)) * 60)) * 60}'',\n";
                result += $"ChargeSlots is: {ChargeSlots}";
                return result;
            }
        }
        
    }
}