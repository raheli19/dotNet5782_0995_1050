using System;
using System.Collections.Generic;
using System.Text;

using DalObject;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {

DroneById(int id);
        Client ClientById(int id);
        Parcel ParcelById(int id);
        IEnumerable<Station> StationList();

        IEnumerable<Drone> DroneList();
        IEnumerable<Client> ClientList();
        IEnumerable<Parcel> ParcelList();
       
    }
}
 
