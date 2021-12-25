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
        ListView ListViewStation;
        // -------------------------------------------------------UPGRADE------------------------------------------------------------------------------

        #region constructorUPGRADE
        //ctor to upgrade the station
        public StationWindow(object selectedItem, BLApi.IBL bl, object stationListView)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCstation;
            this.stationDescription = (StationDescription)selectedItem;
            AddStationGrid.Visibility = Visibility.Hidden;
            UpdateStationGrid.Visibility = Visibility.Visible;
            //Drone_Label.Content = bl.displayDrone(droneDescription.Id);
            //Drones_Details.Visibility = Visibility.Visible;
            ListViewStation = (ListView)stationListView;

        }
        #endregion

        // ctor to add a drone
        public StationWindow(BLApi.IBL bl, object DroneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCstation;
            //dlw = new DroneListWindow(bl);
            AddStationGrid.Visibility = Visibility.Visible;
            UpdateStationGrid.Visibility = Visibility.Hidden;
            ListViewStation = (ListView)DroneListWindow;

        }


    }
}
