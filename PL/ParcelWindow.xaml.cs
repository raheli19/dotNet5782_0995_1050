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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BLApi.IBL bl;
        BO.Parcel dataCparcel = new BO.Parcel();
        private ParcelDescription parcelDescription = new ParcelDescription();
        //DroneListWindow dlw;
        ListView ListViewParcel;

        #region ADDPARCEL
        public ParcelWindow(BLApi.IBL bl, object ParcelListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCparcel;
            //dlw = new DroneListWindow(bl);
            //AddDroneGrid.Visibility = Visibility.Visible;
            //UpdateDroneGrid.Visibility = Visibility.Hidden;
            //this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            //this.comboStationSelector.ItemsSource = bl.DisplayStationList();
            ListViewParcel = (ListView)ParcelListWindow;

        }
        #endregion

        private void ClickUpdate(object sender, RoutedEventArgs e)
        {

        }

        private void ClickCloseParcelWindow(object sender, RoutedEventArgs e)
        {

        }
    }
}
