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
    /// Interaction logic for ParcelList.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        private BLApi.IBL bl;
        private ObservableCollection<BO.ParcelDescription> boParcelList = new ObservableCollection<BO.ParcelDescription>();
        private bool checkFlag = false;
        public ParcelListWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            DataContext = boParcelList;
            ParcelsListView.DataContext = boParcelList;
            this.bl = bl;
            foreach (var item in bl.displayParcelList())
            {
                boParcelList.Add(item);


            }
            
            //combobox?
        }

        private void ParcelView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //open the ParcelWindow
        }

        private void button_addDrone(object sender, RoutedEventArgs e)
        {
            //open parcelWindow
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
    }
}
