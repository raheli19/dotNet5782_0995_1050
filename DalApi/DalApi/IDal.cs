using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        void AddDrone(Drone drone);// add a new drone to the dronelist

        void AddClient(Client client);

        void AddStation(Station station);

        void AddParcel(Parcel parcel);

        void AddDroneCharge(DroneCharge dc);

        void AddParcelToDrone(Parcel parcel, Drone d); // associate a parcel to a drone

        void UpdateDrone(Drone droneToUpdate);

        void UpdateDroneChargeList(int droneId, int statId);

        void UpdateParcelFromBL(Parcel ParcelToUpdate);

        void UpdateStation(Station stationToUpdate);

        void UpdateClient(Client clientToUpdate);

        void Assignement(int parcelId, int droneId);

        void IsPickedUp(int parcelId, int droneId);

        void DeliveredToClient(int parcelId);//deliver a package to a customer

        void DroneToCharge(int droneId, int stationId);

        void DroneCharged(int droneId, int stationId);

        int RunnerNumber();

        double[] ElectricityUse();

        Station StationById(int id);
        Drone DroneById(int id);
        Client ClientById(int id);
        Parcel ParcelById(int id);

        IEnumerable<Station> IEStationList(Func<Station, bool> predicate = null);
        IEnumerable<Drone> IEDroneList();
        IEnumerable<Client> IEClientList();
        IEnumerable<Parcel> IEParcelList();
        IEnumerable<DroneCharge> IEDroneChargeList();

        Parcel FindParcelAssociatedWithDrone(int droneId);
        double FindLat(int myID);
        double FindLong(int myID);
        void AddFromBLDroneCharging(int DroneID, int StationID);
        void AddParcelFromBL(Parcel p);

        List<int> IdStation();
        public List<int> clientReceivedParcel();
        //void Assignement(object iD, int id);

        //public Station FoundStation(int id);

    }
}
