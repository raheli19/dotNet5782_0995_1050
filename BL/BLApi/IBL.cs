using System;
using System.Collections.Generic;
using System.Text;

using BO;

namespace BLApi
{
    public interface IBL
    {
        //using functions
        string Name(int id);
        //functions ADD
        Parcel GetParcel(int id);
        Client GetClient(int id);
        Station GetStation(int id);
        Drone GetDrone(int id);
        int GetIdParcel(int senderId, int TargetId);
        void addStation(Station s);

        void addDrone(Drone d, int StationID);

        void addClient(Client c);

        int addParcel(Parcel p);

        //functions UPDATE

        void updateDroneName(int Id, string newName);

        void updateStationName_CS(int Id, string newName = " ", int newCS = -1);

        void updateClientName_Phone(int Id, string newName = " ", string newTel = " ");

        void DroneToCharge(int DroneId);

        void DroneCharged(int DroneId, double timeInCharge);

        void Assignement(int DroneId);

        void PickedUp(int DroneId);

        void delivered(int DroneId);

        List<ParcelToClient> FindParcelsToClient(int clientId);
        List<ParcelToClient> FindParcelsFromClient(int clientId);
        string parcelsFromCLiList(int clientID);
        string parcelsToCliList(int clientID);

        public List<int> AllTargets_Id();
        public List<int> AllSenders_Id();

        //functions print

        Station displayStation(int stationId);
        Drone displayDrone(int droneId);
        Client displayClient(int clientId);
        Parcel displayParcel(int parcelId);

        void RemoveParcel(Parcel parcelToRemove);
        DroneCharging displayDroneCharging(int stationId);
        IEnumerable<DroneCharging> displayDroneChargingList(int stationId);
        IEnumerable<StationDescription> DisplayStationList();
        IEnumerable<DroneDescription> displayDroneList();
        IEnumerable<ClientActions> displayClientList();
        IEnumerable<ParcelDescription> displayParcelList();
        IEnumerable<ParcelDescription> displayParcelsNotAssigned();
        IEnumerable<StationDescription> printFreeStations();

    }
}
