using System;
using System.Collections.Generic;
using System.Text;

using BL;
using IBL.BO;

namespace IBL
{
    interface IBL
    {
        //functions ADD

        void addStation(Station s);

        void addDrone(Drone d);

        void addClient(Client c);

        void addParcel(Parcel p);

        //functions UPDATE

        void updateDrone(Drone d);

    }
}
