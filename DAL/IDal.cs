using System;
using System.Collections.Generic;
using System.Text;

using DalObject;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {


        //functions ADD
        void AddDrone(Drone drone);// add a new drone to the dronelist
         void addClient(Client client);
         void addStation(Station station);
         void addParcel(Parcel parcel);
         void addDroneCharge(DO.DroneCharge dc);
        void AddParcelToDrone(DO.Parcel parcel); // associate a parcel to a drone


        //functions UPDATE

        void Assignement(int parcelId, int droneId);
        void IsPickedUp(int parcelId, int droneId);
        void DeliveredToClient(int parcelId);//deliver a package to a customer
        void DroneToCharge(int droneId, int stationId);
        void DroneCharged(int droneId, int stationId);
        Station StationById(int id);
        Drone DroneById(int id);
        Client ClientById(int id);
        Parcel ParcelById(int id);
        List<Station> StationList();
        List<Drone> DroneList();
        List<Client> ClientList();
        List<Parcel> ParcelList();
       
    }
}
 
