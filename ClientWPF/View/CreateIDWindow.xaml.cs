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
using ClientServerLib;

namespace ClientWPF.View
{
    /// <summary>
    /// Логика взаимодействия для CreateIDWindow.xaml
    /// </summary>
    public partial class CreateIDWindow : Window
    {
        private Account account;
        public CreateIDWindow(Account _account)
        {
            InitializeComponent();
            account = _account;
        }

        private void BT_CreateGuid_Click(object sender, RoutedEventArgs e)
        {
            account.UniqueIdGeneration();
            TB_Unique_Id.Text = account.Unique_Id;

            //string unique_Id = Guid.NewGuid().ToString();
            //TB_Unique_Id.Text = unique_Id;
        }
    }
}
