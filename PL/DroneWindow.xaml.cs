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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL bl;
        
        ListView ListViewDrone;

        #region AddDrone
        // ctor to add a drone
        public DroneWindow(IBL.IBL bl, object DroneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            this.comboStationSelector.ItemsSource = bl.DisplayStationList();
            ListViewDrone = (ListView)DroneListWindow;

        }

        #region add_Click
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            if (comboWeightSelector.SelectedItem == null || comboStationSelector.SelectedItem == null || Drone_Id.Text == "" || Drone_Model.Text == "")
            {
                MessageBox.Show("Please fill al the fields", "WARNING");
                return;
            }
            string droneIdString = Drone_Id.Text;
            int droneIdInt;
            if (!int.TryParse(droneIdString, out droneIdInt))
            {
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
                MessageBox.Show(ex.Message, "ERROR");

            }
            MessageBox.Show("Success!", "Added the drone");
            ListViewDrone.ItemsSource = bl.displayDroneList();
            this.Close();
        }
        #endregion
        #endregion

        //ctor to upgrade the drone
        public DroneWindow(object selectedItem, IBL.IBL bl, object dronesListView)
        {
            InitializeComponent();
            this.bl = bl;

            
        }

        #region buttonsNotNeeded
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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

            this.Close();
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ca marche", "success");
        }
    }
}
