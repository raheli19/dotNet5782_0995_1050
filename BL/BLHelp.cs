
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DalObject;
using System.Linq;

namespace IBL
{

    public partial class BL : IBL
    {

        //------------------------------------------HELP------------------------------------------
        //Distance
        #region helpFunctions
        private double distance(double lat1, double lon1, double lat2, double lon2)
        {
            var myPI = 0.017453292519943295;    // Math.PI / 180
            var a = 0.5 - Math.Cos((lat2 - lat1) * myPI) / 2 +
                    Math.Cos(lat1 * myPI) * Math.Cos(lat2 * myPI) *
                    (1 - Math.Cos((lon2 - lon1) * myPI)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
        private Station NearestStation(Localisation l, bool flag)
        {
            Station s = new Station();
            s.Loc = new Localisation();
            double lat1 = l.latitude;
            double long1 = l.longitude;
            double minDistance = 99999999;
            double tempDistance = 0;

            foreach (var item in p.IEStationList())
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
        private double DistanceAccToBattery(double battery)
        {
            //Le drone perd 1% en 7 min  et la vitesse du drone de 50 km/h
            // le drone gagne 1% en 7 min
            double timeInHours = 7 / 60;
            double speed = 50;  //50km/h
            double totalTime = timeInHours * battery;
            double distance = totalTime * speed;
            return distance;

        }
        private double BatteryAccToTime(double time)
        {
            double batt = time * 7;
            return batt;

        }
        private double BatteryAccToDistance(double distance)
        {
            double time = distance / 50;
            double batteryLost = time / (7 / 60);
            return batteryLost;
        }
        private string Name(int id)
        {
            try
            {
                IDAL.DO.Client c = p.ClientById(id);
                return c.Name;
            }
            catch (IDAL.DO.ClientException custEX)
            {
                throw new NotFound("Didn't find", custEX);

            }


        }
        private Localisation location(double lat1, double long1)
        {
            Localisation l = new Localisation();
            l.longitude = long1;
            l.latitude = lat1;
            return l;
        }
        IDAL.DO.Parcel ClosestParcel(List<IDAL.DO.Parcel> list, Localisation droneLoc)
        {
            double minDist = double.MaxValue;
            IDAL.DO.Parcel tempParcel = new IDAL.DO.Parcel();
            foreach (var item in list)
            {
                double dist = distance(p.ClientById(item.SenderId).Latitude, p.ClientById(item.SenderId).Longitude, droneLoc.latitude, droneLoc.longitude);
                if (dist < minDist)
                {
                    minDist = dist;
                    tempParcel = item;
                }

            }
            return tempParcel;
        }
        IDAL.DO.Station ConvertStationToDal(Station s)
        {
            IDAL.DO.Station stat = new IDAL.DO.Station();
            stat.ID = s.ID;
            stat.Name = s.Name;
            stat.Latitude = s.Loc.latitude;
            stat.Longitude = s.Loc.longitude;
            stat.ChargeSlots = s.ChargeSlots;
            return stat;

        }
        bool CheckId(int id)
        {
            int i = 0;
            while (id > 0)
            {
                ++i;
                id /= 10;

            }
            if (i == 8)
                return true;
            return false;
        }
        void updateBlDroneList(DroneDescription droneToUpdate)
        {

        }
        #endregion


    }
}