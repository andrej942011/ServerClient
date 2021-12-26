using ClientServerLib;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server2
{
    class Program
    {
        private const int port = 8001; //порт для прослушивания подключений 
        private static TcpListener listener;
        private static LogServer logServer;
        static object locker = new object();
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                string dir = args[0];
                if (!Directory.Exists(args[0]))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine("Каталог для сообщений создан");
                }

                try
                {
                    logServer = new LogServer(dir);
                    listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                    listener.Start();
                    Console.WriteLine("Ожидание подключений к серверу 2...");

                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        ClientObject clientObject = new ClientObject(client, logServer, locker);

                        //принять сообщение
                        Task clientTask = new Task(clientObject.ProcessAcceptMessage);
                        clientTask.Start(); //TaskScheduler там есть
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (listener != null)
                        listener.Stop();
                }
            }
            else
            {
                Console.WriteLine("Не верные аргументы переданные в приложение");
            }

            Console.ReadKey();
        }
    }
}
