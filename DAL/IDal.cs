using System;
using System.Collections.Generic;
using System.Text;

using DalObject;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
       void AddDrone(Drone drone);// add a new drone to the dronelist

       void addClient(Client client);

       void addStation(Station station);

       void addParcel(Parcel parcel);

       void addDroneCharge(DroneCharge dc);

       void AddParcelToDrone(Parcel parcel,Drone d); // associate a parcel to a drone

        void UpdateDrone(Drone droneToUpdate);

        void updateDroneChargeList(int droneId, int statId);

        void UpdateStation(Station stationToUpdate);

        void UpdateClient(Client clientToUpdate);

        void Assignement(int parcelId, int droneId);

       void IsPickedUp(int parcelId, int droneId);

       void DeliveredToClient(int parcelId);//deliver a package to a customer

       void DroneToCharge(int droneId, int stationId);

       void DroneCharged(int droneId, int stationId);



       Station StationById(int id);
       Drone DroneById(int id);
       Client ClientById(int id);
       Parcel ParcelById(int id);

       IEnumerable<Station> StationList();
       IEnumerable<Drone> DroneList();
       IEnumerable<Client> ClientList();
       IEnumerable<Parcel> ParcelList();
        IEnumerable<DroneCharge> DroneChargeList();


        Parcel FindParcelAssociatedWithDrone(int droneId);
        double FindLat(int myID);
        double FindLong(int myID);
        void AddFromBLDroneCharging(int DroneID, int StationID);
        void AddParcelFromBL(Parcel p);

        List<int> IdStation();
        public List<int> clientReceivedParcel();

        //public Station FoundStation(int id);

    }
}
 
