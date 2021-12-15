using System;
using System.Collections.Generic;
using System.Text;


namespace BO
{
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Localisation Loc { get; set; }
        public int ChargeSlots { get; set; }

        public List<DroneCharging> DroneCharging = new List<DroneCharging>();


        public override string ToString()
        {
            String result = "";
            result += $"ID is: {ID},\n";
            result += $"Name is: {Name},\n";
            result += $"Longitude is: {(int)(this.Loc.longitude)}°{(int)((this.Loc.longitude - (int)(this.Loc.longitude)) * 60)}' {((this.Loc.longitude - (int)(this.Loc.longitude)) * 60 - (int)((this.Loc.longitude - (int)(this.Loc.longitude)) * 60)) * 60}'',\n";
            result += $"Latitude is: {(int)(this.Loc.latitude)}°{(int)((this.Loc.latitude - (int)(this.Loc.latitude)) * 60)}' {((this.Loc.latitude - (int)(this.Loc.latitude)) * 60 - (int)((this.Loc.latitude - (int)(this.Loc.latitude)) * 60)) * 60}'',\n";
            result += $"ChargeSlots is: {ChargeSlots}\n";

            StringBuilder builderDroneInCharges = new StringBuilder();
            foreach (var elementInCharge in DroneCharging)
                builderDroneInCharges.Append(elementInCharge).Append(", ");
            result += $"Drones charging in this station: {builderDroneInCharges.ToString()}";

            return result;
        }
    }
}
