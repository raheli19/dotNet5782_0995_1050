using System;

namespace DO
{


    public struct Drone
    {

        public int ID { get; set; }
        public String Model { get; set; }
        public WeightCategories weight { get; set; }
        //public double Battery { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {ID},\n";
            result += $"Model is: {Model},\n";
            result += $"MaxWeight is: {weight},\n";
            //result += $"Status is: {Status},\n";

            return result;
        }
    }

}
