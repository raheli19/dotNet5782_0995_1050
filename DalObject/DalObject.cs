

using System;
using System.Collections.Generic;
using DO;
using DalApi;
using Dal;

namespace Dal
{
    sealed partial class DalObject : IDal //internal
    {
        
        #region Singleton
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
        DalObject() { }
        #endregion


        //public DalObject() { DataSource.Initialize(); }//constructor

        static Random rand = new Random();

        public static int ID { get; private set; }
        public static int DroneId { get; private set; }


        public int RunnerNumber()
        {
            return Configuration.RunnerIDnumber;
        }

        public double[] ElectricityUse()
        {
            double[] arr = new double[5];
            arr[0] = Configuration.BatteryFree;
            arr[1] = Configuration.BatteryLightWeight;
            arr[2] = Configuration.BatteryMiddleWeight;
            arr[3] = Configuration.BatteryHeavyWeight;
            arr[4] = Configuration.ChargeDroneRate;
            return arr;
        }




    }

}