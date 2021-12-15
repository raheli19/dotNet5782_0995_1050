﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class ClientInParcel //client in Parcel
    {
        public int ID { set; get; }
        public string name { set; get; }

        public override string ToString()
        {
            String result = "";
            result += $"The client's id is {ID},\n";
            result += $"The client's name is {name}";
            return result;
        }
    }
}

