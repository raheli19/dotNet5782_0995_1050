using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {


        //-----------------------------------CREATE-FUNCTIONS/ADD----------------------------------------


        #region addClient
        public void addClient(Client c)
        {

            if (DataSource.ClientList.Exists(client => client.ID == c.ID))
            {
                throw new ClientException($"id {c.ID} already exists!!");
            }
            DataSource.ClientList.Add(c);
        }
        #endregion
        //-----------------------------------UPDATE-FUNCTIONS-------------------------------------------

        #region UPDATING
        #region UpdateClient
        public void UpdateClient(Client ClientToUpdate)
        {
            Client myClient = new Client();
            myClient.ID = -1;
            myClient = DataSource.ClientList.Find(x => x.ID == ClientToUpdate.ID);

            if (myClient.ID == -1)
            {
                throw new ClientException("This Client doesn't exist in the system.");

            }
            DataSource.ClientList.Remove(myClient);
            myClient.ID = ClientToUpdate.ID;
            myClient.Name = ClientToUpdate.Name;
            myClient.Phone = ClientToUpdate.Phone;
            myClient.Latitude = ClientToUpdate.Latitude;
            myClient.Longitude = ClientToUpdate.Longitude;
            DataSource.ClientList.Add(myClient);
        }
        #endregion
        #endregion

        //-----------------------------------ACTIONS-------------------------------------------

        #region ClientById
        /// <summary>
        /// Receives an id and returns the client which contains this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client ClientById(int id)
        {
            Client cToReturn = default;
            if (!DataSource.ClientList.Exists(client => client.ID == id))
            {
                throw new ClientException($"id {id} doesn't exist!!");

            };
            cToReturn = DataSource.ClientList.Find(c => c.ID == id);
            return cToReturn;
        }
        #endregion


        #region IENUMERABLE

        #region IEClientList
        /// <summary>
        /// Returns the clients' list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> IEClientList()
        {
            List<Client> ClientLst = new List<Client>();
            ClientLst = DataSource.ClientList;
            return ClientLst;
        }
        #endregion
        #endregion

    }
}