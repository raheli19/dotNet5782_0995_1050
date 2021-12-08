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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL bl;
        private DroneDescription droneDescription = new DroneDescription();

        ListView ListViewDrone;

        #region AddDrone
        // ctor to add a drone
        public DroneWindow(IBL.IBL bl, object DroneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            AddDroneGrid.Visibility = Visibility.Visible;
            UpdateDroneGrid.Visibility = Visibility.Hidden;
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            this.comboStationSelector.ItemsSource = bl.DisplayStationList();
            ListViewDrone = (ListView)DroneListWindow;

        }
        private bool checkFlag = false;

        #region add_Click
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            Drone_Id.Background = Brushes.White;
            Drone_Model.Background = Brushes.White;

            if (comboWeightSelector.SelectedItem == null || comboStationSelector.SelectedItem == null || Drone_Id.Text == "" || Drone_Model.Text == "")
            {
                MessageBox.Show("Please fill al the fields", "WARNING");
                return;
            }
            string droneIdString = Drone_Id.Text;
            int droneIdInt;
            if (!int.TryParse(droneIdString, out droneIdInt))
            {
                Drone_Id.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Id", "ERROR");
                return;
            }
            string droneModel = Drone_Model.Text;
            WeightCategories droneWeight = (WeightCategories)comboWeightSelector.SelectedItem;
            StationDescription stationId = (StationDescription)comboStationSelector.SelectedItem;
            IBL.BO.Drone drone = new Drone() { ID = droneIdInt, Model = droneModel, MaxWeight = droneWeight };
            try
            {
                bl.addDrone(drone, stationId.Id);
            }
            catch (Exception ex)
            {
                if(ex.Message=="ID is not valid")
                    Drone_Id.Background = Brushes.Red;
                MessageBox.Show(ex.Message, "ERROR");
                return;

            }
            MessageBox.Show("Success!", "Added the drone");
            ListViewDrone.ItemsSource = bl.displayDroneList();
            
            this.Close();
        }

        #endregion
        #endregion  
       
        #region buttonsNotNeeded
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

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

        private void Button_Close(object sender, RoutedEventArgs e)
        { 
             this.checkFlag = true; // will allow us to close the window from the button and not from the "X"
            this.Close();
        }

        // -------------------------------------------------------UPGRADE------------------------------------------------------------------------------

        #region constructorUPGRADE
        //ctor to upgrade the drone
        public DroneWindow(object selectedItem, IBL.IBL bl, object dronesListView)
        {
            InitializeComponent();
            this.bl = bl;
            this.droneDescription = (DroneDescription)selectedItem;
            AddDroneGrid.Visibility = Visibility.Hidden;
            UpdateDroneGrid.Visibility = Visibility.Visible;
            Drone_Label.Content = bl.displayDrone(droneDescription.Id);
            DroneStatuses statusDrone = droneDescription.Status;
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
                SecondButton.Content = "READY FOR PICKING!";
                SecondButton.ToolTip = "Send drone to delivery";
            }
            else if (statusDrone == DroneStatuses.shipping)
            {
                SecondButton.Content = "DELIVERING THE PACKAGE";
                SecondButton.ToolTip = "Deliver Parcel";
            }
            else
            {
                SecondButton.Visibility = Visibility.Hidden;
            }
            ListViewDrone = (ListView)dronesListView;

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
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            Update.Visibility = Visibility.Visible;

            MessageBox.Show("The model of your drone is being updated.", "Success!");

            //updater le model du drone
        }
        #endregion

        private void button5_Click()
        {
            TextBox dynamicTextBox = new TextBox();
            dynamicTextBox.Text = "Type Partnumber";
            
        }

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
            if (droneDescription.Status == DroneStatuses.free)
            {

                MessageBox.Show("We are sending your drone to the closest base station.\n It will ready in a few moments. ", "Don't worry!");
                bl.DroneToCharge(droneDescription.Id);
                ListViewDrone.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(droneDescription.Id);
               // this.droneDescription = bl.dron
              
            }
        //Second case:The status of the drone is MAINTENANCE,the button is FULLY CHARGED?
        //We release the drone from its base station only if its status is MAINTENANCE
        else if (droneDescription.Status == DroneStatuses.maintenance)
            {
                MessageBox.Show("Your drone is fully charged. We are going to unplug it", "Success!");
                //Prendre le temps de chargement du mishtamesh:bl.DroneCharged(droneDescription.Id);
                ListViewDrone.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(droneDescription.Id);

            }
         //Third Case:The status of the drone is SHIPPING,the button is COLLECT PACKAGE
         //The drone is going to collect a package
         else if(droneDescription.Status==DroneStatuses.shipping)
            {
                MessageBox.Show("Your drone is going to collect the parcel attached to it", "On it's way!");
                bl.Assignement(droneDescription.Id);
                ListViewDrone.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(droneDescription.Id);
            }
        }
        #endregion

        #region SecondButton_Click

        /// <summary>
        /// SECOND_BUTTON:Can be:1)Deliver Parcel(DELIVERING THE PACKAGE) if status:shipping 2)Send Drone To Delivery(READY FOR PICKING!) if status:free
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            //SECOND_BUTTON: Can be:1)Deliver Parcel(DELIVERING THE PACKAGE) if status:shipping 2)Send Drone To Delivery(READY FOR PICKING!) if status:free

            //First Case:The status of the drone is free, the button is READY FOR PICKING!
            if (droneDescription.Status == DroneStatuses.free)
            {
                MessageBox.Show("Your drone is going to pick the parcel attached to it", "On it's way!");
                bl.PickedUp(droneDescription.Id);
                ListViewDrone.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(droneDescription.Id);
            }

            //Second case:The status of the drone is shipping,the button is DELIVERING THE PACKAGE
            else if (droneDescription.Status == DroneStatuses.shipping)
            {
                MessageBox.Show("Your drone is delivering the parcel attached to it", "Mission almost accomplished");
                bl.delivered(droneDescription.Id);
                ListViewDrone.ItemsSource = bl.displayDroneList();
                Drone_Label.Content = bl.displayDrone(droneDescription.Id);
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


    }
}
