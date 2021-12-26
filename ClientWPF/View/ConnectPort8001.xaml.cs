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
    /// Логика взаимодействия для ConnectPort8001.xaml
    /// </summary>
    public partial class ConnectPort8001 : Window
    {
        private ClientPort8001 clientPort8001;
        private Account account;
        private int port = 8001;
        private string address = "127.0.0.1";
        public ConnectPort8001(Account _account)
        {
            InitializeComponent();
            clientPort8001 = new ClientPort8001(address, port);
            account = _account;

            if (account.Unique_Id == null && account.Unique_Code == null)
            {
                TB_Unique_Id.Text = "Сгенерируйте уникальный ID!";
                TB_Unique_Code.Text = "Получите уникальный код!";
            }
            else if (account.Unique_Id == null)
            {
                TB_Unique_Id.Text = "Сгенерируйте уникальный ID!";
                TB_Unique_Code.Text = account.Unique_Code;
            }
            else if (account.Unique_Code == null)
            {
                TB_Unique_Id.Text = account.Unique_Id;
                TB_Unique_Code.Text = "Получите уникальный код!";
            }
            else
            {
                TB_Unique_Id.Text = account.Unique_Id;
                TB_Unique_Code.Text = account.Unique_Code;
            }
        }

        /// <summary>
        /// Отправить сообщение 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_send_a_message_Click(object sender, RoutedEventArgs e)
        {
            if (account.Unique_Id != null && account.Unique_Code != null)
            {
                if (TB_Message.Text != "Null")
                {
                    //для ручной подмены значений
                    account.Unique_Id = TB_Unique_Id.Text;
                    account.Unique_Code = TB_Unique_Code.Text;

                    account.Message = TB_Message.Text;
                    SendMessage();
                }
                else
                {
                    MessageBox.Show("Впишите текст сообщения для отправик");
                }
            }
            else
            {
                MessageBox.Show("Отправка сообщения невозможна", "Ошибка");
            }
        }

        private void SendMessage()
        {
            try
            {
                clientPort8001.SendMessage(account);
                LableStatus.Content = clientPort8001.Status;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка");
            }
        }
    }
}
