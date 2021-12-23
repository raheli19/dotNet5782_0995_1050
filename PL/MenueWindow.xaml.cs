﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MenueWindow.xaml
    /// </summary>
    public partial class MenueWindow : Window
    {
        internal readonly BLApi.IBL bl = BLFactory.GetBL();
        public MenueWindow()
        {
            InitializeComponent();
        }

        private void OpenClientWindow(object sender, RoutedEventArgs e) 
        {
            new SignUpSignIn(bl).Show();
        }

        private void OpenWorkerWindow(object sender, RoutedEventArgs e)
        { 
          new MainWindow(bl).Show();
        }
    }
}
