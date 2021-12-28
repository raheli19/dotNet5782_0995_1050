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
        private ObservableCollection<BO.ParcelDescription> boParcelList = new ObservableCollection<BO.ParcelDescription>();
        private bool checkFlag = false;
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
            List<ParcelDescription> parcelsBySender = new List<ParcelDescription>();
            foreach (var item in boParcelList)
            {
                parcelsBySender.Add(new ParcelDescription() { Id = item.Id, SenderName = item.SenderName, TargetName = item.TargetName, weight = item.weight, Status = item.Status, priority = item.priority});

            }
            List<ParcelDescription> parcelsBYSender = new List<ParcelDescription>();
            parcelsBYSender = parcelsBySender.OrderBy(d => d.SenderName).ToList();
            FilterBySender.ItemsSource = parcelsBYSender;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilterBySender.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Add(groupDescription);



            List<ParcelDescription> parcelsByTarget = new List<ParcelDescription>();
            foreach (var item in boParcelList)
            {
                parcelsByTarget.Add(new ParcelDescription() { Id = item.Id, SenderName = item.SenderName, TargetName = item.TargetName, weight = item.weight, Status = item.Status, priority = item.priority });

            }
            List<ParcelDescription> parcelsBYTarget = new List<ParcelDescription>();
            parcelsBYTarget = parcelsByTarget.OrderBy(d => d.TargetName).ToList();
            FilterByTarget.ItemsSource = parcelsBYTarget;

            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(FilterByTarget.ItemsSource);
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("TargetName");
            view2.GroupDescriptions.Add(groupDescription2);
            //combobox?
        }

        private void ParcelView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(ParcelsListView.SelectedItem, bl, ParcelsListView).Show();
        }

        private void FilterBySender_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(FilterBySender.SelectedItem, bl, ParcelsListView).Show();
        }

            #region closeFunctions
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

        private void button_addParcel(object sender, RoutedEventArgs e)
        {
            ParcelWindow subWindow = new ParcelWindow(bl, ParcelsListView);
            subWindow.Show();
        }

        private void FilterByTarget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(FilterByTarget.SelectedItem, bl, ParcelsListView).Show();
        }

        private void FilterBySender1_Click(object sender, RoutedEventArgs e)
        {
            FilterBySender.Visibility = Visibility.Visible;
            FilterByTarget.Visibility = Visibility.Hidden;
            ParcelsListView.Visibility = Visibility.Hidden;
        }

        private void FilterByTarget1_Click(object sender, RoutedEventArgs e)
        {
            FilterByTarget.Visibility = Visibility.Visible;
            FilterBySender.Visibility = Visibility.Hidden;
            ParcelsListView.Visibility = Visibility.Hidden;
        }

        private void clearGrouping_Click(object sender, RoutedEventArgs e)
        {
            FilterBySender.Visibility = Visibility.Hidden;
            FilterByTarget.Visibility = Visibility.Hidden;
            ParcelsListView.Visibility= Visibility.Visible; 
        }
    }
}
