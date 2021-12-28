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
            this.Combo_priority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            this.Combo_weight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

            //combobox?
        }

        private void ParcelView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //open the ParcelWindow
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

        #region selections
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

    }
}
