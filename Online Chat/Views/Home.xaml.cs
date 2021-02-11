using System;
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
            (DataContext as HomeViewModel).OnSuccessfulConnect += InitializeChatWindow;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                this.DragMove();
            }
        }

        private void MessageRequest(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void InitializeChatWindow(object sender, ConnectEventArgs e)
        {
            MainChat chat = new MainChat(e.ChatVM);
            (DataContext as HomeViewModel).Alert -= MessageRequest;
            (DataContext as HomeViewModel).OnSuccessfulConnect -= InitializeChatWindow;
            this.Close();
        }

    }
}
