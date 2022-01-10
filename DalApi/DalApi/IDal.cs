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
        //-------------------------------------------------DAL OBJECT FUNCTIONS---------------------------------------------------------------
        int RunnerNumber();
        double[] ElectricityUse();

        //---------------------------------------------------ADD FUNCTIONS--------------------------------------------------------------------
        void AddDrone(Drone drone);// add a new drone to the dronelist
        void AddClient(Client client);
        void AddStation(Station station);
        void AddParcel(Parcel parcel);
        void AddDroneCharge(DroneCharge dc);
        void AddFromBLDroneCharging(int DroneID, int StationID);
        void AddParcelFromBL(Parcel p);

        //-----------------------------------------------------UPDATE---------------------------------------------------------------------------
        void UpdateDrone(Drone droneToUpdate);
        void UpdateDroneChargeList(int droneId, int statId);
        void UpdateParcelFromBL(Parcel ParcelToUpdate);
        void UpdateStation(Station stationToUpdate);
        void UpdateClient(Client clientToUpdate);

        //-----------------------------------------------------GET------------------------------------------------------------------------------
        Station StationById(int id);
        Drone DroneById(int id);
        Client ClientById(int id);
        Parcel ParcelById(int id);
        Parcel FindParcelAssociatedWithDrone(int droneId);

        //----------------------------------------------------GET LISTS------------------------------------------------------------------------
        IEnumerable<Station> IEStationList(Func<Station, bool> predicate = null);
        IEnumerable<Drone> IEDroneList();
        IEnumerable<Client> IEClientList();
        IEnumerable<Parcel> IEParcelList();
        IEnumerable<DroneCharge> IEDroneChargeList();

        //------------------------------------------------------HELP FUNCTIONS------------------------------------------------------------------
        void RemoveParcel(DO.Parcel p);
        double FindLat(int myID);
        double FindLong(int myID);
        List<int> IdStation();
        public List<int> clientReceivedParcel();
        int DalGetIdParcel(int senderId, int TargetId);


    }
}
