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
    /// Interaction logic for ClientListWindow.xaml
    /// </summary>
    public partial class ClientListWindow : Window
    {
        private BLApi.IBL bl;

        private ObservableCollection<BO.ClientActions> boClientList = new ObservableCollection<BO.ClientActions>();
        private bool checkFlag = false;

        public ClientListWindow(BLApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            ClientListView.DataContext = boClientList;
            foreach (var item in bl.displayClientList())
            {
                boClientList.Add(item);

            }
            
        }
        #region ClientListView_MouseDoubleClick
        /// <summary>
        /// Opens clientwindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClientWindow subWindow = new ClientWindow(ClientListView.SelectedItem, bl, ClientListView);
            subWindow.Show();
        }
        #endregion

        #region AddClient_Click
        /// <summary>
        /// Opens client window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow subWindow = new ClientWindow(bl, ClientListView);

            subWindow.Show();
        }
        #endregion

        #region closeFct
        /// <summary>
        /// Button to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.checkFlag = true; // will allow us to close the window from the button and not from the "X"
            this.Close();
        }

        /// <summary>
        /// Close/Not the window
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

      
    }
}
