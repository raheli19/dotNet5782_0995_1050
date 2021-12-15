using System;
using System.Collections.Generic;
using System.Text;


namespace BO
{
    public class ParcelDescription //parcel to list
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories weight { get; set; }
        public Priorities priority { get; set; }
        public ParcelStatus Status { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id},\n";
            result += $"Sender's name is {SenderName},\n";
            result += $"Target's name is {TargetName},\n";
            result += $"Parcel's weight is: {weight},\n";
            result += $"Priority is: {priority}.\n";
            return result;
        }



    }
}

