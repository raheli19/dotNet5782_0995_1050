﻿using System;
using System.Collections.Generic;
using System.Text;


namespace IBL
{
    namespace BO
    {
        public class Client
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public String Phone { get; set; }
            public Localisation ClientLoc { set; get; }

            List<ParcelToClient> ParcLstFromClient = new List<ParcelToClient>();

            List<ParcelToClient> ParcLstToClient = new List<ParcelToClient>();


            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID},\n";
                result += $"Name is: {Name},\n";
                result += $"Phone is: {Phone.Substring(0, 3) + '-' + Phone.Substring(3)},\n";
                result += $"Longitude is: {(int)(this.ClientLoc.longitude)}°{(int)((this.ClientLoc.longitude - (int)(this.ClientLoc.longitude)) * 60)}' {((this.ClientLoc.longitude - (int)(this.ClientLoc.longitude)) * 60 - (int)((this.ClientLoc.longitude - (int)(this.ClientLoc.longitude)) * 60)) * 60}'',\n";
                result += $"Latitude is: {(int)(this.ClientLoc.latitude)}°{(int)((this.ClientLoc.latitude - (int)(this.ClientLoc.latitude)) * 60)}' {((this.ClientLoc.latitude - (int)(this.ClientLoc.latitude)) * 60 - (int)((this.ClientLoc.latitude - (int)(this.ClientLoc.latitude)) * 60)) * 60}'',\n";
                return result;
            }
        }
    }
}
