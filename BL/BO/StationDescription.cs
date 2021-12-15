using System;
using System.Collections.Generic;
using System.Text;


namespace BO
{
    public class StationDescription
    {
        public int Id { get; set; }
        public string name { get; set; }

        public int freeChargeSlots { get; set; }
        public int fullChargeSlots { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"ID is: {Id},\n";

            result += $"Name is: {name},\n";
            result += $"Amount of freeChargeSlots: {freeChargeSlots},\n";
            result += $"Amount of fullChargeSlots: {fullChargeSlots},\n";

            return result;
        }


    }
}

