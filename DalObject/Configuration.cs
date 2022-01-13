using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    static public class Configuration
    {
        public static int RunnerIDnumber = 1000;

        public static double BatteryFree = 0.0005; // % of battery per km.
        public static double BatteryLightWeight = 0.001;
        public static double BatteryMiddleWeight = 0.0015;
        public static double BatteryHeavyWeight = 0.002;
        public static double ChargeDroneRate = 10;



    }
}
