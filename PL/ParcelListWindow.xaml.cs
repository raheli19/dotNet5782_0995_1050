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
        Priorities prior = new Priorities();//statut
        WeightCategories weight = new WeightCategories();
        public BO.Parcel dataCparcel = new Parcel();
        
        private ObservableCollection<BO.ParcelDescription> boParcelList = new ObservableCollection<BO.ParcelDescription>();
        private bool checkFlag = false;
        public static Priorities parcelPriority = 0;//dronestat
        public static WeightCategories weightStat = 0;
        public bool weightFlag = false;
        public bool priorityFlag = false; //droneFlag

        #region Ctor
        public ParcelListWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            //DataContext = boParcelList;
            ParcelsListView.DataContext = boParcelList;
            dataCparcel.ID = 9;
            RemoveGrid.DataContext = dataCparcel;
            this.bl = bl;
            foreach (var item in bl.displayParcelList())
            {
                boParcelList.Add(item);

            }
            //combobox?
        }
        #endregion

        #region Close_Functions
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

        #region OpenParcel
        private void button_addParcel(object sender, RoutedEventArgs e)
        {
            ParcelWindow subWindow = new ParcelWindow(bl, ParcelsListView);
            subWindow.Show();
        }
        #endregion

        #region Double_Click

        private void FilterByTarget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(FilterByTarget.SelectedItem, bl, ParcelsListView).Show();
        }
        private void ParcelView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(ParcelsListView.SelectedItem, bl, ParcelsListView).Show();
        }

        private void FilterBySender_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(FilterBySender.SelectedItem, bl, ParcelsListView).Show();
        }

        #endregion

        #region Filters_Select
        private void FilterBySender1_Click(object sender, RoutedEventArgs e)
        {
            List<ParcelDescription> parcelsBySender = new List<ParcelDescription>();
            foreach (var item in bl.displayParcelList())
            {
                parcelsBySender.Add(new ParcelDescription() { Id = item.Id, SenderName = item.SenderName, TargetName = item.TargetName, weight = item.weight, Status = item.Status, priority = item.priority });

            }
            List<ParcelDescription> parcelsBYSender = new List<ParcelDescription>();
            parcelsBYSender = parcelsBySender.OrderBy(d => d.SenderName).ToList();
            FilterBySender.ItemsSource = parcelsBYSender;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilterBySender.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Add(groupDescription);
            FilterBySender.Visibility = Visibility.Visible;
            FilterByTarget.Visibility = Visibility.Hidden;
            ParcelsListView.Visibility = Visibility.Hidden;
        }

        private void FilterByTarget1_Click(object sender, RoutedEventArgs e)
        {
            List<ParcelDescription> parcelsByTarget = new List<ParcelDescription>();
            foreach (var item in bl.displayParcelList())
            {
                parcelsByTarget.Add(new ParcelDescription() { Id = item.Id, SenderName = item.SenderName, TargetName = item.TargetName, weight = item.weight, Status = item.Status, priority = item.priority });

            }
            List<ParcelDescription> parcelsBYTarget = new List<ParcelDescription>();
            parcelsBYTarget = parcelsByTarget.OrderBy(d => d.TargetName).ToList();
            FilterByTarget.ItemsSource = parcelsBYTarget;

            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(FilterByTarget.ItemsSource);
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("TargetName");
            view2.GroupDescriptions.Add(groupDescription2);
            this.Combo_priority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            this.Combo_weight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            FilterByTarget.Visibility = Visibility.Visible;
            FilterBySender.Visibility = Visibility.Hidden;
            ParcelsListView.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Clear_Click
        private void clearGrouping_Click(object sender, RoutedEventArgs e)
        {
            FilterBySender.Visibility = Visibility.Hidden;
            FilterByTarget.Visibility = Visibility.Hidden;
            ParcelsListView.Visibility = Visibility.Visible;
        }

        private void ClearPriority_Click(object sender, RoutedEventArgs e)
        {
            Combo_priority.SelectedItem = null;
            weightFlag = false;
            weightStat = 0;
            ParcelsListView.DataContext = boParcelList;

        }

        private void ClearWeight_Click(object sender, RoutedEventArgs e)
        {
            Combo_weight.SelectedItem = null;
            parcelPriority = 0;
            priorityFlag = false;
            ParcelsListView.DataContext = boParcelList;

        }

        #endregion

        #region Combo_Selections
        private void Combo_weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo_weight.SelectedItem == null)
            {
                return;
            }
            /*DroneStatuses*/
            weight = (WeightCategories)Combo_weight.SelectedItem;
            weightStat = weight;

            weightFlag = true;
            if (priorityFlag)
                this.ParcelsListView.ItemsSource = boParcelList.Where(x => x.weight == weight && x.priority == parcelPriority);
            else
                this.ParcelsListView.ItemsSource = boParcelList.Where(x => x.weight == weight);

        }

        private void Combo_priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo_priority.SelectedItem == null)
            {
                return;
            }
            /*WeightCategories*/
            prior = (Priorities)Combo_priority.SelectedItem;
            parcelPriority = prior;
            priorityFlag = true;
            if (weightFlag)
                this.ParcelsListView.ItemsSource = boParcelList.Where(x => x.priority == prior && x.weight == weightStat);
            else
                this.ParcelsListView.ItemsSource = boParcelList.Where(x => x.priority == prior);

        }

        #endregion

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            RemoveGrid.Visibility = Visibility.Visible;

        }

        #region enter_tap
        private void enter_tap(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    RemoveGrid.DataContext = bl.displayParcel(Convert.ToInt32( ID.Text) );
                    dataCparcel = bl.displayParcel(Convert.ToInt32(ID.Text));
                    RemoveGrid.Visibility = Visibility.Visible;
                    this.Remove_Button(sender, e, dataCparcel); }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }

            }
        }
        #endregion

        #region Remove_Button
        private void Remove_Button( object sender, RoutedEventArgs e, BO.Parcel parcel)
        {
            //if (Combo_SenderId.SelectedItem == null || Combo_TargetId.SelectedItem == null)
            //{
            //    MessageBox.Show("Please make a selection", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            //if (Combo_SenderId.SelectedItem.Equals(Combo_TargetId.SelectedItem))
            //{
            //    MessageBox.Show("A client didn't sent a parcel to himself! ", "Aie aie aie...", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            //dataCparcel.ID = bl.displayParcelList().Where(x => x.SenderName == bl.displayClient((int)Combo_SenderId.SelectedItem).Name && x.TargetName == bl.displayClient((int)Combo_TargetId.SelectedItem).Name);
            //dataCparcel.ID = bl.GetIdParcel((int)Combo_SenderId.SelectedItem, (int)Combo_TargetId.SelectedItem);
            try
            {

                bl.RemoveParcel(bl.displayParcel(parcel.ID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Success!", "The Parcel is removed", MessageBoxButton.OK, MessageBoxImage.Information);
            //this.Combo_SenderId.SelectedItem = null;
            //this.Combo_TargetId.SelectedItem = null;
            //this.Combo_SenderId.ItemsSource = bl.AllSenders_Id();
            //this.Combo_TargetId.ItemsSource = bl.AllTargets_Id();
            ParcelsListView.ItemsSource = bl.displayParcelList();
            RemoveGrid.Visibility = Visibility.Hidden;

        }

        #endregion

        
    }
}
