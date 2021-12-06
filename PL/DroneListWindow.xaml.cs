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

        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatuses status = (DroneStatuses)comboStatusSelector.SelectedItem;
            this.DronesListView.ItemsSource = IEDrones.Where(x => x.Status == status);
        }

        private void comboWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)comboWeightSelector.SelectedItem;
            this.DronesListView.ItemsSource = IEDrones.Where(x => x.weight == weight);
        }
    }
}
