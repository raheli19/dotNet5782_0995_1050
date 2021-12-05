using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
       public class DroneDescription
        {

            public int Id { set; get; }
            public String Model { get; set; }
            public WeightCategories weight { get; set; }
            public double battery { get; set; }
            public DroneStatuses Status { get; set; }
            public Localisation loc { get; set; }
            public int parcelId { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"ID is: {Id},\n";
                result += $"Name is: {Model},\n";
                result += $"Weight can carry is: {weight},\n";
                result += $"Battery is at: {battery}%,\n";
                result += $"Drone's statut is: {Status},\n";
                result += $"Longitude is: {(int)(this.loc.longitude)}°{(int)((this.loc.longitude - (int)(this.loc.longitude)) * 60)}' {((this.loc.longitude - (int)(this.loc.longitude)) * 60 - (int)((this.loc.longitude - (int)(this.loc.longitude)) * 60)) * 60}'',\n";
                result += $"Latitude is: {(int)(this.loc.latitude)}°{(int)((this.loc.latitude - (int)(this.loc.latitude)) * 60)}' {((this.loc.latitude - (int)(this.loc.latitude)) * 60 - (int)((this.loc.latitude - (int)(this.loc.latitude)) * 60)) * 60}'',\n";
                result += $"ID of delivering parcel: {parcelId}.\n";
                return result;
            }

        }
    }
}
