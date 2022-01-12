﻿using BO;
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
        DroneDescription drone = new DroneDescription();
        private int timer = 500;
        private int KM = 1;
        int DELAY = 1000;
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
                    drone = bl.displayDroneList().First(x => x.Id == droneID);

                }
                action();
                Thread.Sleep(DELAY);
                if (drone.Status == DroneStatuses.free)
                {
                    try
                    {
                        lock (bl) 
                        {
                            bl.Assignement(drone.Id);
                            drone = bl.displayDroneList().First(x => x.Id == droneID);
                            
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
                        { Thread.Sleep(DELAY);
                            action();
                        }
                        else //if there is no battery to drone to take parcels
                        {
                            location = drone.loc;
                            lock (bl)
                            {
                                bl.DroneToCharge(drone.Id);
                                timer = (int)(bl.Distance(location, drone.loc) / SPEED);

                            }
                            Thread.Sleep(Convert.ToInt32(timer) * 1000);
                            action();
                        }
                    }
                }
                Thread.Sleep(DELAY);

                if (drone.Status == DroneStatuses.maintenance)
                {
                    lock (bl)
                    {
                        bl.DroneCharged(drone.Id,120);
                        if (drone.battery != 100)
                            bl.DroneToCharge(drone.Id);
                    }
                }
                Thread.Sleep(DELAY);
                action();

                if (drone.Status == DroneStatuses.shipping)
                {
                    
                    if(bl.displayDrone(droneID).myParcel.deliveringStatus==false) //if the parcel is just connected
                    {
                        lock (bl)
                        {
                            bl.PickedUp(drone.Id);
                            drone = bl.displayDroneList().First(x => x.Id == droneID);
                            timer = (int)bl.displayDrone(droneID).myParcel.distance / SPEED;
                        }
                        Thread.Sleep(Convert.ToInt32(timer) * 10);
                        action();
                    }
                    else
                    {
                        lock (bl)
                        {
                            bl.delivered(drone.Id);
                            drone = bl.displayDroneList().First(x => x.Id == droneID);
                            timer =(int)bl.displayDrone(droneID).myParcel.distance / SPEED;
                        }
                        Thread.Sleep(Convert.ToInt32(timer) * 1000);
                        action();
                    }

                }
            }
        }

  
    }
}
