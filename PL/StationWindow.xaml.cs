using System;
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
    /// Logique d'interaction pour StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window

    {
        string newName = "";
        private BLApi.IBL bl; 
        BO.Station dataCstation = new BO.Station();
        private StationDescription stationDescription = new StationDescription();
        IEnumerable<DroneCharging> droneListFromBo = new List<DroneCharging>();
        ListView ListViewStation;
        // -------------------------------------------------------UPGRADE------------------------------------------------------------------------------

        #region constructorUPGRADE
        //ctor to upgrade the station
        public StationWindow(object selectedItem, BLApi.IBL bl, object stationListView)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCstation;
            dataCstation.Loc = new();
            this.stationDescription = (StationDescription)selectedItem;
            AddStationGrid.Visibility = Visibility.Hidden;
            UpdateStationGrid.Visibility = Visibility.Visible;
            stationDetails.Content = bl.displayStation(stationDescription.Id);
            droneListFromBo = bl.displayDroneChargingList(stationDescription.Id);
            stationDetails.Visibility = Visibility.Visible;
            ListViewStation = (ListView)stationListView;
            DronesChargingListView.ItemsSource = droneListFromBo;

        }
        #endregion

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneCharging DCh = (DroneCharging)DronesChargingListView.SelectedItem;
            DroneDescription DC = bl.displayDroneList().First(x => x.Id == DCh.ID);
            new DroneWindow(DC, bl, DronesChargingListView).Show();
            //comboStatusSelector.SelectedItem = null;
            //comboWeightSelector.SelectedItem = null;
        }

        // ctor to add a station
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
                if(ex.Message== "latitude is not valid")
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

        private void ClickUpdate(object sender, RoutedEventArgs e)
        {  MessageBox.Show(/*"The model of your drone is being updated.*/"Please close this window and enter the new name.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateTextBox.Visibility = Visibility.Visible;
                UpdateLabel.Visibility = Visibility.Visible;
                CheckUpdate.Visibility = Visibility.Visible;


                UpdateLabel.Content = "Enter the new name:";

        }
        private void Check_Click_Update(object sender, RoutedEventArgs e)
        {
            newName = UpdateTextBox.Text;

            if (stationDescription.name == "")
            {
                MessageBox.Show("Please enter a name", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            try
            {
                bl.updateStationName_CS(stationDescription.Id, newName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ListViewStation.ItemsSource = bl.DisplayStationList();
            stationDetails.Content = bl.displayStation(stationDescription.Id);
            UpdateTextBox.Visibility = Visibility.Hidden;
            UpdateLabel.Visibility = Visibility.Hidden;
            CheckUpdate.Visibility = Visibility.Hidden;
        }
    }
}
