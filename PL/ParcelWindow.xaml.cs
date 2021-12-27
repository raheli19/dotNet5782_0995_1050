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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BLApi.IBL bl;
        BO.Parcel dataCparcel = new BO.Parcel();
        private ParcelDescription parcelDescription = new ParcelDescription();
        //DroneListWindow dlw;
        ListView ListViewParcel;
        private bool checkFlag = false;

        #region ADDPARCEL
        public ParcelWindow(BLApi.IBL bl, object ParcelListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            dataCparcel.Sender = new ClientInParcel();
            dataCparcel.Target = new ClientInParcel();
            DataContext = dataCparcel;


            //dlw = new DroneListWindow(bl);
            //AddDroneGrid.Visibility = Visibility.Visible;
            //UpdateDroneGrid.Visibility = Visibility.Hidden;
            this.Combo_SenderId.ItemsSource = bl.AllSenders_Id();
            this.Combo_TargetId.ItemsSource = bl.AllTargets_Id();
            this.Combo_Weight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            this.Combo_Priority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            //this.comboStationSelector.ItemsSource = bl.DisplayStationList();
            ListViewParcel = (ListView)ParcelListWindow;

        }

        private void OpenWindow_Add(object sender, RoutedEventArgs e)
        {

            if (Combo_SenderId == null || Combo_TargetId == null || Combo_Weight == null || Combo_Priority == null)
            {
                MessageBox.Show("Please make a selection", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        }
        #endregion



        #region UPDATE
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {

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

        private void Combo_SenderId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataCparcel.Sender.ID = (int)Combo_SenderId.SelectedItem;
        }
    }
}
