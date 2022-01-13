// double clique ne marche pas
//add avec la photo



using System;
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
using System.Collections.ObjectModel;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {

        private BLApi.IBL bl;
        DroneStatuses status = new DroneStatuses();
        WeightCategories weight = new WeightCategories();
        public static DroneStatuses droneStat = 0;
        public static WeightCategories weightStat = 0;
        public bool weightFlag = false;
        public bool statusFlag = false;
        //private object isDataDirty;
        private bool checkFlag = false;

        private ObservableCollection<BO.DroneDescription> boDroneList = new ObservableCollection<BO.DroneDescription>();

        #region EmptyCtor
        public DroneListWindow()
        {

        }
        #endregion

        #region Ctor
        public DroneListWindow(BLApi.IBL bl)
        {

            InitializeComponent();

            foreach (var item in bl.displayDroneList())
            {
                boDroneList.Add(item);


            }
            this.bl = bl;
            DronesListView.DataContext = boDroneList;
            DataContext = boDroneList;

            this.comboStatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        #endregion

        #region SelectStatut
        /// <summary>
        /// Functions to filter by status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboStatusSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            FilterStatus();
        }
        private void FilterStatus()
        {
            if (comboStatusSelector.SelectedItem == null)
            {
                return;
            }
            /*DroneStatuses*/
            status = (DroneStatuses)comboStatusSelector.SelectedItem;
            droneStat = status;
            statusFlag = true;
            if (weightFlag)
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.Status == status && x.weight == weightStat);
            else
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.Status == status);
        }

        #endregion

        #region WeightSelector
        /// <summary>
        /// Functions to filter by weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboWeightSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            FilterWeight();
        }
        private void FilterWeight()
        {
            if (comboWeightSelector.SelectedItem == null)
            {
                return;
            }
            /*WeightCategories*/
            weight = (WeightCategories)comboWeightSelector.SelectedItem;
            weightStat = weight;
            weightFlag = true;
            if (statusFlag)
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.weight == weight && x.Status == droneStat);
            else
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.weight == weight);

        }

        public void CheckFields()
        {
            if (weightFlag && statusFlag)
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.Status == status && x.weight == weightStat);
            else if (statusFlag)
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.Status == status);
            else if (weightFlag)
                this.DronesListView.ItemsSource = bl.displayDroneList().Where(x => x.weight == weight && x.Status == droneStat);
            else
                this.DronesListView.ItemsSource = bl.displayDroneList();
        }
        #endregion

        #region AddDroneOpenWindow
        /// <summary>
        /// Opens drone window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_addDrone(object sender, RoutedEventArgs e)
        {
            DroneWindow subWindow = new DroneWindow(bl, DronesListView);

            subWindow.ShowDialog();
            subWindow.Hide();
            FilterStatus();
            FilterWeight();

        }
        #endregion

        #region DoubleClicks
        /// <summary>
        /// Opens drone window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(DronesListView.SelectedItem, bl, DronesListView, default, default).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }


        private void FilterByWeight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(FilterByWeight.SelectedItem, bl, DronesListView, FilterByWeight, default).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }

        private void FilterByStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(FilterByStatus.SelectedItem, bl, DronesListView, default, FilterByStatus).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }

        #endregion

        #region closeFunctions

        /// <summary>
        /// Functions to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region ClearClick
        /// <summary>
        /// Functions to clear the status and /or the weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearStatus_Click(object sender, RoutedEventArgs e)
        {
            comboStatusSelector.SelectedItem = null;
            weightFlag = false;
            weightStat = 0;
            DronesListView.ItemsSource = bl.displayDroneList();

        }

        private void ClearWeight_Click(object sender, RoutedEventArgs e)
        {
            comboWeightSelector.SelectedItem = null;
            droneStat = 0;
            statusFlag = false;
            DronesListView.ItemsSource = bl.displayDroneList();

        }

        private void clearGrouping_Click(object sender, RoutedEventArgs e)
        {
            FilterByWeight.Visibility = Visibility.Hidden;
            FilterByStatus.Visibility = Visibility.Hidden;
            DronesListView.ItemsSource = bl.displayDroneList();
            DronesListView.Visibility = Visibility.Visible;
        }

        #endregion

        #region FilterClick
        /// <summary>
        /// Functions to filter by weight and status (Grouping)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterByWeightButton_Click(object sender, RoutedEventArgs e)
        {
            List<DroneDescription> dronesByWeight = new List<DroneDescription>();
            foreach (var item in bl.displayDroneList())
            {
                dronesByWeight.Add(new DroneDescription() { Id = item.Id, Model = item.Model, battery = item.battery, weight = item.weight, Status = item.Status, loc = item.loc, parcelId = item.parcelId });

            }
            List<DroneDescription> dronesBYWeight = new List<DroneDescription>();
            dronesBYWeight = dronesByWeight.OrderBy(d => d.weight).ToList();
            FilterByWeight.ItemsSource = dronesBYWeight;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilterByWeight.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("weight");
            view.GroupDescriptions.Add(groupDescription);

            FilterByWeight.Visibility = Visibility.Visible;
            FilterByStatus.Visibility = Visibility.Hidden;
            DronesListView.Visibility = Visibility.Hidden;
        }

        private void FilterByStatusButton_Click(object sender, RoutedEventArgs e)
        {
            List<DroneDescription> dronesByStatus = new List<DroneDescription>();
            foreach (var item in bl.displayDroneList())
            {
                dronesByStatus.Add(new DroneDescription() { Id = item.Id, Model = item.Model, battery = item.battery, weight = item.weight, Status = item.Status, loc = item.loc, parcelId = item.parcelId });

            }
            List<DroneDescription> dronesBYStatus = new List<DroneDescription>();
            dronesBYStatus = dronesByStatus.OrderBy(d => d.Status).ToList();
            FilterByStatus.ItemsSource = dronesBYStatus;

            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(FilterByStatus.ItemsSource);
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("Status");
            view2.GroupDescriptions.Add(groupDescription2);
            FilterByWeight.Visibility = Visibility.Hidden;
            FilterByStatus.Visibility = Visibility.Visible;
            DronesListView.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}

