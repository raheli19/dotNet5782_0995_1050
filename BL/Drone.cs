﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int ID { get; set; }
            public String Model { get; set; }
            public WeightCategories MaxWeight { get; set; } // à construire
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public Localisation initialLoc { get; set; }
            public ParcelInDelivering myParcel{get;set;}
            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID},\n";
                result += $"Model is: {Model},\n";
                result += $"MaxWeight is: {MaxWeight},\n";
                result += $"Status is: {Status},\n";
                result += $"Battery is: {Battery}%,\n";
                result += $"Location is: {initialLoc}\n";
                result += $"Amount of packages in delivering is: {myParcel}\n";
                return result;
            }
        }
    }
}
