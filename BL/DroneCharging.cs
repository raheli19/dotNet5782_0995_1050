using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class DroneCharging
        {
            public int ID { set; get; }
            public double battery { set; get; }

            public override string ToString()
            {
                String result = "";
                result += $"The DroneCharging's id is {ID},\n";
                result += $"The battery of the Drone is at {battery}%.\n";

                return result;
            }
        }
    }
}
