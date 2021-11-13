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
                result += $"Longitude is: {(int)(this.loc.longitude)}°{(int)((this.loc.longitude - (int)(this.loc.longitude)) * 60)}' {((this.loc.longitude - (int)(this.loc.longitude)) * 60 - (int)((this.loc.longitude - (int)(this.loc.longitude)) * 60)) * 60}'',\n";
                result += $"Latitude is: {(int)(this.loc.latitude)}°{(int)((this.loc.latitude - (int)(this.loc.latitude)) * 60)}' {((this.loc.latitude - (int)(this.loc.latitude)) * 60 - (int)((this.loc.latitude - (int)(this.loc.latitude)) * 60)) * 60}'',\n";
                result += $"ChargeSlots is: {ChargeSlots}";
                return result;
            }
        }
    }
}
