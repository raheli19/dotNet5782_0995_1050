// double clique ne marche pas
//add avec la photo



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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.IBL bl;
        
        IEnumerable<DroneDescription> IEDrones = new List<DroneDescription>();
        public DroneListWindow(IBL.IBL bl)
        {
           
            InitializeComponent();
            this.bl = bl;
            IEDrones = bl.displayDroneList();
            DronesListView.ItemsSource = IEDrones;
            this.comboStatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }
        static DroneStatuses droneStat = 0;
        static WeightCategories weightStat = 0;
        bool weightFlag= false;
        bool statusFlag = false;
        
        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatuses status = (DroneStatuses)comboStatusSelector.SelectedItem;
            droneStat = status;
            statusFlag = true;
            if (weightFlag)
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status && x.weight == weightStat);
            else
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status);
        }

        private void comboWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)comboWeightSelector.SelectedItem;
            weightStat = weight;
            weightFlag = true;
            if (statusFlag)
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.weight == weight && x.Status==droneStat);
            else
                this.DronesListView.ItemsSource = IEDrones.Where(x => x.weight == weight);

        }

        private void button_addDrone(object sender, RoutedEventArgs e)
        {
            DroneWindow subWindow = new DroneWindow(bl, DronesListView);
            subWindow.Show();

        }

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(DronesListView.SelectedItem, bl, DronesListView).Show();
            comboStatusSelector.SelectedItem = null;
            comboWeightSelector.SelectedItem = null;
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            droneStat = 0;
            weightStat = 0;
            weightFlag = false;
            statusFlag = false;
            DronesListView.ItemsSource = IEDrones;
            
            //comboStatusSelector.SetCurrentValue(" ");
           // comboWeightSelector.;
        }
    }
}
