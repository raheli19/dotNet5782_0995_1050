using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        IEnumerable<ParcelDescription> parcelListFromBo = new List<ParcelDescription>();
        private ObservableCollection<BO.Parcel> boParcelList = new ObservableCollection<BO.Parcel>();


        public ParcelListWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            DataContext = boParcelList;
            foreach (var item in bl.displayParcelList())
            {
                boParcelList.Add(bl.displayParcel((item.Id)));


            }
            parcelListFromBo = bl.displayParcelList();
            ParcelsListView.ItemsSource = parcelListFromBo;
            //combobox?
        }

        private void ParcelView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //open the ParcelWindow
        }
    }
}
