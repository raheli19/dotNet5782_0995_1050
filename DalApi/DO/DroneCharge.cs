﻿using System;

namespace DO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }
        public int StationId { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"DroneId is: {DroneId},\n";
            result += $"StationId is: {StationId},\n";

            return result;
        }
    }
}