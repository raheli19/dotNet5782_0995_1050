using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class ParcelInDelivering
        {
            public int ID { set; get; }
            public Priorities priority { set; get; }
            public Client sender { set; get; }
            public Client receiver { set; get; }
        }
    }
}
