using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        private BLApi.IBL bl;
        IEnumerable<StationDescription> stationListFromBo = new List<StationDescription>();
        public ObservableCollection<BO.StationDescription> boStationList = new ObservableCollection<BO.StationDescription>();
        private bool checkFlag = false;

        #region CTOR
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="bl"></param>
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
        #endregion

        #region StationListView_MouseDoubleClick
        /// <summary>
        /// open the window of a station ou doubled click on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(StationListView.SelectedItem, bl, StationListView).Show();
        }
        #endregion

        #region GotOrNotAvailableChargeSlots_MouseDoubleClick
        /// <summary>
        /// opens a station window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GotOrNotAvailableChargeSlots_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(GotOrNotAvailableChargeSlots.SelectedItem, bl, StationListView).Show();

        }
        #endregion

        #region CountAvailableChargeSlots_MouseDoubleClick
        /// <summary>
        /// open station window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountAvailableChargeSlots_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(CountAvailableChargeSlots.SelectedItem, bl, StationListView).Show();

        }
        #endregion

        #region ContainsFreeChargeSlots_Click
        /// <summary>
        /// filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void containsFreeChargeSlots_Click(object sender, RoutedEventArgs e)
        {
            this.StationListView.ItemsSource = boStationList.Where(x => x.freeChargeSlots > 0);
        }
        #endregion

        #region NumFCS_TextChanged
        /// <summary>
        /// filter by amount of free charge slots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numFCS_TextChanged(object sender, TextChangedEventArgs e)
        {
            string numFCSstr = numFCS.Text;
            if (numFCSstr == "")
            {
                return;
            }
            int num = int.Parse(numFCSstr);
            if (stationListFromBo.Where(x => x.freeChargeSlots == num) != null)

            { this.StationListView.ItemsSource = boStationList.Where(x => x.freeChargeSlots == num); }
            else
                MessageBox.Show("Input not valid.Please press the button clear", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        #endregion

        #region Button_addStation
        /// <summary>
        /// addstation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_addStation(object sender, RoutedEventArgs e)
        {
            StationWindow subWindow = new StationWindow(bl, StationListView);
            subWindow.ShowDialog();
            subWindow.Hide();


        }
        #endregion

        #region ClearNumFCS_Click
        private void clearNumFCS_Click(object sender, RoutedEventArgs e)
        {
            numFCS.Text = "";
            this.StationListView.ItemsSource = boStationList;
        }
        #endregion

        #region Button_Click
        /// <summary>
        /// grouping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<StationDescription> listByFree = new List<StationDescription>();
            foreach (var item in bl.DisplayStationList())
            {
                listByFree.Add(new StationDescription() { Id = item.Id, name = item.name, freeChargeSlots = item.freeChargeSlots, fullChargeSlots = item.fullChargeSlots });

            }
            GotOrNotAvailableChargeSlots.ItemsSource = listByFree;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(GotOrNotAvailableChargeSlots.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("freeornot");
            view.GroupDescriptions.Add(groupDescription);
            GotOrNotAvailableChargeSlots.Visibility = Visibility.Visible;
            CountAvailableChargeSlots.Visibility = Visibility.Hidden;
            StationListView.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Button1_Click
        /// <summary>
        /// grouping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            List<StationDescription> listByNumFree = new List<StationDescription>();
            foreach (var item in bl.DisplayStationList())
            {
                listByNumFree.Add(new StationDescription() { Id = item.Id, name = item.name, freeChargeSlots = item.freeChargeSlots, fullChargeSlots = item.fullChargeSlots });

            }
            List<StationDescription> listByCount = new List<StationDescription>();
            listByCount = listByNumFree.OrderBy(d => d.freeChargeSlots).ToList();
            CountAvailableChargeSlots.ItemsSource = listByCount;

            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(CountAvailableChargeSlots.ItemsSource);
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("CS");
            view2.GroupDescriptions.Add(groupDescription2);
            CountAvailableChargeSlots.Visibility = Visibility.Visible;
            GotOrNotAvailableChargeSlots.Visibility = Visibility.Hidden;
            StationListView.Visibility = Visibility.Hidden;
        }
        #endregion

        #region ClearGrouping_Click
        /// <summary>
        /// clear grouping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearGrouping_Click(object sender, RoutedEventArgs e)
        {
            CountAvailableChargeSlots.Visibility = Visibility.Hidden;
            GotOrNotAvailableChargeSlots.Visibility = Visibility.Hidden;
            StationListView.Visibility = Visibility.Visible;
        }
        #endregion

        #region Close_Click
        /// <summary>
        /// This button closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.checkFlag = true;
            this.Close();
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {

            if (this.checkFlag)// call from the button
                e.Cancel = false;
            else
                e.Cancel = true;// call from the "X", we don't want to close

        }
        private void ClickCloseDroneWindow(object sender, RoutedEventArgs e)
        {
            this.checkFlag = true; // will allow us to close the window from the button and not from the "X"

            this.Close();
            //    MessageBox.Show("Your drone is fully charged. We are going to unplug it", "Success!");
        }



        #endregion

    }
}
