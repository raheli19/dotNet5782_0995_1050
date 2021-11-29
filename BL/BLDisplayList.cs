
//Tania:DroneToCharge, DroneCharged et Assignment verifier


using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DAL;
using DalObject;
using System.Linq;

namespace IBL
{

    public partial class BL : IBL
    {
        //-----------------------------------PRINT-FUNCTIONS--------------------------------------
        #region PRINTING
        #region IENUMERABLE 
        #region DisplayStationList
        public IEnumerable<StationDescription> DisplayStationList()
        {

            List<StationDescription> statList = new List<StationDescription>();
            foreach (var item in p.StationList())
            {
                StationDescription statD = new StationDescription();
                statD.Id = item.ID;
                statD.name = item.Name;
                foreach (var item2 in p.DroneChargeList())// full chargeSlots
                {
                    if (item2.StationId == item.ID) // if the drone is in the station
                    {
                        statD.fullChargeSlots++;

                    }

                }

                statD.freeChargeSlots = item.ChargeSlots;// free ones
                statList.Add(statD);// add it to the list

            }
            return statList;
        }
        #endregion

        #region DisplayDroneList
        public IEnumerable<DroneDescription> displayDroneList()
        {
            return DroneList;

        }
        #endregion

        #region DisplayClientList
        public IEnumerable<ClientActions> displayClientList()
        {
            List<ClientActions> LstCA = new List<ClientActions>();

            foreach (var item in p.ClientList())
            {
                ClientActions tempCA = new ClientActions();
                tempCA.Id = item.ID;
                tempCA.name = item.Name;
                tempCA.phone = item.Phone;
                IEnumerable<IDAL.DO.Parcel> sent_and_delivLst = p.ParcelList().Where(x => x.SenderId == item.ID && x.Delivered != DateTime.MinValue);
                IEnumerable<IDAL.DO.Parcel> sent_and_notDelivLst = p.ParcelList().Where(x => x.SenderId == item.ID && x.Delivered == DateTime.MinValue);
                IEnumerable<IDAL.DO.Parcel> receivLst = p.ParcelList().Where(x => x.TargetId == item.ID && x.Delivered != DateTime.MinValue);
                IEnumerable<IDAL.DO.Parcel> receivingLst = p.ParcelList().Where(x => x.TargetId == item.ID && x.Delivered == DateTime.Now);
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

        #region DisplayParcelList
        public IEnumerable<ParcelDescription> displayParcelList()
        {
            //id 
            //sendername
            //targetname
            //weight
            //time
            //matsav havila
            List<ParcelDescription> parList = new List<ParcelDescription>();

            foreach (var item in p.ParcelList())
            {
                ParcelDescription tempPar = new ParcelDescription();
                tempPar.Id = item.ID;
                try
                {
                    tempPar.SenderName = p.ClientById(item.SenderId).Name;
                    tempPar.TargetName = p.ClientById(item.TargetId).Name;
                }
                catch (IDAL.DO.ClientException ex)
                {
                    throw new IDNotFound("not found", ex);
                }
                tempPar.weight = (WeightCategories)item.Weight;
                tempPar.priority = (Priorities)item.Priority;
                if (item.Requested == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.requested;
                }
                else if (item.Scheduled == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.scheduled;
                }
                else if (item.PickedUp == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.pickedup;
                }
                else if (item.Delivered == DateTime.Now)
                {
                    tempPar.Status = ParcelStatus.delivered;
                }
                parList.Add(tempPar);
            }
            return parList;
        }
        #endregion
        public void printParcelsNotAssigned() { }
        public void printFreeStations()
        {
            List<StationDescription> statList = new List<StationDescription>();
            StationDescription tempStat = new StationDescription();
            foreach (var item in p.StationList())
            {
                if (item.ChargeSlots > 0)
                {
                    tempStat.Id = item.ID;
                    tempStat.name = item.Name;
                    tempStat.freeChargeSlots = item.ChargeSlots;
                    foreach (var item2 in p.DroneChargeList())// full chargeSlots
                    {
                        if (item2.StationId == item.ID)
                            tempStat.fullChargeSlots++;
                    }
                }
                statList.Add(tempStat);
            }

        }
        public IEnumerable<ParcelDescription> displayParcelsNotAssigned()
        {
            List<ParcelDescription> PDList = new List<ParcelDescription>();
            foreach (var item in p.ParcelList())
            {
                if (item.Scheduled == DateTime.Now)
                {
                    ParcelDescription tempPD = new ParcelDescription();
                    tempPD.Id = item.ID;
                    tempPD.SenderName = Name(item.SenderId);
                    tempPD.TargetName = Name(item.TargetId);
                    tempPD.weight = (WeightCategories)item.Weight;
                    if (item.Requested == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.requested;
                    }
                    else if (item.Scheduled == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.scheduled;
                    }
                    else if (item.PickedUp == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.pickedup;
                    }
                    else if (item.Delivered == DateTime.Now)
                    {
                        tempPD.Status = ParcelStatus.delivered;
                    }
                    PDList.Add(tempPD);
                }
            }
            return PDList;
        }
        #endregion

        #endregion




    }
}