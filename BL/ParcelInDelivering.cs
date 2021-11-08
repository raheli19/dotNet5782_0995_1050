using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class ParcelInDelivering
        {
            public int ID { get; set; }
            public bool DeliveringStatus { get; set; }

            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
           
            public ClientInParcel Sender { get; set; }
            public ClientInParcel Target { get; set; }
            public Localisation picking { get; set; }
            public Localisation delivered { get; set; }
            public double distance { get; set; }
        }
    }
}
