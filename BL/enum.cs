using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
         public enum DroneStatuses
        {
            free, maintenance, shipping
        };
        public enum WeightCategories
        {
            low, middle, heavy
        };
        public enum Priorities
        {
            regular, fast, emergency
        };
        public enum ParcelStatus
        {
           requested,scheduled, pickedup, delivered
        };

    }
}
