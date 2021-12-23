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

        private ObservableCollection<BO.Station> boStationList = new ObservableCollection<BO.Station>();

        public StationListWindow()
        {
            InitializeComponent();
            DataContext = boStationList;
            foreach (var item in bl.DisplayStationList())
            {
                boStationList.Add(bl.displayStation((item.Id)));

            }
            
            stationListFromBo = bl.DisplayStationList();
            StationListView.ItemsSource = stationListFromBo;
        }

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
