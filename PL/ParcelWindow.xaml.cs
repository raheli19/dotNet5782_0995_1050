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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        #region ctor
        private BLApi.IBL bl;
        BO.Parcel dataCparcel = new BO.Parcel();
        BO.ParcelDescription dataCparcelUpdate = new BO.ParcelDescription();
        private ParcelDescription parcelDescription = new ParcelDescription();
        //DroneListWindow dlw;
        ListView ListViewParcel;
        private bool checkFlag = false;
        private ObservableCollection<BO.ClientActions> boClientList = new ObservableCollection<BO.ClientActions>();
        private ObservableCollection<BO.DroneDescription> boDroneList = new ObservableCollection<BO.DroneDescription>();
        #endregion

        #region ADDPARCEL
        /// <summary>
        /// opens the add parcel grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="ParcelListWindow"></param>
        public ParcelWindow(BLApi.IBL bl, object ParcelListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            dataCparcel.Sender = new ClientInParcel();
            dataCparcel.Target = new ClientInParcel();
            dataCparcel.Weight = new WeightCategories();
            dataCparcel.Priority = new Priorities();
            DataContext = dataCparcel;


            //dlw = new DroneListWindow(bl);
            AddGrid.Visibility = Visibility.Visible;
            UpgradeGrid.Visibility = Visibility.Hidden;
            this.Combo_SenderId.ItemsSource = bl.AllSenders_Id();
            this.Combo_TargetId.ItemsSource = bl.AllTargets_Id();
            this.Combo_Weight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            this.Combo_Priority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            //this.comboStationSelector.ItemsSource = bl.DisplayStationList();
            ListViewParcel = (ListView)ParcelListWindow;

        }

        /// <summary>
        /// add the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {

            if (Combo_SenderId == null || Combo_TargetId == null || Combo_Weight == null || Combo_Priority == null)
            {
                MessageBox.Show("Please make a selection", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Combo_SenderId.SelectedItem.Equals(Combo_TargetId.SelectedItem))
            {
                MessageBox.Show("A client can't send a parcel to himself! ", "Aie aie aie...", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                bl.addParcel(dataCparcel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Added the parcel", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            ListViewParcel.ItemsSource = bl.displayParcelList();
            this.Combo_SenderId.ItemsSource = bl.AllSenders_Id();
            this.Combo_TargetId.ItemsSource = bl.AllTargets_Id();

        }

        #region comboboxSelections
        /// <summary>
        /// slect a sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_SenderId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataCparcel.Sender.ID = (int)Combo_SenderId.SelectedItem;
            dataCparcel.Sender.name = bl.displayClient(dataCparcel.Sender.ID).Name;

        }
        /// <summary>
        /// select a target
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_TargetId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataCparcel.Target.ID = (int)Combo_TargetId.SelectedItem;
            dataCparcel.Target.name = bl.displayClient(dataCparcel.Target.ID).Name;

        }
        /// <summary>
        /// select a priority
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataCparcel.Priority = (Priorities)Combo_Priority.SelectedItem;
        }
        
        /// <summary>
        /// select a weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_Weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataCparcel.Weight = (WeightCategories)Combo_Weight.SelectedItem;
        }
        #endregion

        #endregion

        #region UPDATE
        /// <summary>
        /// open the update grid
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="bl"></param>
        /// <param name="parcelListView"></param>
        public ParcelWindow(object selectedItem, BLApi.IBL bl, object parcelListView)
        {
            InitializeComponent();
            this.bl = bl;
            this.dataCparcelUpdate = (ParcelDescription)selectedItem;
            DataContext = dataCparcelUpdate;
            AddGrid.Visibility = Visibility.Hidden;
            UpgradeGrid.Visibility = Visibility.Visible;
            parcelDetails.Content = bl.displayParcel(dataCparcelUpdate.Id);
            //Drones_Details.Visibility = Visibility.Visible;

            ListViewParcel = (ListView)parcelListView;
            ClientslistView.DataContext = boClientList;
            foreach (var item in bl.displayClientList())
            {
                boClientList.Add(item);

            }

            DronesListView.DataContext = boDroneList;
            foreach (var item in bl.displayDroneList())
            {
                boDroneList.Add(item);


            }
            DronesListView.ItemsSource = bl.displayDroneList();


        }

        
        #endregion

        #region closeFunctions
        private void ClickCloseParcelWindow(object sender, RoutedEventArgs e)
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


        #region DisplaySender_Click
        /// <summary>
        /// opens the window of the sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displaySender_Click(object sender, RoutedEventArgs e)
        {
            ClientActions clientActions = new();
            clientActions = bl.displayClientList().First(x => x.name == dataCparcelUpdate.SenderName);

            new ClientWindow(clientActions, bl, ClientslistView).Show();
        }
        #endregion

        #region DisplayTarget_Click
        /// <summary>
        /// opens the window of the target
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayTarget_Click(object sender, RoutedEventArgs e)
        {
            ClientActions clientActions = new();
            clientActions = bl.displayClientList().First(x => x.name == dataCparcelUpdate.TargetName);

            new ClientWindow(clientActions, bl, ClientslistView).Show();
        }
        #endregion

        #region DisplayDrone_Click
        /// <summary>
        /// opens the window of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayDrone_Click(object sender, RoutedEventArgs e)
        {
            //if (bl.displayDrone(bl.displayParcel(dataCparcelUpdate.Id).Drone.ID).ID != 0)
            try
            {
                DroneDescription droneDescription = bl.displayDroneList().First(x => x.parcelId == dataCparcelUpdate.Id);
                new DroneWindow(droneDescription, bl, DronesListView, default, default).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The parcel isn't connected to any drone!");
            }
        }
        #endregion
    }

}
