using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BL.BL;
namespace BL
{
    class Simulator
    {
        BLApi.IBL BL;
        int droneID;
        Action action;
        Func<bool> stop;
        Drone drone = new Drone();
        private int timer = 500;
        private int KM = 1;
        int DELAY = 500;
        int SPEED = 100;
        Localisation location = new Localisation();

        public Simulator(BLApi.IBL bl,int id,Action action, Func<bool>stop)
        {
            BL = bl;
            droneID = id;
            this.action = action;
            this.stop = stop;

            while (!stop())
            {
                lock (bl)
                {
                    drone = BL.displayDrone(droneID);

                }
                Thread.Sleep(DELAY);
                if (drone.Status == DroneStatuses.free)
                {
                    try
                    {
                        lock (bl) 
                        {
                            bl.Assignement(drone.ID);
                        }
                    }
                    catch(NotAvailable)
                    {
                        IEnumerable<ParcelDescription> parcels;
                        lock (bl)
                        {
                            parcels = bl.displayParcelsNotAssigned();
                        }
                        if (parcels.Any())//if there is no parcels in requested
                            Thread.Sleep(DELAY);
                        else //if there is no battery to drone to take parcels
                        {
                            location = drone.initialLoc;
                            lock (bl)
                            {
                                bl.DroneToCharge(drone.ID);
                                timer = (int)(bl.Distance(location, drone.initialLoc) / SPEED);
                            }
                            Thread.Sleep(Convert.ToInt32(timer) * 1000);
                        }
                    }
                }
                Thread.Sleep(DELAY);

                if (drone.Status == DroneStatuses.maintenance)
                {
                    lock (bl)
                    {
                        bl.DroneCharged(drone.ID,120);
                        if (drone.Battery != 100)
                            bl.DroneToCharge(drone.ID);
                    }
                }
                Thread.Sleep(DELAY);

                if (drone.Status == DroneStatuses.shipping)
                {
                    if(drone.myParcel.deliveringStatus==false) //if the parcel is just connected
                    {
                        lock (bl)
                        {
                            bl.PickedUp(drone.ID);
                            timer = (int) drone.myParcel.distance / SPEED;
                        }
                        Thread.Sleep(Convert.ToInt32(timer) * 1000);
                    }
                    else
                    {
                        lock (bl)
                        {
                            bl.delivered(drone.ID);
                            timer =(int) drone.myParcel.distance / SPEED;
                        }
                        Thread.Sleep(Convert.ToInt32(timer) * 1000);
                    }

                }
            }
        }

  
    }
}
