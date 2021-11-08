using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class ParcelDescription //parcel to list
        { 
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string TargetName { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }

            // public ParcelStatus Status{ get; set; }




        }
    }
}
