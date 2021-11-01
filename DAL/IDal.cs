using System;
using System.Collections.Generic;
using System.Text;

using DalObject;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        public void AddDrone(Drone drone);// add a new drone to the dronelist

        public void addClient(Client client);

        public void addStation(Station station);

        public void addParcel(Parcel parcel);

        public void addDroneCharge(DroneCharge dc);

        public void AddParcelToDrone(Parcel parcel); // associate a parcel to a drone

        public void Assignement(int parcelId, int droneId);

        public void IsPickedUp(int parcelId, int droneId);

        public void DeliveredToClient(int parcelId);//deliver a package to a customer

        public void DroneToCharge(int droneId, int stationId);

        public void DroneCharged(int droneId, int stationId);

        Station StationById(int id);
        Drone DroneById(int id);
        Client ClientById(int id);
        Parcel ParcelById(int id);
        IEnumerable<Station> StationList();

        IEnumerable<Drone> DroneList();
        IEnumerable<Client> ClientList();
        IEnumerable<Parcel> ParcelList();
       
    }
}
 
