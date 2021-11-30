

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


        //-----------------------------------ACTIONS-------------------------------------------






    }

}