﻿/*
 update
region
hagrala sur une liste pour la localisation du drone
*/



using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {


        //----------------------------HELP---------------------


        public Parcel FindParcelAssociatedWithDrone(int droneId)
        {
            Parcel myParcel = new Parcel();

            if (ParcelList.Exists(x => x.DroneId == droneId))
            {
                myParcel = ParcelList.Find(x => x.DroneId == droneId);
            }
            else
            {
                throw new ParcelException("There is no parcel which contains this droneID!");
            }

            return myParcel;
        }
        public double FindLat(int myID)
        {
            Client myClient = new Client();

            if (ClientList.Exists(x => x.ID == myID))
            {
                myClient = ClientList.Find(x => x.ID == myID);

            }
            else
            {
                throw new ClientException("There is not Client with such ID");
            }
            return (myClient).Latitude;
        }
        public double FindLong(int myID)
        {
            Client myClient = new Client();

            if (ClientList.Exists(x => x.ID == myID))
            {
                myClient = ClientList.Find(x => x.ID == myID);

            }
            else
            {
                throw new ClientException("There is not Client with such ID");
            }
            return (myClient).Longitude;
        }

        public void AddFromBLDroneCharging(int DroneID, int StationID)
        {
            DroneCharge DC = new DroneCharge();
            DC.DroneId = DroneID;
            DC.StationId = StationID;
            addDroneCharge(DC);


        }

        public void AddParcelFromBL(Parcel p)
        {
            Parcel myParcel = ParcelList.Find(x => x.ID == p.ID);
            ParcelList.Remove(myParcel);
            addParcel(p);

        }
        public List<int> IdStation()
        {
            List<int> IdStation = new List<int>();
            int sid = 0;
            foreach (var Stat in IEStationList())
            {
                sid = Stat.ID;
                IdStation.Add(sid);
            }
            return IdStation;

        }
        public List<int> clientReceivedParcel() // return list of id of the clients that have received 
        {
            List<int> list = new List<int>();
            //list.Add(DataSource.ParcelList.Find(x=> x.Delivered != DateTime.MinValue),x.id)
            foreach (var item in IEParcelList())
            {
                if (item.Delivered != DateTime.MinValue)
                    list.Add(item.TargetId);
            }
            return list;
        }


    }


}