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
        internal readonly BLApi.IBL bl = BLFactory.GetBL();
        public MainWindow()
        {

           
            InitializeComponent();
            bl.addClient(new BO.Client

        }

        private void MainButton_OpenDroneList(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
