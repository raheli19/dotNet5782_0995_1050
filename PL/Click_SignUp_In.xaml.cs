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
    /// Logique d'interaction pour Click_SignUp_In.xaml
    /// </summary>
    public partial class Click_SignUp_In : Window
    {
        BO.Client DataCclient = new BO.Client();
        private BLApi.IBL bl;
        string clientStatus = " ";
        public Click_SignUp_In(BLApi.IBL bl,int Sign_Uptemp)
        {
            InitializeComponent();
            DataContext = DataCclient;
            DataCclient.ClientLoc = new();
            this.bl = bl;
            Sign_Up.Visibility = Visibility.Visible;


        }
        public Click_SignUp_In(BLApi.IBL bl)
        {
            InitializeComponent();
            DataContext = DataCclient;
            this.bl = bl;
            Sign_In.Visibility = Visibility.Visible;


        }
        private void button_Join_Click(object sender, RoutedEventArgs e)
        {
            textBox_PersonalId.Background = Brushes.White;
            textBox_Phone.Background = Brushes.White;
            textBox_Latitude.Background = Brushes.White;
            textBox_Longitude.Background = Brushes.White;
            clientStatus = "Sign Up";

            if (textBox_PersonalId.Text == "" || textBox_Phone.Text == "" || textBox_Latitude.Text == "" || textBox_Longitude.Text == "" || textBox_Name.Text == "")
            {
                MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string droneIdString = textBox_PersonalId.Text;
            int droneIdInt;
            if (!int.TryParse(droneIdString, out droneIdInt))
            {
                textBox_PersonalId.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Id", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                bl.addClient(DataCclient);
            }
            catch (InputNotValid ex)
            {
                if (ex.Message == "ID is not valid")
                    textBox_PersonalId.Background = Brushes.Red;
                if (ex.Message == "Phone not valid")
                    textBox_Phone.Background = Brushes.Red;
                if (ex.Message == "latitude is not valid")
                    textBox_Latitude.Background = Brushes.Red;
                if (ex.Message == "longitude is not valid")
                    textBox_Longitude.Background = Brushes.Red;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            MessageBox.Show("Success!", "Thanks for joining us", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
            new DeliveringProcess(bl, DataCclient, clientStatus).Show();
        }

        private void NEXT_Click(object sender, RoutedEventArgs e)
        {
            clientStatus = "Sign In";
            if (textBox_ID.Text == " ")
            {
                MessageBox.Show("Please enter your ID", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            BO.Client tempClient = new BO.Client();
            try
            {
                tempClient = bl.GetClient(DataCclient.ID);
            }
            catch (IDNotFound ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataCclient = bl.displayClient(DataCclient.ID);
            this.Close();
            new DeliveringProcess(bl, DataCclient, clientStatus).Show();
        }
    }
}
