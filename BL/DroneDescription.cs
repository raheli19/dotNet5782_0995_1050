using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        class DroneDescription
        {

            public int Id { set; get; }
            public String Model { get; set; }
            public WeightCategories weight { get; set; }
            public double battery { get; set; }
            public DroneStatuses Status { get; set; }

            public Localisation loc { get; set; }
            public int DeliveredParcels { get; set; }


        }
    }
}
