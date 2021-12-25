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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        private BLApi.IBL bl;
        IEnumerable<StationDescription> stationListFromBo = new List<StationDescription>();
        BO.Station dataStation = new BO.Station();
        private StationDescription statDescp = new StationDescription();
        ListView ListViewStation;
        private ObservableCollection<BO.Station> boStationList = new ObservableCollection<BO.Station>();
        public StationListWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            stationListFromBo = bl.DisplayStationList();
            DataContext = boStationList;
            foreach (var item in bl.DisplayStationList())
            {
                boStationList.Add(bl.displayStation((item.Id)));


            }
           ListViewStation.ItemsSource = stationListFromBo;
        }
        #region station
        //public StationListWindow(object selectedItem, BLApi.IBL bl, object StationListWindow)
        //{
        //    InitializeComponent();
        //    this.bl = bl;
        //    DataContext = dataStation;
        //    //foreach (var item in bl.DisplayStationList())
        //    //{
        //    //    boStationList.Add(bl.displayStation((item.Id)));

        //    //}

        //    //stationListFromBo = bl.DisplayStationList();
        //    //StationListView.ItemsSource = stationListFromBo;
        //}

        //public StationListWindow(BLApi.IBL bl, object StationListWindow)
        //{
        //    InitializeComponent();
        //    this.bl = bl;
        //    DataContext = dataStation;
        //    //dlw = new DroneListWindow(bl);
        //    //AddGrid.Visibility = Visibility.Visible;
        //    //UpgradeClientGrid.Visibility = Visibility.Hidden;
        //    //this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        //    //this.comboStationSelector.ItemsSource = bl.DisplayStationList();
        //    // ListViewClient = (ListView)ClientListWindow;

        //}
        #endregion
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
