using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class DroneWithParcel
        {
            public int ID { set; get; }
            public double battery { set; get; }
            public Localisation departureLoc { set; get; }

            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID},\n";
                result += $"Battery is at: {battery}%,\n";
                result += $"Longitude is: {(int)(this.departureLoc.longitude)}°{(int)((this.departureLoc.longitude - (int)(this.departureLoc.longitude)) * 60)}' {((this.departureLoc.longitude - (int)(this.departureLoc.longitude)) * 60 - (int)((this.departureLoc.longitude - (int)(this.departureLoc.longitude)) * 60)) * 60}'',\n";
                result += $"Latitude is: {(int)(this.departureLoc.latitude)}°{(int)((this.departureLoc.latitude - (int)(this.departureLoc.latitude)) * 60)}' {((this.departureLoc.latitude - (int)(this.departureLoc.latitude)) * 60 - (int)((this.departureLoc.latitude - (int)(this.departureLoc.latitude)) * 60)) * 60}'',\n";


                return result;
            }
        }
    }
}
