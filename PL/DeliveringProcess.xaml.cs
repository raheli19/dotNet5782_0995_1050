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
using BLApi;
using BO;
namespace PL
{
    /// <summary>
    /// Logique d'interaction pour DeliveringProcess.xaml
    /// </summary>



    public partial class DeliveringProcess : Window
    {
        private BLApi.IBL bl;
        BO.Client DataCclient = new BO.Client();
        string clientStatus;
        BO.Parcel DataCParcel = new BO.Parcel();
        public DeliveringProcess(BLApi.IBL bl, BO.Client DataCclient, string clientStatus)
        {
            InitializeComponent();
            this.MaxWeightL.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            this.PriorityL.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            DataContext = DataCParcel;
            DataCParcel.Sender = new();
            DataCParcel.Target = new();
            this.bl = bl;
            this.DataCclient = DataCclient;
            this.clientStatus = clientStatus;
        }


        private void ParcelsFromClient_Click(object sender, RoutedEventArgs e)
        {
            if (clientStatus == "Sign Up")   //Join
            {
                MessageBox.Show("Aie aie aie...", "You just joined our delivering system.You didn't send any parcel yet. ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (clientStatus == "Sign In")
            {
                string parcelsFromClientList = bl.parcelsToCliList(DataCclient.ID);
                parcelsFromClient.Visibility = Visibility.Visible;
                parcelsFromClient.Content = parcelsFromClientList;


            }
        }

        private void ParcelsToClient_Click(object sender, RoutedEventArgs e)
        {
            if (clientStatus == "Sign Up")   //Join
            {
                MessageBox.Show("Aie aie aie...", "You just joined our delivering system.There is no parcels for you. ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (clientStatus == "Sign In")
            {
                string parcelsToClientList = bl.parcelsFromCLiList(DataCclient.ID);
                parcelsToClient.Visibility = Visibility.Visible;
                parcelsToClient.Content = parcelsToClientList;
            }
        }

        private void SendNewParcel_Click(object sender, RoutedEventArgs e)
        {
            senderID.Background = Brushes.White;
            TargetID.Background = Brushes.White;
            //Labels
            SenderId.Visibility = Visibility.Visible;
            TargetId.Visibility = Visibility.Visible;
            MaxWeight.Visibility = Visibility.Visible;
            Priority.Visibility = Visibility.Visible;
            //textboxs and comboBox
            senderID.Visibility = Visibility.Visible;
            TargetID.Visibility = Visibility.Visible;
            MaxWeightL.Visibility = Visibility.Visible;
            PriorityL.Visibility = Visibility.Visible;

            try
            {
                bl.addParcel(DataCParcel);
            }
            catch (Exception ex)
            {
                if (ex.Message == "SenderID not valid")
                {
                    senderID.Background = Brushes.Red;
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (ex.Message == "TargetID not valid")
                {
                    TargetID.Background = Brushes.Red;
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (ex.Message == "This package already exists")
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void AssignParcelClick(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_WeightSelection(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_PrioritySelection(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
