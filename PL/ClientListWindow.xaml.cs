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
    /// Interaction logic for ClientListWindow.xaml
    /// </summary>
    public partial class ClientListWindow : Window
    {
        private BLApi.IBL bl;
        IEnumerable<ClientActions> clientListFromBo = new List<ClientActions>();

        private ObservableCollection<BO.Client> boClientList = new ObservableCollection<BO.Client>();

        public ClientListWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            clientListFromBo = bl.displayClientList();
            DataContext = boClientList;
            foreach (var item in bl.displayDroneList())
            {
                boClientList.Add(bl.displayClient((item.Id)));


            }
            ClientListView.ItemsSource = clientListFromBo;
        }

        private void ClientListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClientWindow subWindow = new ClientWindow(bl, ClientListView);
            subWindow.Show();
        }
    }
}
