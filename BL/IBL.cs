using System;
using System.Collections.Generic;
using System.Text;

using BL;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //using functions

        //functions ADD
        Parcel GetParcel(int id);
        Client GetClient(int id);
        Station GetStation(int id);
        Drone GetDrone(int id);

        void addStation(Station s);

        void addDrone(Drone d, int StationID);

        void addClient(Client c);

        int addParcel(Parcel p);

        //functions UPDATE

        void updateDroneName(int Id,string newName);

        void updateStationName_CS(int Id, int newName=-1,int newCS=-1);

        void updateClientName_Phone(int Id, string newName =" ", string newTel=" ");

        void DroneToCharge(int DroneId);

        void DroneCharged(int DroneId,double timeInCharge);

        void Assignement(int DroneId, int i);

        void PickedUp(int DroneId);

        void delivered(int DroneId);


        //functions print

        Station displayStation(int stationId);
        Drone displayDrone(int droneId);
        Client displayClient(int clientId);
        Parcel displayParcel(int parcelId);
        IEnumerable<StationDescription> DisplayStationList();
        IEnumerable<DroneDescription> displayDroneList();
        IEnumerable<ClientActions> displayClientList();
        IEnumerable<ParcelDescription> displayParcelList();
        IEnumerable<ParcelDescription> displayParcelsNotAssigned();
        void printFreeStations();
        double distance(double lat1, double lon1, double lat2, double lon2);
        Station NearestStation(Localisation l, bool flag);
        double DistanceAccToBattery(double battery);
        double BatteryAccToTime(double time);
        double BatteryAccToDistance(double distance);
        string Name(int id);
    }
}
