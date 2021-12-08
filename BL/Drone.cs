using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int ID { get; set; }
            public String Model { get; set; }
            public WeightCategories MaxWeight { get; set; } // à construire
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public Localisation initialLoc { get; set; }
            public ParcelInDelivering myParcel{get;set;}
            public override string ToString()
            {
                String result = "";
                result += $"ID: {ID},\n";
                result += $"Model: {Model},\n";
                result += $"MaxWeight: {MaxWeight},\n";
                result += $"Status: {Status},\n";
                result += $"Battery: {Battery}%,\n";
                result += $"Location: {initialLoc}";
                result += $"Parcel in transfer:";
                if(myParcel.ID!=0)
                   result+= $" {myParcel}\n";
                return result;
            }
        }
    }
}
