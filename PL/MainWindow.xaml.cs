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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLApi;



// garde le sinoun apres avoir ajoute un dronert
// ii => vii voir que tout marche comme il se doitttt

// design de toute les fenetres qui ne sont pas designées SANS FAIRE UNE TRUC MOOOOCHE NI TROP GNAGNAN

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BLApi.IBL bl;

        #region Ctors
        public MainWindow()
        {
            InitializeComponent();

        }

        public MainWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
        }
        #endregion

        #region ButtonOpenLists
        /// <summary>
        /// open dronelist window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainButton_OpenDroneList(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
            
        }

        /// <summary>
        /// openclient list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainButton_ClientList(object sender, RoutedEventArgs e)
        {
            new ClientListWindow(bl).Show();
        }

        /// <summary>
        /// open parcel list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainButton_OpenParcelList(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(bl).Show();
        }

        /// <summary>
        /// open the station list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainButton_OpenStationsList(object sender, RoutedEventArgs e)
        {
            new StationListWindow(bl).Show();
        }
        #endregion
    }
}
