﻿// double clique ne marche pas
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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.IBL bl;
        DroneStatuses status = new DroneStatuses();
        WeightCategories weight = new WeightCategories();
        IEnumerable<DroneDescription> IEDrones = new List<DroneDescription>();

        public DroneListWindow()
        {
                
        }

        public DroneListWindow(IBL.IBL bl)
        {

            InitializeComponent();
            this.bl = bl;
            IEDrones = bl.displayDroneList();
            DronesListView.ItemsSource = IEDrones;
            this.comboStatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }
        static public DroneStatuses droneStat = 0;
        static public WeightCategories weightStat = 0;
        public bool weightFlag= false;
        public bool statusFlag = false;
        //private object isDataDirty;
        private bool checkFlag = false;

        #region SelectStatut
        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboStatusSelector.SelectedItem == null)
            {
                return;
            }
            /*DroneStatuses*/ status = (DroneStatuses)comboStatusSelector.SelectedItem;
            droneStat = status;
            statusFlag = true;
            if (weightFlag)
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status && x.weight == weightStat);
            else
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status);
        }
        #endregion

        #region WeightSelector
        private void comboWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.weight == weight && x.Status==droneStat);
            else
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.weight == weight);

        }

        public void CheckFields()
        {
            if (weightFlag && statusFlag)
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status && x.weight == weightStat);
            else if (statusFlag)
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status);
            else if (weightFlag)
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.weight == weight && x.Status == droneStat);
            else
                this.DronesListView.ItemsSource = IEDrones;
        }
        #endregion

        #region AddDroneOpenWindow
        private void button_addDrone(object sender, RoutedEventArgs e)
        {
            DroneWindow subWindow = new DroneWindow(bl, DronesListView);
            subWindow.Show();
            


        }
        #endregion 

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(DronesListView.SelectedItem, bl, DronesListView).Show();
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
            DronesListView.ItemsSource = IEDrones;

        }

        private void ClearWeight_Click(object sender, RoutedEventArgs e)
        {
            comboWeightSelector.SelectedItem = null;
            droneStat = 0;
            statusFlag = false;
            DronesListView.ItemsSource = IEDrones;

        }
    }
}
