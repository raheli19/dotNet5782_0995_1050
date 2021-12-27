
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using BO;
using DalApi;
using System.Linq;
using BLApi;

namespace BL
{

    sealed partial class BL : IBL
    {
        //-----------------------------------PRINT-FUNCTIONS--------------------------------------
        #region PRINTING
       
        #region DisplayStationList
        /// <summary>
        /// This function gets all the stations with their details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationDescription> DisplayStationList()
        {
            IEnumerable<DO.Station> stationsFromDal = p.IEStationList();   //gets the station list from DAL
            List<StationDescription> statList = new List<StationDescription>();  //creates a new list of station description
            foreach (var item in p.IEStationList())
            {
                //StationDescription statD = new StationDescription();
                //statD.Id = item.ID;
                //statD.name = item.Name;
                //foreach( var item2 in p.DroneChargeList())// full chargeSlots
                //{
                //    if (item2.StationId == item.ID) // if the drone is in the station
                //    { 
                //        statD.fullChargeSlots++;

                //    }

                //}

                //statD.freeChargeSlots = item.ChargeSlots;// free ones
                //statList.Add(statD);// add it to the list

                Station station = displayStation(item.ID);      //copies the fields 
                statList.Add(new StationDescription()
                {
                    Id = item.ID,
                    name = item.Name,
                    freeChargeSlots = item.ChargeSlots,
                    fullChargeSlots = station.DroneCharging.Count()
                });

            }
            return statList;
        }

        #endregion

        #region displayDroneList

        /// <summary>
        /// This function returns the list of drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneDescription> displayDroneList()
        {
            List<DroneDescription> temp = new List<DroneDescription>();
            
            foreach (var item in DroneList)
            {

                temp.Add(new DroneDescription()
                {
                    Id = item.Id,
                    Model = item.Model,
                    weight = (WeightCategories)item.weight,
                    loc = item.loc,
                    parcelId = item.parcelId,
                    Status=item.Status,
                    battery = item.battery,
                }); ;
            }
            //if (DroneList.Count == 0)
            //    throw new EmptyListException("No drones to display.");
            return temp;


        }

        #endregion

        #region displayClientList

        /// <summary>
        /// This function returns the list of clients with their details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientActions> displayClientList()
        {
            IEnumerable<DO.Client> customersFromDal = p.IEClientList();
            List<ClientActions> LstCA = new List<ClientActions>();

            foreach (var item in p.IEClientList())
            {
                ClientActions tempCA = new ClientActions();
                tempCA.Id = item.ID;
                tempCA.name = item.Name;
                tempCA.phone = item.Phone;
                IEnumerable<DO.Parcel> sent_and_delivLst = p.IEParcelList().Where(x => x.SenderId == item.ID && x.Delivered != DateTime.MinValue);
                IEnumerable<DO.Parcel> sent_and_notDelivLst = p.IEParcelList().Where(x => x.SenderId == item.ID && x.Delivered == DateTime.MinValue);
                IEnumerable<DO.Parcel> receivLst = p.IEParcelList().Where(x => x.TargetId == item.ID && x.Delivered != DateTime.MinValue);
                IEnumerable<DO.Parcel> receivingLst = p.IEParcelList().Where(x => x.TargetId == item.ID && x.Delivered != DateTime.MinValue);
                int sent_and_delivLstCount = sent_and_delivLst.Count();
                int sent_and_notDelivLstCount = sent_and_notDelivLst.Count();
                int receivLstCount = receivLst.Count();
                int receivingLstCount = receivingLst.Count();
                tempCA.deliveredParcels = sent_and_delivLstCount;
                tempCA.deliveringParcels = sent_and_notDelivLstCount;
                tempCA.receivedParcels = receivLstCount;
                tempCA.receivingParcels = receivingLstCount;
                LstCA.Add(tempCA);
            }
            return LstCA;
        }
        #endregion

        #region displayParcelList

        /// <summary>
        /// This function returns the list of parcels with their details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelDescription> displayParcelList()
        {
            IEnumerable<DO.Parcel> parcelsFromDal = p.IEParcelList();
            List<ParcelDescription> parList = new List<ParcelDescription>();

            foreach (var item in p.IEParcelList())
            {
                ParcelDescription tempPar = new ParcelDescription();
                tempPar.Id = item.ID;
                try
                {
                    tempPar.SenderName = p.ClientById(item.SenderId).Name;
                    tempPar.TargetName = p.ClientById(item.TargetId).Name;
                }
                catch (DO.ClientException ex)
                {
                    throw new IDNotFound("not found", ex);
                }
                tempPar.weight = (WeightCategories)item.Weight;
                tempPar.priority = (Priorities)item.Priority;
                if (item.Requested != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.requested;
                }
                else if (item.Scheduled != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.scheduled;
                }
                else if (item.PickedUp != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.pickedup;
                }
                else if (item.Delivered != DateTime.MinValue)
                {
                    tempPar.Status = ParcelStatus.delivered;
                }
                parList.Add(tempPar);
            }
            return parList;
        }
        #endregion
        public IEnumerable<StationDescription> printFreeStations()
        {
            List<StationDescription> statList = new List<StationDescription>();
            StationDescription tempStat = new StationDescription();
            IEnumerable<DO.Station> dalFreeStations = new List<DO.Station>();
            dalFreeStations = p.IEStationList(x => x.ChargeSlots > 0);
           

            foreach (var item in dalFreeStations)
            {
                statList.Add(new StationDescription
                {
                    Id = item.ID,
                    name = item.Name,
                    freeChargeSlots = item.ChargeSlots,
                    fullChargeSlots = p.IEDroneChargeList().Where(x => x.StationId == item.ID).Count(),
                }) ;
            }
            return statList;

            //foreach (var item in p.IEStationList())
            //{
            //    if (item.ChargeSlots > 0)
            //    {
            //        tempStat.Id = item.ID;
            //        tempStat.name = item.Name;
            //        tempStat.freeChargeSlots = item.ChargeSlots;
            //        foreach (var item2 in p.IEDroneChargeList())// full chargeSlots
            //        {
            //            if (item2.StationId == item.ID)
            //                tempStat.fullChargeSlots++;
            //        }
            //    }
            //    statList.Add(tempStat);
            //}
        }
        public IEnumerable<ParcelDescription> displayParcelsNotAssigned()
        {
            List<ParcelDescription> PDList = new List<ParcelDescription>();
            foreach (var item in p.IEParcelList())
            {
                if (item.Scheduled != DateTime.MinValue)
                {
                    ParcelDescription tempPD = new ParcelDescription();
                    tempPD.Id = item.ID;
                    tempPD.SenderName = Name(item.SenderId);
                    tempPD.TargetName = Name(item.TargetId);
                    tempPD.weight = (WeightCategories)item.Weight;
                    tempPD.priority = (Priorities)item.Priority;
                    if (item.Requested != DateTime.MinValue)
                    {
                        tempPD.Status = ParcelStatus.requested;
                    }
                    else if (item.Scheduled != DateTime.MinValue)
                    {
                        tempPD.Status = ParcelStatus.scheduled;
                    }
                    else if (item.PickedUp != DateTime.MinValue)
                    {
                        tempPD.Status = ParcelStatus.pickedup;
                    }
                    else if (item.Delivered != DateTime.MinValue)
                    {
                        tempPD.Status = ParcelStatus.delivered;
                    }
                    PDList.Add(tempPD);
                }
            }
            return PDList;
        }

        #endregion

        public IEnumerable<DroneCharging> displayDroneChargingList(int stationId)
        {
           
                DronesChargingInStation dc = new DronesChargingInStation();
                List<DroneCharging> droneCharging = new List<DroneCharging>();
                List<int> DronesID = new List<int>();

                IEnumerable<DO.DroneCharge> droneCharges = p.IEDroneChargeList();  //finds the list which contains the the drone charges from DAL
                foreach (var item in droneCharges)
                {
                    if (item.StationId == stationId)  // finds the station with the ID received 
                    {
                        Drone droneInStation = GetDrone(item.DroneId);   // finds the drones contained in this station
                        droneCharging.Add(new DroneCharging() { ID = item.DroneId, battery = droneInStation.Battery, });

                    }

              
            }
            return droneCharging;
        }



    }
}