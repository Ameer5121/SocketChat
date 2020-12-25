﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Online_Chat.ViewModels;
using Online_Chat.Events;

namespace Online_Chat.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
            (DataContext as HomeViewModel).Alert += MessageRequest;
            (DataContext as HomeViewModel).OnConnectOrHost += ChangeWindow;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MessageRequest(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void ChangeWindow(object sender, EventArgs e)
        {
            MainChat chat = new MainChat(DataContext as HomeViewModel);
            chat.Show();
            this.Hide();
        }
    }
}