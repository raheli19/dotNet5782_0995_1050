using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class DeliveringParcel
        {
            public int ID { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public bool DeliveringStatut { get; set; }
            public Localisation picking { get; set; }
            public Localisation delivered { get; set; }
            public double distance { get; set; }
        }
    }
}
