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
        private ObservableCollection<BO.ParcelDescription> boParcelList = new ObservableCollection<BO.ParcelDescription>();

        #region UpgradeClient

        /// <summary>
        /// Constructor to update a client
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="bl"></param>
        /// <param name="ClientListWindow"></param>
        public ClientWindow(object selectedItem, BLApi.IBL bl, object ClientListWindow)
        {

            InitializeComponent();
            this.bl = bl;
            dataCclient.ClientLoc = new Localisation();
            //DataContext = dataCclient;
            this.clientActions = (ClientActions)selectedItem;
            AddGridClient.Visibility = Visibility.Hidden;
            UpgradeClientGrid.Visibility = Visibility.Visible;
            ListViewClient = (ListView)ClientListWindow;
            Client_Details.Visibility = Visibility.Visible;
            Client_Label.Content = bl.displayClient(clientActions.Id);
            ParcelsFromClient.ItemsSource = bl.FindParcelsFromClient(clientActions.Id);
            ParcelsToClient.ItemsSource = bl.FindParcelsToClient(clientActions.Id);
            listViewParcels.DataContext = boParcelList;
            DataContext = boParcelList;
            foreach (var item in bl.displayParcelList())
            {
                boParcelList.Add(item);


            }

        }

        /// <summary>
        /// Call a function to update details of client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please close this window and enter the new name and/or new phone number.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
       
            Client_Details.Visibility = Visibility.Hidden;
            UpdateNameTextBox.Visibility = Visibility.Visible;
            UpdatePhoneTextBox.Visibility = Visibility.Visible;
            UpdateLabel.Visibility = Visibility.Visible;
            CheckUpdate.Visibility = Visibility.Visible;
            UpdateLabel.Content = "Enter the new name and/or new phone number";

        }

        /// <summary>
        /// Updates details of client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_Click_Update(object sender, RoutedEventArgs e)
        {
            string newName = UpdateNameTextBox.Text;
            string newPhone = "n";
            if (UpdatePhoneTextBox.Text != "")
                newPhone = (UpdatePhoneTextBox.Text);

            if (UpdateNameTextBox.Text == null && UpdatePhoneTextBox.Text == null)// didn't enter any information
            {
                MessageBox.Show("Please enter an information", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            try
            {
                bl.updateClientName_Phone(clientActions.Id, newName, newPhone);// not in binding because the datacontext is for the lists
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
            Client_Details.Visibility = Visibility.Visible;
        }


        #endregion

        #region AddClient
        public ClientWindow(BLApi.IBL bl, object ClientListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = dataCclient;
            dataCclient.ClientLoc = new Localisation();
            AddGridClient.Visibility = Visibility.Visible;
            UpgradeClientGrid.Visibility = Visibility.Hidden;
            ListViewClient = (ListView)ClientListWindow;

        }
        /// <summary>
        /// Add a new client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_button(object sender, RoutedEventArgs e)
        {
            txt_id.Background = Brushes.White;
            txt_phone.Background = Brushes.White;
            txt_lat.Background = Brushes.White;
            txt_long.Background = Brushes.White;
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
            MessageBox.Show("You're added to our system! Welcome!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            ListViewClient.ItemsSource = bl.displayClientList();
            //dlw.CheckFields();
            this.Close();

        }

        #endregion

        #region closeFct

        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.checkFlag = true; // will allow us to close the window from the button and not from the "X"
            this.Close();
        }

        /// <summary>
        /// On closing function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, CancelEventArgs e)
        {

            if (this.checkFlag)// call from the button
                e.Cancel = false;
            else
                e.Cancel = true;// call from the "X", we don't want to close

        }
        #endregion

        #region ParcelsToClient_MouseDoubleClick
        /// <summary>
        ///Returns list of parcelsToClient by MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelsToClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToClient myParcelToClient = (ParcelToClient)ParcelsToClient.SelectedItem;
            ParcelDescription myParcelDescription = bl.displayParcelList().First(x => x.Id == myParcelToClient.ID);
            new ParcelWindow(myParcelDescription, bl, listViewParcels).Show();
        }
        #endregion

        #region ParcelsFromClient_MouseDoubleClick
        /// <summary>
        /// Returns list of parcelsFromClient by MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelsFromClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToClient myParcelToClient = (ParcelToClient)ParcelsFromClient.SelectedItem;
            ParcelDescription myParcelDescription = bl.displayParcelList().First(x => x.Id == myParcelToClient.ID);
            new ParcelWindow(myParcelDescription, bl, listViewParcels).Show();
        }
        #endregion 

        #region enterFunctions
        /// <summary>
        /// These functions allow us to pass from textbox to another
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void id_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_name.Focus();
            }

        }

        private void name_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_phone.Focus();
            }
        }


        private void phone_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_lat.Focus();
            }
        }

        private void lat_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txt_long.Focus();
            }
        }

        private void long_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ADD.Focus();
                Add_button(sender, e);
            }
        }

        #endregion
    }
}
