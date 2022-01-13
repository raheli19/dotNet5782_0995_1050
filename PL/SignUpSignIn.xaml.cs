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

namespace PL
{
    /// <summary>
    /// Logique d'interaction pour SignUpSignIn.xaml
    /// </summary>
    public partial class SignUpSignIn : Window
    {
        private BLApi.IBL bl;

        #region CTOR
        public SignUpSignIn(BLApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
        }
        #endregion

        #region Button_SignIn_Click
        /// <summary>
        /// open the sign in window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SignIn_Click(object sender, RoutedEventArgs e)
        {
            new Click_SignUp_In(bl).Show();

        }
        #endregion

        #region Button_SignUp_Click
        /// <summary>
        /// opens the sign up window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SignUp_Click(object sender, RoutedEventArgs e)
        {
            new Click_SignUp_In(bl,3).Show();
        }
        #endregion
    }
}
