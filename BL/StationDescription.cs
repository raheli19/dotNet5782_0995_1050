using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        class StationDescription
        {
            public int Id { get; set; }
            public string name { get; set; }

            public int FreeChargeSlots { get; set; }
            public int FullChargeSlots { get; set; }



        }
    }
}
