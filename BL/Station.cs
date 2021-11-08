using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int ID { get; set; }
            public int Name { get; set; }
            public Localisation loc { get; set; }
            public int ChargeSlots { get; set; }

            public List <Drone> DroneCharging= new List<Drone>();
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
