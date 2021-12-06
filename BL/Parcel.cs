using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int ID { get; set; }
            public ClientInParcel Sender{ get; set; }
            public ClientInParcel Target { get; set; }
            public WeightCategories Weight { get; set; } // à construire
            public Priorities Priority { get; set; }
            public DroneWithParcel Drone { get; set; }

            public DateTime? Requested { get; set; }
            public DateTime? Scheduled { get; set; }
            public DateTime? PickedUp { get; set; }
            public DateTime? Delivered { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID},\n";
                result += $"Sender is: {Sender},\n";
                result += $"Target is: {Target},\n";
                result += $"Weight is: {Weight},\n";
                result += $"Priority is: {Priority},\n";
                result += $"Requested is: {Requested},\n";
                result += $"Drone is: {Drone},\n";
                result += $"Scheduled is: {Scheduled},\n";
                result += $"PickedUp is: {PickedUp},\n";
                result += $"Delivered is: {Delivered},\n";

                return result;
            }
        }
    }
}
