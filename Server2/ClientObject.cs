using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientServerLib;

namespace ClientServerLib
{
    public class ClientObject
    {
        private TcpClient client;
        private LogServer logServer;
        static object locker;

        public ClientObject(TcpClient _tcpClient, LogServer _logServer, object _locker)
        {
            client = _tcpClient;
            logServer = _logServer;
            locker = _locker;
        }

        //Принять сообщение на сервер 2
        public void ProcessAcceptMessage()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);

                //Считываем данные из потока, аккаунт отправителя сообщения
                Account accountSender = new Account();
                accountSender.Unique_Id = reader.ReadString();
                accountSender.Unique_Code = reader.ReadString();
                accountSender.Message = reader.ReadString();

                //проверка аккаунта ,есть ли такой в списке?
                //Обратимся к серверу на порту 8000
                var status = VerificationAccount(accountSender);

                BinaryWriter writer = new BinaryWriter(stream);
                if (status)
                {
                    string message = $"Собщение получено от Unique_Id: {accountSender.Unique_Id} Unique_Code: {accountSender.Unique_Code} \nMessage: {accountSender.Message}";
                    writer.Write("Сообщение получено");
                    Console.WriteLine(message);

                    lock (locker)
                        logServer.WriteLog(accountSender.Message, accountSender.Unique_Id);
                }
                else
                {
                    string message = "Ошибка авторизации неверен id или код";
                    writer.Write(message);
                    Console.WriteLine(message);
                }
                writer.Flush(); //Очистка буфера

                writer.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
        /// <summary>
        /// Верификация аккаунта через 1 сервер
        /// </summary>
        /// <returns></returns>
        private bool VerificationAccount(Account account)
        {
            bool status = false;
            TcpClient tcpClient = null;
            try
            {
                tcpClient = new TcpClient("127.0.0.1", 8000);
                NetworkStream stream = tcpClient.GetStream();

                //Передаем команду на сервер 1
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write("verification");
                writer.Write(account.Unique_Id);
                writer.Write(account.Unique_Code);
                writer.Flush();

                //Получаем ответ от сервара 1
                BinaryReader reader = new BinaryReader(stream);
                string st = reader.ReadString();
                if (st == "success")
                    status = true;

                reader.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VerificationAccount: {ex.Message}");
            }
            return status;
        }
    }
}
