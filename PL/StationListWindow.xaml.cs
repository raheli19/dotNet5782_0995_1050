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
        public ObservableCollection<BO.StationDescription> boStationList = new ObservableCollection<BO.StationDescription>();


        public StationListWindow(BLApi.IBL bl)
        {

            InitializeComponent();
            StationListView.DataContext = boStationList;
            DataContext = boStationList;
            foreach (var item in bl.DisplayStationList())
            {
                boStationList.Add(item);


            }
            this.bl = bl;
            //stationListFromBo = bl.DisplayStationList();
            
            //this.comboStatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            //this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(StationListView.SelectedItem, bl, StationListView).Show();
            //comboStatusSelector.SelectedItem = null;
            //comboWeightSelector.SelectedItem = null;
        }


        private void containsFreeChargeSlots_Click(object sender, RoutedEventArgs e)
        {
              this.StationListView.ItemsSource = stationListFromBo.Where(x => x.freeChargeSlots>0);
        }

        private void numFCS_TextChanged(object sender, TextChangedEventArgs e)
        {
            string numFCSstr = numFCS.Text;
            if (numFCSstr == "")
            {
                return;
            }
            int num = int.Parse(numFCSstr);
            if (stationListFromBo.Where(x => x.freeChargeSlots == num) != null)

            { this.StationListView.ItemsSource = stationListFromBo.Where(x => x.freeChargeSlots == num); }
            else
                MessageBox.Show("Input not valid.Please press the button clear", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_addStation(object sender, RoutedEventArgs e)
        {
                StationWindow subWindow = new StationWindow(bl, StationListView);
                subWindow.ShowDialog();
                subWindow.Hide();
                

        }

        private void clearNumFCS_Click(object sender, RoutedEventArgs e)
        {
            numFCS.Text = "";
            this.StationListView.ItemsSource = stationListFromBo;
        }
    }
}
