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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private BLApi.IBL bl;
        BO.Client dataCclient = new BO.Client();
        //Nullable<int> value;
        private ClientActions clientActions = new ClientActions();
        ListView ListViewClient;
        private bool checkFlag = false;
        #region UpgradeClient
        public ClientWindow(object selectedItem, BLApi.IBL bl, object ClientListWindow)
        {

            InitializeComponent();
            this.bl = bl;
            dataCclient.ClientLoc = new Localisation();
            DataContext = dataCclient;
            this.clientActions = (ClientActions)selectedItem;
            AddGridClient.Visibility = Visibility.Hidden;
            UpgradeClientGrid.Visibility = Visibility.Visible;
            ListViewClient = (ListView)ClientListWindow;
            Client_Details.Visibility = Visibility.Visible;

            Client_Label.Content = bl.displayClient(clientActions.Id);
        }

        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(/*"The model of your drone is being updated.*/"Please close this window and enter the new name and/or new phone number.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            // autres en hidden/visible
            UpdateNameTextBox.Visibility = Visibility.Visible;
            UpdatePhoneTextBox.Visibility = Visibility.Visible;
            UpdateLabel.Visibility = Visibility.Visible;
            CheckUpdate.Visibility = Visibility.Visible;
            UpdateLabel.Content = "Enter the new name and/or new phone number";

        }

        private void Check_Click_Update(object sender, RoutedEventArgs e)
        {
            string newName = UpdateNameTextBox.Text;
            string newPhone="n";
            if (UpdatePhoneTextBox.Text !="")
                 newPhone = (UpdatePhoneTextBox.Text);

            if (clientActions.name == "" && clientActions.phone == "")// didn't enter any information
            {
                MessageBox.Show("Please enter an information", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            try
            {
                bl.updateClientName_Phone(clientActions.Id, newName, newPhone);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Client_Label.Content = bl.displayClient(clientActions.Id);
            ListViewClient.ItemsSource = bl.displayClientList();
            UpdateNameTextBox.Visibility = Visibility.Hidden;
            UpdatePhoneTextBox.Visibility = Visibility.Hidden;
            UpdateLabel.Visibility = Visibility.Hidden;
            CheckUpdate.Visibility = Visibility.Hidden;
        }


        #endregion

#region addClient
        public ClientWindow(BLApi.IBL bl, object ClientListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCclient;
            dataCclient.ClientLoc = new Localisation();
            //dlw = new DroneListWindow(bl);
            AddGridClient.Visibility = Visibility.Visible;
            UpgradeClientGrid.Visibility = Visibility.Hidden;
            //this.comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            //this.comboStationSelector.ItemsSource = bl.DisplayStationList();
             ListViewClient = (ListView)ClientListWindow;

        }
        
        private void txt_long_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataCclient.ClientLoc.longitude = (double)int.Parse( txt_long.Text);
            
        }
        private void txt_lat_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataCclient.ClientLoc.latitude = (double)int.Parse(txt_lat.Text);

        }
        private void Add_button(object sender, RoutedEventArgs e)
        {
            txt_id.Background = Brushes.White;
            txt_phone.Background = Brushes.White;
            txt_lat.Background = Brushes.White;
            txt_long.Background = Brushes.White;
            //if (txt_id.Text =="" && dataCclient.Name ="" && dataCclient.Phone="")
            //MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (txt_id.Text == "" || txt_name.Text == "" || txt_lat.Text == "" || txt_long.Text == "" || txt_phone.Text == "")
            {
                MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string clientIdCheck = txt_id.Text;// to check if it's an integer
            int clientIdInt;
            string phoneCheck = txt_phone.Text;
            int clientPhoneCheck;
            if (!int.TryParse(clientIdCheck, out clientIdInt))
            {
                txt_id.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Id", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(phoneCheck, out clientPhoneCheck))
            {
                txt_phone.Background = Brushes.Red;
                MessageBox.Show("Please enter an integer Number", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                bl.addClient(dataCclient);
            }
            catch (Exception ex)
            {
                if (ex.Message == "ID not valid")
                    txt_id.Background = Brushes.Red;
                if (ex.Message == "Phone not valid")
                    txt_phone.Background = Brushes.Red;
                if (ex.Message == "latitude is not valid")
                    txt_lat.Background = Brushes.Red;
                if (ex.Message == "longitude is not valid")
                    txt_long.Background = Brushes.Red;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            MessageBox.Show("You're added to our system! Welcome!","Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            ListViewClient.ItemsSource = bl.displayClientList();
            //dlw.CheckFields();
            this.Close();

        }

        #endregion



        #region closeFct
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
