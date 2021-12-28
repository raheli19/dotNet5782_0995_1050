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
        IEnumerable<DroneDescription> droneListFromBo = new List<DroneDescription>();

        private ObservableCollection<BO.DroneDescription> boDroneList = new ObservableCollection<BO.DroneDescription>();

        public DroneListWindow()
        {

        }

        public DroneListWindow(BLApi.IBL bl)
        {

            InitializeComponent();
            DronesListView.DataContext = boDroneList;
            foreach (var item in bl.displayDroneList())
            {
                boDroneList.Add(item);


            }
            this.bl = bl;
            
            this.comboStatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));


            List<DroneDescription> dronesByWeight = new List<DroneDescription>();
            foreach (var item in boDroneList)
            {
                dronesByWeight.Add(new DroneDescription() { Id = item.Id, Model = item.Model, battery = item.battery, weight = item.weight,Status=item.Status,loc=item.loc, parcelId=item.parcelId });

            }
            List<DroneDescription> dronesBYWeight = new List<DroneDescription>();
            dronesBYWeight = dronesByWeight.OrderBy(d => d.weight).ToList();
            FilterByWeight.ItemsSource = dronesBYWeight;
            
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilterByWeight.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("weight");
            view.GroupDescriptions.Add(groupDescription);


            List<DroneDescription> dronesByStatus = new List<DroneDescription>();
            foreach (var item in boDroneList)
            {
                dronesByStatus.Add(new DroneDescription() { Id = item.Id, Model = item.Model, battery = item.battery, weight = item.weight, Status = item.Status, loc = item.loc, parcelId = item.parcelId });

            }
            List<DroneDescription> dronesBYStatus = new List<DroneDescription>();
            dronesBYStatus = dronesByStatus.OrderBy(d => d.Status).ToList();
            FilterByStatus.ItemsSource = dronesBYStatus;

            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(FilterByStatus.ItemsSource);
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("Status");
            view2.GroupDescriptions.Add(groupDescription2);

        }
        public static DroneStatuses droneStat = 0;
        public static WeightCategories weightStat = 0;
        public bool weightFlag = false;
        public bool statusFlag = false;
        //private object isDataDirty;
        private bool checkFlag = false;

        #region SelectStatut
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
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.Status == status && x.weight == weightStat);
            else
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.Status == status);
        }

        #endregion




        #region WeightSelector
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
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.weight == weight && x.Status == droneStat);
            else
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.weight == weight);

        }

        public void CheckFields()
        {
            if (weightFlag && statusFlag)
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.Status == status && x.weight == weightStat);
            else if (statusFlag)
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.Status == status);
            else if (weightFlag)
                this.DronesListView.ItemsSource = droneListFromBo.Where(x => x.weight == weight && x.Status == droneStat);
            else
                this.DronesListView.ItemsSource = droneListFromBo;
        }
        #endregion

        #region AddDroneOpenWindow
        private void button_addDrone(object sender, RoutedEventArgs e)
        {
            DroneWindow subWindow = new DroneWindow(bl, DronesListView);

            subWindow.ShowDialog();
            subWindow.Hide();
            //comboStatusSelector_SelectionChanged(comboStatusSelector, new SelectionChangedEventArgs());
            FilterStatus();
            FilterWeight();

        }
        #endregion

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(DronesListView.SelectedItem, bl, DronesListView).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }


        private void FilterByWeight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(FilterByWeight.SelectedItem, bl, DronesListView).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }

        private void FilterByStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(FilterByWeight.SelectedItem, bl, DronesListView).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }
        #region closeFunctions
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

        private void ClearStatus_Click(object sender, RoutedEventArgs e)
        {
            comboStatusSelector.SelectedItem = null;
            weightFlag = false;
            weightStat = 0;
            DronesListView.ItemsSource = droneListFromBo;

        }

        private void ClearWeight_Click(object sender, RoutedEventArgs e)
        {
            comboWeightSelector.SelectedItem = null;
            droneStat = 0;
            statusFlag = false;
            DronesListView.ItemsSource = droneListFromBo;

        }

     
    }
}

