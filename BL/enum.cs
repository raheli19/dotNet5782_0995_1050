﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BL
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

    }
}