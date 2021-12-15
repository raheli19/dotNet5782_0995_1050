using System;
using System.Collections.Generic;
using System.Text;


namespace BO
{

    public class Localisation
    {
        public double longitude { get; set; }
        public double latitude { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Longitude is: {(int)(this.longitude)}°{(int)((this.longitude - (int)(this.longitude)) * 60)}' {((this.longitude - (int)(this.longitude)) * 60 - (int)((this.longitude - (int)(this.longitude)) * 60)) * 60}'',\n";
            result += $"Latitude is: {(int)(this.latitude)}°{(int)((this.latitude - (int)(this.latitude)) * 60)}' {((this.latitude - (int)(this.latitude)) * 60 - (int)((this.latitude - (int)(this.latitude)) * 60)) * 60}'',\n";
            return result;
        }
    }

}

