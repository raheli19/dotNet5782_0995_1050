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

            public override string ToString()
            {
                String result = "";
                result += $"ID is: {Id},\n";
                result += $"Name is: {name},\n";
                result += $"Phone is: {phone.Substring(0, 3) + '-' + phone.Substring(3)},\n";
                result += $"The customer sent {deliveredParcels} packages,\n";
                result += $"The customer is sending {deliveringParcels} packages,\n";
                result += $"The customer received: {receivedParcels} packages,\n";
                result += $"The customer is wainting for {receivingParcels} packages.\n";
                
                return result;
            }
        }
    }
}
