//triage de la liste
// rouge qui s'enleve quand on ferme la fenetre du message 


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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private bool checkFlag = false;

        string newName = "";
        private BLApi.IBL bl;
        BO.Drone dataCdrone = new BO.Drone();
        BO.DroneDescription dataCdroneUpdate = new BO.DroneDescription();
        //private DroneDescription droneDescription = new DroneDescription();
        //DroneListWindow dlw;
        ListView ListViewDrone;
        ListView FilterByWeight;
        ListView FilterByStatus;

        #region AddDrone
        // ctor to add a drone
        public DroneWindow(BLApi.IBL bl, object DroneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCdrone;
            //dlw = new DroneListWindow(bl);
            AddDroneGrid.Visibility = Visibility.Visible;
            UpdateDroneGrid.Visibility = Visibility.Hidden;
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            this.comboStationSelector.ItemsSource = bl.DisplayStationList();
            ListViewDrone = (ListView)DroneListWindow;

        }

        #region add_Click
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            Drone_Id.Background = Brushes.White;
            Drone_Model.Background = Brushes.White;

            if (comboWeightSelector.SelectedItem == null || comboStationSelector.SelectedItem == null || Drone_Id.Text == "" || Drone_Model.Text == "")
            {
                MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string droneIdString = Drone_Id.Text;// to check if it's an integer
            int droneIdInt;
            if (!int.TryParse(droneIdString, out droneIdInt))
            {
                Drone_Id.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Id", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //string droneModel = Drone_Model.Text;
            //WeightCategories droneWeight = (WeightCategories)comboWeightSelector.SelectedItem;
            StationDescription stationId = (StationDescription)comboStationSelector.SelectedItem;
            //BO.Drone drone = new Drone() { ID = droneIdInt, Model = droneModel, MaxWeight = droneWeight };
            try
            {
                bl.addDrone(dataCdrone, stationId.Id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "ID is not valid")
                    Drone_Id.Background = Brushes.Red;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            MessageBox.Show("Success!", "Added the drone", MessageBoxButton.OK, MessageBoxImage.Information);
            ListViewDrone.ItemsSource = bl.displayDroneList();
            //dlw.CheckFields();
            this.Close();
        }

        #endregion
        #endregion

        #region buttonsNotNeeded
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ComboBox_WeightSelection(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        void OnClick5(object sender, RoutedEventArgs e)
        {

        }


        void OnClick6(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.checkFlag = true; // will allow us to close the window from the button and not from the "X"
            this.Close();
        }

        // -------------------------------------------------------UPGRADE------------------------------------------------------------------------------

        #region constructorUPGRADE
        //ctor to upgrade the drone
        public DroneWindow(object selectedItem, BLApi.IBL bl, object dronesListView, object filterByWeight,object filterByStatus)
        {
            InitializeComponent();
            this.bl = bl;
            this.dataCdroneUpdate = (DroneDescription)selectedItem;
            DataContext = dataCdroneUpdate;
            AddDroneGrid.Visibility = Visibility.Hidden;
            UpdateDroneGrid.Visibility = Visibility.Visible;
            Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);
            Drones_Details.Visibility = Visibility.Visible;
            DroneStatuses statusDrone = dataCdroneUpdate.Status;
            if (statusDrone == DroneStatuses.maintenance)
            {
                FirstButton.Content = "FULLY CHARGED?";
                FirstButton.ToolTip = "Release drone from charge";
            }
            else if (statusDrone == DroneStatuses.free)
            {
                FirstButton.Content = "BATTERY LOW?";
                FirstButton.ToolTip = "Send drone to charge";
            }
            else
            {

                FirstButton.Content = "COLLECT PACKAGE";
                FirstButton.ToolTip = "Collect delivery";
            }

            if (statusDrone == DroneStatuses.free)
            {
                SecondButton.Content = "ASSIGN TO A PARCEL";
                SecondButton.ToolTip = "Send drone to delivery";
            }
            else if (statusDrone == DroneStatuses.shipping)
            {
                SecondButton.Content = "DELIVERING THE PACKAGE";
                SecondButton.ToolTip = "Deliver Parcel";
                ShowParcel.Visibility = Visibility.Visible;
            }
            else
            {
                SecondButton.Visibility = Visibility.Hidden;
            }

            ListViewDrone = (ListView)dronesListView;
            FilterByWeight = (ListView)filterByWeight;
            FilterByStatus = (ListView)filterByStatus;
            

        }

        #endregion

        //Here is the list of the buttons which allow us to upgrade a specific drone.
        //There are 4 buttons: 
        //1)UPDATE
        //2)FIRST_BUTTON:Can be:1)Release from charge(FULLY CHARGED?) if status:maintenance 2)Send drone to charge Button(BATTERY LOW?) if status:free 3)Collect delivery(COLLECT PACKAGE) if status:shipping
        //3)SECOND_BUTTON:Can be:1)Deliver Parcel(DELIVERING THE PACKAGE) if status:shipping 2)Send Drone To Delivery(READY FOR PICKING!) if status:free
        //4)CLOSE

        #region Update_Drone_Click
        /// <summary>
        /// This button updates the name of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
       // string newName = "";
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(/*"The model of your drone is being updated.*/"Please close this window and enter the new name.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateTextBox.Visibility = Visibility.Visible;
            UpdateLabel.Visibility = Visibility.Visible;
            CheckUpdate.Visibility = Visibility.Visible;


            UpdateLabel.Content = "Enter the new name:";



        }

        private void Check_Click_Update(object sender, RoutedEventArgs e)
        {
            newName = UpdateTextBox.Text;

            if (dataCdroneUpdate.Model == "")
            {
                MessageBox.Show("Please enter a name", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            try
            {
                bl.updateDroneName(dataCdroneUpdate.Id, newName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(ListViewDrone!=default)
            ListViewDrone.ItemsSource = bl.displayDroneList();
            if(FilterByWeight!=default)
            FilterByWeight.ItemsSource = bl.displayDroneList();
            if (FilterByStatus!=default)
            FilterByStatus.ItemsSource = bl.displayDroneList();
            Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);
            UpdateTextBox.Visibility = Visibility.Hidden;
            UpdateLabel.Visibility = Visibility.Hidden;
            CheckUpdate.Visibility = Visibility.Hidden;
            if (dataCdroneUpdate.Status == DroneStatuses.shipping)
            {
                ShowParcel.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region FirstButton_Click
        /// <summary>
        /// FirstButton:Can be:1)Release from charge(FULLY CHARGED?) if status:maintenance 2)Send drone to charge Button(BATTERY LOW?) if status:free 3)Collect delivery(COLLECT PACKAGE) if status:shipping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void ClickFirstButton(object sender, RoutedEventArgs e)
        {
            //First case:The status of the drone is FREE, the button is BATTERY LOW?
            //We send the drone to the closest station only if its status is FREE
            if (dataCdroneUpdate.Status == DroneStatuses.free)
            {

                MessageBox.Show("We are sending your drone to the closest base station.\n It will ready in a few moments. ", "Don't worry!", MessageBoxButton.OK, MessageBoxImage.Information);
                try
                {
                    bl.DroneToCharge(dataCdroneUpdate.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ListViewDrone != default)
                    ListViewDrone.ItemsSource = bl.displayDroneList();
                if (FilterByWeight != default)
                    FilterByWeight.ItemsSource = bl.displayDroneList();
                if (FilterByStatus != default)
                    FilterByStatus.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);


            }
            //Second case:The status of the drone is MAINTENANCE,the button is FULLY CHARGED?
            //We release the drone from its base station only if its status is MAINTENANCE
            else if (dataCdroneUpdate.Status == DroneStatuses.maintenance)
            {
                MessageBox.Show("Your drone is fully charged. We are going to unplug it.\nPlease close the window and enter the time of chargement", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateTextBox.Visibility = Visibility.Visible;
                UpdateLabel.Visibility = Visibility.Visible;
                CheckFullyCharged.Visibility = Visibility.Visible;
                UpdateLabel.Content = "Enter the time of chargement (in min):";


            }
            //Third Case:The status of the drone is SHIPPING,the button is COLLECT PACKAGE
            //The drone is going to collect a package
            else if (dataCdroneUpdate.Status == DroneStatuses.shipping)
            {
                MessageBox.Show("Your drone is going to collect the parcel attached to it", "On it's way!", MessageBoxButton.OK, MessageBoxImage.Information);
                try
                {
                    bl.PickedUp(dataCdroneUpdate.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ListViewDrone != default)
                    ListViewDrone.ItemsSource = bl.displayDroneList();
                if (FilterByWeight != default)
                    FilterByWeight.ItemsSource = bl.displayDroneList();
                if (FilterByStatus != default)
                    FilterByStatus.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);
            }
        }
        #endregion

        private void Check_Click_FullyCharged(object sender, RoutedEventArgs e)
        {
            string chargeTimeString = UpdateTextBox.Text;
            double chargeTime;
            if (!double.TryParse(chargeTimeString, out chargeTime))
            {
                MessageBox.Show("Please enter an integer Id", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                bl.DroneCharged(dataCdroneUpdate.Id, chargeTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);
            if (ListViewDrone != default)
                ListViewDrone.ItemsSource = bl.displayDroneList();
            if (FilterByWeight != default)
                FilterByWeight.ItemsSource = bl.displayDroneList();
            if (FilterByStatus != default)
                FilterByStatus.ItemsSource = bl.displayDroneList();
            UpdateTextBox.Visibility = Visibility.Hidden;
            UpdateLabel.Visibility = Visibility.Hidden;
            CheckFullyCharged.Visibility = Visibility.Hidden;
        }

        #region SecondButton_Click

        /// <summary>
        /// SECOND_BUTTON:Can be:1)Deliver Parcel(DELIVERING THE PACKAGE) if status:shipping 2)Send Drone To Delivery(READY FOR PICKING!) if status:free
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            //SECOND_BUTTON: Can be:1)Deliver Parcel(DELIVERING THE PACKAGE) if status:shipping 2)Assign a drone to a parcel if status:free

            //First Case:The status of the drone is free, the button is READY FOR PICKING!
            if (dataCdroneUpdate.Status == DroneStatuses.free)
            {
                MessageBox.Show("We are going to assign the drone to a parcel", "On it's way!");
                try
                {
                    bl.Assignement(dataCdroneUpdate.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ListViewDrone != default)
                    ListViewDrone.ItemsSource = bl.displayDroneList();
                if (FilterByWeight != default)
                    FilterByWeight.ItemsSource = bl.displayDroneList();
                if (FilterByStatus != default)
                    FilterByStatus.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);

                ShowParcel.Visibility = Visibility.Visible;

            }

            //Second case:The status of the drone is shipping,the button is DELIVERING THE PACKAGE
            else if (dataCdroneUpdate.Status == DroneStatuses.shipping)
            {
                MessageBox.Show("Your drone is delivering the parcel attached to it", "Mission almost accomplished");
                try
                {
                    bl.delivered(dataCdroneUpdate.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ListViewDrone != default)
                    ListViewDrone.ItemsSource = bl.displayDroneList();
                if (FilterByWeight != default)
                    FilterByWeight.ItemsSource = bl.displayDroneList();
                if (FilterByStatus != default)
                    FilterByStatus.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(dataCdroneUpdate.Id);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region Close_Click
        /// <summary>
        /// This button closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clear_button.Cursor = Cursors.Wait;
            Drone_Id.Text = "";
            Drone_Model.Text = "";
            comboStationSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
            clear_button.Cursor = Cursors.Arrow;

        }

        private void ShowParcel_Click(object sender, RoutedEventArgs e)
        {
            if (bl.displayDrone(dataCdroneUpdate.Id).Status == DroneStatuses.shipping)
            {
                int parcelID = 0;
                foreach (var item in bl.displayDroneList())
                {
                    if (item.Id == dataCdroneUpdate.Id)
                        parcelID = item.parcelId;
                }
                ParcelDescription parcelSelectedItem = bl.displayParcelList().First(x => x.Id == parcelID);
                new ParcelWindow(parcelSelectedItem, bl, ListViewDrone).Show();
            }
            else
            {
                MessageBox.Show("There is no parcel attached to your drone...", "Sorry!");
            }

        }

        private void Id_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                Drone_Model.Focus();
            }
        }

        private void Model_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                comboWeightSelector.Focus();
            }
        }
    }
}
