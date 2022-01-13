using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window

    {
        string newName = "n";
        int newCS = -1;
        private BLApi.IBL bl; 
        BO.Station dataCstation = new BO.Station();
        StationDescription dataCstationUpdate = new StationDescription();
        //private StationDescription stationDescription = new StationDescription();
        IEnumerable<DroneCharging> droneListFromBo = new List<DroneCharging>();
        public ObservableCollection<BO.DroneCharging> boDronesCharging = new ObservableCollection<BO.DroneCharging>();
        ListView ListViewStation;
        private ObservableCollection<BO.DroneDescription> boDroneList = new ObservableCollection<BO.DroneDescription>();

        // -------------------------------------------------------ADD------------------------------------------------------------------------------
        // ctor to add a station
        #region CTOR:AddStation
        public StationWindow(BLApi.IBL bl, object DroneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCstation;
            dataCstation.Loc = new();
            //dlw = new DroneListWindow(bl);
            AddStationGrid.Visibility = Visibility.Visible;
            UpdateStationGrid.Visibility = Visibility.Hidden;
            ListViewStation = (ListView)DroneListWindow;

        }
        #endregion

        #region EnterTab
        // allows us to do enter => goes to the next texbox
        private void id_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_name.Focus();
            }
        }

        private void Add_enter(object sender, KeyEventArgs e)
        {
            Add_button(sender, e);
        }
        private void name_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_CS.Focus();
            }
        }

        private void CS_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_lat.Focus();
            }
        }

        private void lat_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_long.Focus();
            }
        }
        private void long_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ADD.Focus();
                
            }
        }

        #endregion

        #region Add_button
        /// <summary>
        /// add a station to the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_button(object sender, RoutedEventArgs e)
        {

            txt_id.Background = Brushes.White;
            txt_lat.Background = Brushes.White;
            txt_long.Background = Brushes.White;
            txt_CS.Background = Brushes.White;
            //if (txt_id.Text =="" && dataCclient.Name ="" && dataCclient.Phone="")
            //MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (txt_id.Text == "" || txt_name.Text == "" || txt_lat.Text == "" || txt_long.Text == "" || txt_CS.Text == "")
            {
                MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string stationIdCheck = txt_id.Text;// to check if it's an integer
            int stationIdInt;
            string CSCheck = txt_CS.Text;
            int stationCSCheck;
            if (!int.TryParse(stationIdCheck, out stationIdInt))
            {
                txt_id.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Id", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(CSCheck, out stationCSCheck))
            {
                txt_CS.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Number", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                bl.addStation(dataCstation);
            }
            catch (Exception ex)
            {
                if (ex.Message == "ID not valid")
                    txt_id.Background = Brushes.Red;
                if (ex.Message == "latitude is not valid")
                    txt_lat.Background = Brushes.Red;
                if (ex.Message == "longitude is not valid")
                    txt_long.Background = Brushes.Red;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            MessageBox.Show("You're added to our system! Welcome!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            ListViewStation.ItemsSource = bl.DisplayStationList();
            //dlw.CheckFields();
            this.Close();
        }
        #endregion

        // -------------------------------------------------------UPGRADE------------------------------------------------------------------------------

        //ctor to upgrade the station
        #region CTRO:UpdateStation

        public StationWindow(object selectedItem, BLApi.IBL bl, object stationListView)
        {
            InitializeComponent();
            this.bl = bl;
            dataCstationUpdate = (StationDescription)selectedItem;
            DataContext = dataCstationUpdate;
            dataCstation.Loc = new();
            //this.stationDescription = (StationDescription)selectedItem;
            AddStationGrid.Visibility = Visibility.Hidden;
            UpdateStationGrid.Visibility = Visibility.Visible;
            stationDetails.Content = bl.displayStation(dataCstationUpdate.Id);
            //droneListFromBo = bl.displayDroneChargingList(stationDescription.Id);
            stationDetails.Visibility = Visibility.Visible;
            ListViewStation = (ListView)stationListView;
            DronesChargingListView.DataContext = boDronesCharging;
            foreach(var item in bl.displayDroneChargingList(dataCstationUpdate.Id))
            {
                boDronesCharging.Add(item);
            }
            DronesListView.DataContext = boDroneList;
            DataContext = boDroneList;
            foreach (var item in bl.displayDroneList())
            {
                boDroneList.Add(item);


            }
            //boDronesCharging= (ObservableCollection<DroneCharging>)bl.displayDroneChargingList(stationDescription.Id);

        }
        #endregion

        #region StationListView_MouseDoubleClick
        /// <summary>
        /// opens the details of the drone 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesChargingListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DroneCharging DCh = (DroneCharging)DronesChargingListView.SelectedItem;
            DroneDescription DC = bl.displayDroneList().First(x => x.Id == DCh.ID);
            new DroneWindow(DC, bl, DronesListView,default,default).Show();
            //comboStatusSelector.SelectedItem = null;
            //comboWeightSelector.SelectedItem = null;
        }
        #endregion

        #region ClickUpdate
        /// <summary>
        /// updates the station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {  MessageBox.Show(/*"The model of your drone is being updated.*/"Please close this window and enter the new name and/or new number of charge slots.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateNameTextBox.Visibility = Visibility.Visible;
                UpdateNameLabel.Visibility = Visibility.Visible;
                CheckNameUpdate.Visibility = Visibility.Visible;
            UpdateCSTextBox.Visibility = Visibility.Visible;
            UpdateCSLabel.Visibility = Visibility.Visible;
            CheckCSUpdate.Visibility = Visibility.Visible;

            UpdateNameLabel.Content = "Enter the new name:";
            UpdateCSLabel.Content = "Enter the new number of charge slots";

        }
        #endregion

        #region Check_Click_Update
        /// <summary>
        /// update the station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_Click_Update(object sender, RoutedEventArgs e)
        {
            if (UpdateNameTextBox.Text != "")
                newName = UpdateNameTextBox.Text;
            else
                newName = "n";
           
        
            try
            {
                bl.updateStationName_CS(dataCstationUpdate.Id, newName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ListViewStation.ItemsSource = bl.DisplayStationList();
            stationDetails.Content = bl.displayStation(dataCstationUpdate.Id);
            UpdateNameTextBox.Visibility = Visibility.Hidden;
            UpdateNameLabel.Visibility = Visibility.Hidden;
            CheckNameUpdate.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Check_Click_UpdateCS
        /// <summary>
        /// updates num of free charge slots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Check_Click_UpdateCS(object sender, RoutedEventArgs e)
        {
            if (UpdateCSTextBox.Text != "")
                newCS = int.Parse(UpdateCSTextBox.Text);
            else
                newCS = -1;
            try
            {
                bl.updateStationName_CS(dataCstationUpdate.Id, "n",newCS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ListViewStation.ItemsSource = bl.DisplayStationList();
            stationDetails.Content = bl.displayStation(dataCstationUpdate.Id);
            UpdateCSTextBox.Visibility = Visibility.Hidden;
            UpdateCSLabel.Visibility = Visibility.Hidden;
            CheckCSUpdate.Visibility = Visibility.Hidden;
        }

        #endregion

        
    }
}
