using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
using ClientServerLib;
using ClientServerLib.Client;

namespace ClientWPF.View
{
    /// <summary>
    /// Логика взаимодействия для ConnectPort8000.xaml
    /// </summary>
    public partial class ConnectPort8000 : Window
    {
        private ClientPort8000 clientPort8000; 
        private Account account;
        private int port = 8000;
        private string address = "127.0.0.1";
        public ConnectPort8000(Account _account)
        {
            InitializeComponent();
            clientPort8000 = new ClientPort8000(address, port);
            account = _account;

            if (account.Unique_Id == null)
            {
                TB_Unique_Id.Text = "Сгенерируйте уникальный ID!";
            }
            else
            {
                TB_Unique_Id.Text = account.Unique_Id;
            }
            
        }

        private void BT_to_get_the_code_Click(object sender, RoutedEventArgs e)
        {
            if (account.Unique_Id != null)
            {
                RegistrationAccount();
                if (account.Unique_Code != null)
                {
                    TB_Unique_Code.Text = account.Unique_Code;
                }
            }
            else
            {
                MessageBox.Show("Генерация уникального кода невозможно, сгенерируйте уникальный ID");
            }
        }

        private void RegistrationAccount()
        {
            try
            {
                clientPort8000.RegistrationAccount(account);
                LableStatus.Content = clientPort8000.Status;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка");
            }
        }
    }
}
