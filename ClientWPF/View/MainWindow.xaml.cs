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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientServerLib;

namespace ClientWPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Account account;
        public MainWindow()
        {
            InitializeComponent();
            account = new Account();
        }

        private void BTCreateID_Click(object sender, RoutedEventArgs e)
        {
            CreateIDWindow createId = new CreateIDWindow(account);
            createId.Show();
        }

        private void BTConnectPort8000_Click(object sender, RoutedEventArgs e)
        {
            ConnectPort8000 connect = new ConnectPort8000(account);
            connect.Show();
        }

        private void BTConnectPort8001_Click(object sender, RoutedEventArgs e)
        {
            ConnectPort8001 connect = new ConnectPort8001(account);
            connect.Show();
        }

        private void BTExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
