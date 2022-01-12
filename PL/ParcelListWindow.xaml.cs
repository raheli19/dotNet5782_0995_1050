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
            DataContext = boParcelList;
            ParcelsListView.DataContext = boParcelList;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
