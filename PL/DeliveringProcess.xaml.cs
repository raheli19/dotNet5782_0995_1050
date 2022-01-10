﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLApi;
using BO;
namespace PL
{
    /// <summary>
    /// Logique d'interaction pour DeliveringProcess.xaml
    /// </summary>



    public partial class DeliveringProcess : Window
    {
        private BLApi.IBL bl;
        BO.Client DataCclient = new BO.Client();
        string clientStatus;
        BO.Parcel DataCParcel = new BO.Parcel();

        #region ctor
        public DeliveringProcess(BLApi.IBL bl, BO.Client DataCclient, string clientStatus)
        {
            InitializeComponent();
            this.MaxWeightL.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            this.PriorityL.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            this.Combo_TargetId.ItemsSource = bl.AllTargets_Id();
            DataContext = DataCParcel;
            DataCParcel.Sender = new();
            DataCParcel.Target = new();
            DataCParcel.Sender.ID = DataCclient.ID;
            DataCParcel.Sender.name = DataCclient.Name;
            DataCParcel.ID = Dal.Configuration.RunnerIDnumber;
            this.bl = bl;
            this.DataCclient = DataCclient;
            this.clientStatus = clientStatus;
        }
        #endregion


        #region AllButtonsClick
        private void ParcelsFromClient_Click(object sender, RoutedEventArgs e)
        {
            if (clientStatus == "Sign Up")   //Join
            {
                MessageBox.Show("Aie aie aie...", "You just joined our delivering system.You didn't send any parcel yet. ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (clientStatus == "Sign In")
            {
                string parcelsFromClientList = bl.parcelsToCliList(DataCclient.ID); //
                parcelsFromClient.Visibility = Visibility.Visible;
                parcelsFromClient.Content = parcelsFromClientList;


            }
        }

        private void ParcelsToClient_Click(object sender, RoutedEventArgs e)
        {
            // enters the number of the parcel // fromthe list
            //calls the function delivered

            if (clientStatus == "Sign Up")   //Join
            {
                MessageBox.Show("Aie aie aie...", "You just joined our delivering system.There is no parcels for you. ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (clientStatus == "Sign In")
            {
                string parcelsToClientList = bl.parcelsFromCLiList(DataCclient.ID);
                parcelsToClient.Visibility = Visibility.Visible;
                parcelsToClient.Content = parcelsToClientList;
            }
        }

        private void SendNewParcel_Click(object sender, RoutedEventArgs e)
        {
            ////senderID.Background = Brushes.White;
            Combo_TargetId.Background = Brushes.White;
            //Labels
            //SenderId.Visibility = Visibility.Visible;
            TargetId.Visibility = Visibility.Visible;
            MaxWeight.Visibility = Visibility.Visible;
            Priority.Visibility = Visibility.Visible;
            //textboxs and comboBox
            //senderID.Visibility = Visibility.Visible;
            Combo_TargetId.Visibility = Visibility.Visible;
            MaxWeightL.Visibility = Visibility.Visible;
            PriorityL.Visibility = Visibility.Visible;
            Add_Parcel_ToSend.Visibility = Visibility.Visible;



            try
            {
                bl.addParcel(DataCParcel);
            }
            catch (Exception ex)
            {
                if (ex.Message == "TargetID not valid")
                {
                    Combo_TargetId.Background = Brushes.Red;
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (ex.Message == "This package already exists")
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void AssignParcelClick(object sender, RoutedEventArgs e)
        {

        }

        private void received_parcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel_Id_Entered.Visibility = Visibility.Visible;
            label_info.Content = "Please enter its Id";
            label_info.Visibility = Visibility.Visible;
            BO.DroneDescription delDrone = new DroneDescription();
            delDrone = bl.displayDroneList().First(x => x.parcelId == DataCParcel.ID);
            try
            {
                bl.delivered(delDrone.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aie aie aie...", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }
        }

        #endregion

        

        

        #region checkPickedUp
        private void Picked_up(object sender, RoutedEventArgs e)
        {
            //affiche liste des parcels qu'ils envoit
            List<ParcelDescription> tempList = new List<ParcelDescription>();
            DataCParcel.Drone = new DroneWithParcel();
            tempList = bl.displayParcelList().Where(x => x.SenderName == DataCclient.Name).ToList();
            listOf_Parcels.Visibility = Visibility.Visible;
            label_info.Visibility = Visibility.Visible;
            Parcel_Id_Entered.Visibility = Visibility.Visible;
            // Drone_Id_entered.Visibility = Visibility.Visible;
            click_PickedUpButton.Visibility = Visibility.Visible;
            listOf_Parcels.Content = tempList.ToString(); //doesn't work to print the list!!!!


            //DataCParcel.ID = Convert.ToInt32(Parcel_Id_Entered.Text);
            //DataCParcel.Drone.ID = bl.displayDroneList().First(x => x.parcelId == DataCParcel.ID).Id;

            //selectionne une
            //met comme quoi il ont pîcked up
        }

        private void click_PickedUp_Click(object sender, RoutedEventArgs e)
        {
            //enter the id of the parcel from the list
            // supposed to go into th elist of the parcel, find it, and assign in the datacontext the id of the associated' drone!
            //DataCParcel.ID = Convert.ToInt32(Parcel_Id_Entered.Text);
            //DataCParcel.Drone.ID = Convert.ToInt32(Drone_Id_entered.Text);
            try
            {
                DataCParcel.Drone.ID = bl.displayDroneList().First(x => x.parcelId == DataCParcel.ID).Id;

                bl.PickedUp(DataCParcel.Drone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aie aie aie...", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        #endregion

        #region CloseInfos
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            listOf_Parcels.Visibility = Visibility.Hidden;
            label_info.Visibility = Visibility.Hidden;
            Parcel_Id_Entered.Visibility = Visibility.Hidden;
            click_PickedUpButton.Visibility = Visibility.Hidden;
            TargetId.Visibility = Visibility.Hidden;
            MaxWeight.Visibility = Visibility.Hidden;
            Priority.Visibility = Visibility.Hidden;
            //textboxs and comboBox
            Combo_TargetId.Visibility = Visibility.Hidden;
            MaxWeightL.Visibility = Visibility.Hidden;
            PriorityL.Visibility = Visibility.Hidden;
            parcelsToClient.Visibility = Visibility.Hidden;
            parcelsFromClient.Visibility = Visibility.Hidden;
            Add_Parcel_ToSend.Visibility = Visibility.Hidden;

        }
        #endregion

        private void Combo_TargetId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataCParcel.Target.ID = (int)Combo_TargetId.SelectedItem;
            DataCParcel.Target.name = bl.displayClient(DataCParcel.Target.ID).Name;

        }

        private void Add_Parcel_ToSend_Click(object sender, RoutedEventArgs e)
        {
            if (MaxWeightL==null || MaxWeightL == null || Combo_TargetId == null)
            {
                MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                bl.addParcel(DataCParcel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
        }
    }
}
