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

//filtration avant adding obligatoire?
// button close dans droneList window (a completer) et droneLIst pour upgrade
// button qui annule l'ajout
// garde le sinoun apres avoir ajoute un dronert
// dans la droneWindow une possibilité de fermer la fenetre
// ii => vii voir que tout marche comme il se doitttt
// il faut diminuer le nb de buttons dans la upgrade drone
// bonus qu'on peut pas fermer avec la croix mais seulement le caftor

// design de toute les fenetres qui ne sont pas designées SANS FAIRE UNE TRUC MOOOOCHE NI TROP GNAGNAN
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL.IBL bl;
        public MainWindow()
        {
            bl = new IBL.BL();
            InitializeComponent();
        }

        private void MainButton_OpenDroneList(object sender, RoutedEventArgs e)
        {
            DroneListWindow subWindow = new DroneListWindow(bl);
            subWindow.Show();
        }

    }
}
