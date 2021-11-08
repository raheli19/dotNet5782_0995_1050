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
        Drone DroneById(int id);
        Station StationById(int id);
        Client ClientById(int id);
        Parcel ParcelById(int id);

        //functions ADD

        void addStation(Station s);

        void addDrone(Drone d, int StationID);

        void addClient(Client c);

        void addParcel(Parcel p);

        //functions UPDATE

        void updateDrone(int Id,string newName);

        void updateStation(int Id, int newName=-1,int newCS=-1);

        void updateClient(int Id, string newName =" ", string newTel=" ");

        void DroneToCharge(int DroneId);

        void DroneCharged(int DroneId,double timeInCharge);

        void Assignement(int DroneId);

        void PickedUp(int DroneId);

        void delivered(int DroneId);


        //functions print

        void printStation();
        void printDrone();
        void printClient();
        void printParcel();
        void printStationList();
        void printDroneList();
        void printClientList();
        void printParcelList();
        void printParcelsNotAssigned();
        void printFreeStations();

    }
}
