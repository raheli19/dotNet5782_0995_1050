using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class ParcelToClient
        {
            public int ID { set; get; }
            public WeightCategories weight { set; get; }

            public Priorities priority { set; get; }

            public ParcelStatus Status { set; get; }
            public ClientInParcel client { set; get; }


            public override string ToString()
            {

                return
                    $"ID: {ID}\nWeight:{weight}\nPriority:{priority}\n" +
                    $"Status: {Status}\nClient: {client}";
            }
        }
    }
}
