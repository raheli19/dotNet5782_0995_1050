using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class ClientActions//client to List
        {
            public int Id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public int deliveredParcels { get; set; }
            public int deliveringParcels { get; set; }
            public int receivedParcels { get; set; }
            public int receivingParcels { get; set; }


        }
        }
}
