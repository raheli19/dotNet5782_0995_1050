using System;
using System.Collections.Generic;
using System.Text;


namespace BO
{
    public class ParcelInDelivering
    {
        public int ID { get; set; }
        public bool deliveringStatus { get; set; }

        public WeightCategories weight { get; set; }
        public Priorities priority { get; set; }

        public ClientInParcel Sender { get; set; }
        public ClientInParcel Target { get; set; }
        public Localisation picking { get; set; }
        public Localisation delivered { get; set; }
        public double distance { get; set; }

        string Status(bool deliveringStatus)
        {
            if (deliveringStatus == false)
                return " not ";
            return " ";

        }
        public override string ToString()
        {
            String result = "";
            result += $"ID is: {ID},\n";
            result += $"The Package is{Status(deliveringStatus)}in delivering,\n";
            result += $"Package's weight is {weight},\n";
            result += $"Package's priority is {priority},\n";
            result += $"Sender: {Sender},\n";
            result += $"Target: {Target},\n";
            result += $"Picking location: {picking},\n";
            result += $"Delivering location:  {delivered}\n";
            result += $"Distance: {distance}\n";
            return result;
        }
    }
}
