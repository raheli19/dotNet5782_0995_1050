

using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        public DalObject() { DataSource.Initialize(); }//constructor

        static Random rand = new Random();

        public static int ID { get; private set; }
        public static int DroneId { get; private set; }


        public int RunnerNumber()
        {
            return DataSource.Config.RunnerIDnumber;
        }

        public double[] ElectricityUse()
        {
            double[] arr = new double[5];
            arr[0] = DataSource.Config.BatteryFree;
            arr[1] = DataSource.Config.BatteryLightWeight;
            arr[2] = DataSource.Config.BatteryMiddleWeight;
            arr[3] = DataSource.Config.BatteryHeavyWeight;
            arr[4] = DataSource.Config.ChargeDroneRate;
            return arr;
        }




    }

}