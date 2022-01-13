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
    /// Logic interaction for DeliveringProcess.xaml
    /// </summary>

    public partial class DeliveringProcess : Window
    {
        private BLApi.IBL bl;
        BO.Client DataCclient = new BO.Client();
        string clientStatus;
        BO.Parcel DataCParcel = new BO.Parcel();

        #region ctor
        public DeliveringProcess(BLApi.IBL bl, BO.Client DataCclient, string clientStatus)
        {
            InitializeComponent();
            this.MaxWeightL.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            this.PriorityL.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            this.Combo_TargetId.ItemsSource = bl.AllTargets_Id();
            DataContext = DataCParcel;
            DataCParcel.Sender = new();
            DataCParcel.Target = new();
            DataCParcel.Sender.ID = DataCclient.ID;
            DataCParcel.Sender.name = DataCclient.Name;
            DataCParcel.ID = Dal.Configuration.RunnerIDnumber;
            this.bl = bl;
            this.DataCclient = DataCclient;
            this.clientStatus = clientStatus;
        }
        #endregion

        #region AllButtonsClick
        private void ParcelsFromClient_Click(object sender, RoutedEventArgs e)
        {
            if (clientStatus == "Sign Up")   //Join
            {
                MessageBox.Show("Aie aie aie...", "You just joined our delivering system.You didn't send any parcel yet. ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (clientStatus == "Sign In")
            {
                string parcelsFromClientList = bl.parcelsToCliList(DataCclient.ID); //
                parcelsFromClient.Visibility = Visibility.Visible;
                parcelsFromClient.Content = parcelsFromClientList;


            }
        }


        private void Combo_TargetId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataCParcel.Target.ID = (int)Combo_TargetId.SelectedItem;
            DataCParcel.Target.name = bl.displayClient(DataCParcel.Target.ID).Name;

        }

        private void Add_Parcel_ToSend_Click(object sender, RoutedEventArgs e)
        {
            if (MaxWeightL == null || PriorityL == null || Combo_TargetId == null)
            {
                MessageBox.Show("Please fill al the fields", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                bl.addParcel(DataCParcel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            MaxWeightL.Visibility = Visibility.Hidden;
            PriorityL.Visibility = Visibility.Hidden;
            Add_Parcel_ToSend.Visibility = Visibility.Hidden;
            MaxWeight.Visibility = Visibility.Hidden;
            Combo_TargetId.Visibility = Visibility.Hidden;
            Priority.Visibility = Visibility.Hidden;
            MaxWeight.Visibility = Visibility.Hidden;
            TargetId.Visibility = Visibility.Hidden;
            this.clientStatus = "Sign In";
            MessageBox.Show("The parcel has been added to the list", "Done!", MessageBoxButton.OK, MessageBoxImage.Information);

            return;
        }

        private void ParcelsToClient_Click(object sender, RoutedEventArgs e)
        {
            // enters the number of the parcel // fromthe list
            //calls the function delivered

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
            ////senderID.Background = Brushes.White;
            Combo_TargetId.Background = Brushes.White;
            //Labels
            //SenderId.Visibility = Visibility.Visible;
            TargetId.Visibility = Visibility.Visible;
            MaxWeight.Visibility = Visibility.Visible;
            Priority.Visibility = Visibility.Visible;
            //textboxs and comboBox
            //senderID.Visibility = Visibility.Visible;
            Combo_TargetId.Visibility = Visibility.Visible;
            MaxWeightL.Visibility = Visibility.Visible;
            PriorityL.Visibility = Visibility.Visible;
            Add_Parcel_ToSend.Visibility = Visibility.Visible;



            try
            {
                bl.addParcel(DataCParcel);
            }
            catch (Exception ex)
            {
                if (ex.Message == "TargetID not valid")
                {
                    Combo_TargetId.Background = Brushes.Red;
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

        private void received_parcel_Click(object sender, RoutedEventArgs e)
        {
            //Parcel_Id_Entered.Visibility = Visibility.Visible;
            //label_info.Content = "Please enter its Id";
            //label_info.Visibility = Visibility.Visible;
            BO.DroneDescription delDrone = new DroneDescription();
            BO.ParcelDescription parcelDelivered = new ParcelDescription();

            parcelDelivered = bl.displayParcelList().FirstOrDefault(x => x.TargetName == this.Name);
            if (parcelDelivered == null)
            {
                MessageBox.Show("Aie aie aie...", "There is no parcels for you. ", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            delDrone = bl.displayDroneList().First(x => x.Id == bl.displayParcel(parcelDelivered.Id).Drone.ID);
            try
            {


                bl.delivered(delDrone.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aie aie aie...", ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }
        }

        #endregion

        #region checkPickedUp

        private void Picked_up(object sender, RoutedEventArgs e)
        {
            //enter the id of the parcel from the list
            // supposed to go into th elist of the parcel, find it, and assign in the datacontext the id of the associated' drone!
            //DataCParcel.ID = Convert.ToInt32(Parcel_Id_Entered.Text);
            //DataCParcel.Drone.ID = Convert.ToInt32(Drone_Id_entered.Text);

            // trouver le drone qui puisse porter
            // puisse faire la distance+ aller se charger

            List<BO.DroneDescription> listOfDrones = bl.displayDroneList().ToList();
            BO.DroneDescription associatedDrone = new DroneDescription();
            if (DataCParcel.Scheduled != DateTime.MinValue)
            {
                try
                {

                    bl.PickedUp(DataCParcel.Drone.ID);

                }


                catch (Exception ex)
                {
                    MessageBox.Show("Aie aie aie...", ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Aie aie aie...", "Your Parcel is not associatedDrone yet", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }
        #endregion

        #region CloseInfos
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            listOf_Parcels.Visibility = Visibility.Hidden;
            label_info.Visibility = Visibility.Hidden;
            Parcel_Id_Entered.Visibility = Visibility.Hidden;
            click_PickedUpButton.Visibility = Visibility.Hidden;
            TargetId.Visibility = Visibility.Hidden;
            MaxWeight.Visibility = Visibility.Hidden;
            Priority.Visibility = Visibility.Hidden;
            //textboxs and comboBox
            Combo_TargetId.Visibility = Visibility.Hidden;
            MaxWeightL.Visibility = Visibility.Hidden;
            PriorityL.Visibility = Visibility.Hidden;
            parcelsToClient.Visibility = Visibility.Hidden;
            parcelsFromClient.Visibility = Visibility.Hidden;
            Add_Parcel_ToSend.Visibility = Visibility.Hidden;

        }
        #endregion

        #region UpdateName/Phone
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(/*"The model of your drone is being updated.*/"Please close this window and enter the new name and/or new phone number.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            // autres en hidden/visible
            UpdateNameTextBox.Visibility = Visibility.Visible;
            UpdatePhoneTextBox.Visibility = Visibility.Visible;
            UpdateLabel.Visibility = Visibility.Visible;
            CheckUpdate.Visibility = Visibility.Visible;
            //CheckUpdate2.Visibility = Visibility.Visible;
            UpdateLabel.Content = "Enter your new name and/or new phone number";
            DataContext = this.DataCclient;
        }

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
                bl.updateClientName_Phone(this.DataCclient.ID, DataCclient.Name, DataCclient.Phone);// not in binding because the datacontext is for the lists
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UpdateNameTextBox.Visibility = Visibility.Hidden;
            UpdatePhoneTextBox.Visibility = Visibility.Hidden;
            UpdateLabel.Visibility = Visibility.Hidden;
            CheckUpdate.Visibility = Visibility.Hidden;
            DataContext = DataCParcel;
        }
        #endregion

    }
}
