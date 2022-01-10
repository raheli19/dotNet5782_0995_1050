
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using BO;
using DalApi;
using System.Linq;
using BLApi;

namespace BL
{

    partial class BL : IBL
    {

        //------------------------------------------HELP------------------------------------------
        //Distance
        #region helpFunctions
        public double distance(double lat1, double lon1, double lat2, double lon2)
        {
            var myPI = 0.017453292519943295;    // Math.PI / 180
            var a = 0.5 - Math.Cos((lat2 - lat1) * myPI) / 2 +
                    Math.Cos(lat1 * myPI) * Math.Cos(lat2 * myPI) *
                    (1 - Math.Cos((lon2 - lon1) * myPI)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
        #endregion

        #region NearestStation
        public Station NearestStation(Localisation l, bool flag)
        {
            Station s = new Station();
            s.Loc = new Localisation();
            double lat1 = l.latitude;
            double long1 = l.longitude;
            double minDistance = 99999999;
            double tempDistance = 0;

            foreach (var item in dal.IEStationList())
            {
                if (flag == true)
                {
                    if (item.ChargeSlots > 0)// if theres a slot available
                    { tempDistance = distance(lat1, long1, item.Latitude, item.Longitude); }
                    else
                        continue;
                }

                if (minDistance > tempDistance)
                {
                    minDistance = tempDistance;// keeps the closest one
                    s.ID = item.ID;
                    s.Name = item.Name;
                    s.Loc.longitude = item.Longitude;
                    s.Loc.latitude = item.Latitude;
                    s.ChargeSlots = item.ChargeSlots;
                }
            }
            return s;
        }
        #endregion

        #region DistanceAccToBattery
        public double DistanceAccToBattery(double battery)
        {
            //Le drone perd 1% en 7 min  et la vitesse du drone de 50 km/h
            // le drone gagne 1% en 7 min
            //double timeInHours = (7 / 60);
            //double speed = 50;  //50km/h
            //double totalTime = timeInHours * battery;
            //double distance = totalTime * speed;
            double distance = battery / 0.0005;
            return distance;

        }
        #endregion

        #region BatteryAccToTime
        public double BatteryAccToTime(double time, double battery)
        {
            double timeInHour = time / 60;
            double distance = 150 * timeInHour;
            battery += distance * 0.5;
            return battery;

        }
        #endregion

        #region BatteryAccToDistance
        public double BatteryAccToDistance(double distance)
        {
            double batteryLost = distance * 0.0005;
            return batteryLost;
        }
        #endregion

        //return the client's name according to his id
        #region ClientName 
        public string Name(int id)
        {
            try
            {
                DO.Client c = dal.ClientById(id);
                return c.Name;
            }
            catch (DO.ClientException custEX)
            {
                throw new NotFound("Didn't find", custEX);

            }


        }
        #endregion

        #region Location
        public Localisation location(double lat1, double long1)
        {
            Localisation l = new Localisation();
            l.longitude = long1;
            l.latitude = lat1;
            return l;
        }
        #endregion

        #region ClosestParcel
        public DO.Parcel ClosestParcel(List<DO.Parcel> list, Localisation droneLoc)
        {
            double minDist = double.MaxValue;
            DO.Parcel tempParcel = new DO.Parcel();
            foreach (var item in list)
            {
                double dist = distance(dal.ClientById(item.SenderId).Latitude, dal.ClientById(item.SenderId).Longitude, droneLoc.latitude, droneLoc.longitude);
                if (dist < minDist)
                {
                    minDist = dist;
                    tempParcel = item;
                }

            }
            return tempParcel;
        }
        #endregion

        #region ConvertStationToDal
        public DO.Station ConvertStationToDal(Station s)
        {
            DO.Station stat = new DO.Station();
            stat.ID = s.ID;
            stat.Name = s.Name;
            stat.Latitude = s.Loc.latitude;
            stat.Longitude = s.Loc.longitude;
            stat.ChargeSlots = s.ChargeSlots;
            return stat;

        }
        #endregion

        public void updateBlDroneList(DroneDescription droneToUpdate)
        {

            DroneDescription blD = new DroneDescription();
            blD = DroneList.Find(x => droneToUpdate.Id == x.Id);
            DroneList.Remove(blD);
            DroneList.Add(droneToUpdate);


        }

        public int GetIdParcel(int senderId, int TargetId)
        {
             int id = dal.DalGetIdParcel( senderId,TargetId);
            return id;
        }

        


    }
}