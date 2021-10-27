using System;
namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int ID { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; } // à construire
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }


            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID},\n";
                result += $"SenderId is: {SenderId},\n";
                result += $"TargetId is: {TargetId},\n";
                result += $"Weight is: {Weight},\n";
                result += $"Priority is: {Priority},\n";
                result += $"Requested is: {Requested},\n";
                result += $"DroneId is: {DroneId},\n";
                result += $"Scheduled is: {Scheduled},\n";
                result += $"PickedUp is: {PickedUp},\n";
                result += $"Delivered is: {Delivered},\n";
                
                return result;
            }
        }
    }
}