using ClientServerLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server1
{
    /// <summary>
    /// 1) Порты 8000 и 8001
    /// 2) При запуске клиент выбирает для себя уникальный идентификатор
    /// 3) 3.1 Клиент подключается к серверу к порту 8000
    ///    3.2 и передает ему свой идентификатор
    ///    3.3 и получает от сервера уникальный код
    /// 4) 4.1 Клиент подключается к серверу к порту 8001
    ///    4.2 и передает произвольное текстовое сообщение, свой идентификатор и код, полученный на шаге 2
    /// 5) Если переданный код не соответствует его уникальному идентификатору, то сервер выдает сообщение об ошибке
    /// 6) Если код передан правильно, сервер записывает полученное сообщение в лог 
    /// </summary>
    class Program
    {
        private const int port = 8000; //порт для прослушивания подключений 
        private static TcpListener listener;
        private static List<Account> accounts;
        static void Main(string[] args)
        {
            accounts = new List<Account>();

            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                Console.WriteLine("Ожидание подключений к серверу 1...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client, accounts);

                    //создаем поток для обслуживания нового клиента
                    Task clientTask = new Task(clientObject.ProcessGetComands);//ProcessGetCode);
                    clientTask.Start(); //TaskScheduler там есть

                    Console.WriteLine(accounts.Count);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(listener !=null)
                    listener.Stop();
            }
        }
    }
}
