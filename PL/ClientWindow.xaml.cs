﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private BLApi.IBL bl;
        BO.Client dataCclient = new BO.Client();
        private ClientActions clientActions = new ClientActions();
        ListView ListViewClient;
        private bool checkFlag = false;

        public ClientWindow(object selectedItem, BLApi.IBL bl, object ClientListWindow)
        {

            InitializeComponent();
            this.bl = bl;
            DataContext = dataCclient;
            ListViewClient = (ListView)ClientListWindow;
        }
        public ClientWindow(BLApi.IBL bl, object ClientListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCclient;
            //dlw = new DroneListWindow(bl);
            AddGridClient.Visibility = Visibility.Visible;
            UpgradeClientGrid.Visibility = Visibility.Hidden;
            //this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            //this.comboStationSelector.ItemsSource = bl.DisplayStationList();
           // ListViewClient = (ListView)ClientListWindow;

        }

        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(/*"The model of your drone is being updated.*/"Please close this window and enter the new name and/or new phone number.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            // autres en hidden/visible
            UpdateNameTextBox.Visibility = Visibility.Visible;
            UpdatePhoneTextBox.Visibility = Visibility.Visible;
            UpdateLabel.Visibility = Visibility.Visible;
            CheckUpdate.Visibility = Visibility.Visible;
            UpdateLabel.Content = "Enter the new name and/or new phone number";

        }

        private void Check_Click_Update(object sender, RoutedEventArgs e)
        {
            if (dataCclient.Name == "" && dataCclient.Phone == "")// didn't enter any information
            {
                MessageBox.Show("Please enter an information", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            try
            {
                bl.updateClientName_Phone(dataCclient.ID, dataCclient.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #region addClient
      



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (txt_id.Text =="" && dataCclient.Name ="" && dataCclient.Phone="")
            //MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);

            bl.addClient(dataCclient);
            //AddClient.Visibility = Visibility.Hidden;
        }

        #endregion

        #region closeFct
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.checkFlag = true; // will allow us to close the window from the button and not from the "X"
            this.Close();
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {

            if (this.checkFlag)// call from the button
                e.Cancel = false;
            else
                e.Cancel = true;// call from the "X", we don't want to close

        }
        #endregion

        private void txt_lat_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
