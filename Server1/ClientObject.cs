using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientServerLib;

namespace Server1
{
    public class ClientObject
    {
        public TcpClient client;
        public List<Account> accounts;

        public ClientObject(TcpClient tcpClient, List<Account> _accounts)
        {
            client = tcpClient;
            accounts = _accounts;
        }

        /// <summary>
        /// Метод выбора команды
        /// </summary>
        public void ProcessGetComands()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                string comand = reader.ReadString(); //exeption
                if(comand == "registration")
                    GetUniqueCode(reader, stream);
                if (comand == "verification")
                    GetAccount(reader, stream);

                //reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        private void GetAccount(BinaryReader reader, NetworkStream stream)
        {
            bool verification = false;

            //считываем данные из потока
            string unique_Id = reader.ReadString();
            string unique_Code = reader.ReadString();

            foreach (var account in accounts)
            {
                if (account.Unique_Id == unique_Id && account.Unique_Code == unique_Code)
                    verification = true;
            }

            Console.WriteLine($"Авторизация пользователя {unique_Id} {unique_Code}");

            //Отправим ответ в виде уникального кода клиента
            BinaryWriter writer = new BinaryWriter(stream);
            if(verification)
                writer.Write("success");
            else
            {
                writer.Write("error");
            }
            writer.Flush(); //Очистка буфера

            writer.Close();
            reader.Close();
        }

        private void GetUniqueCode(BinaryReader reader, NetworkStream stream)
        {
            //считываем данные из потока
            string unique_Id = reader.ReadString();
            //создаем по полученным данным от клиента
            Account account = new Account();
            account.Unique_Id = unique_Id;
            //Сгенерируем униальный код клиента
            account.UniqueCodeGeneration();
            Console.WriteLine($"Зарегестрирован пользователь с Unique_Id: {account.Unique_Id}, Unique_Code: {account.Unique_Code}");

            //Добавим аккаунт в список заререстрированнных
            accounts.Add(account);

            //Отправим ответ в виде уникального кода клиента
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(account.Unique_Code);
            writer.Flush(); //Очистка буфера

            writer.Close();
            reader.Close();
        }

        /// <summary>
        /// Метод устарел!  Процесс регестрации пользователей и получение уникального кода
        /// </summary>
        public void ProcessGetCode()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                //считываем данные из потока
                string unique_Id = reader.ReadString();
                //создаем по полученным данным от клиента
                Account account = new Account();
                account.Unique_Id = unique_Id;
                //Сгенерируем униальный код клиента
                account.UniqueCodeGeneration();
                Console.WriteLine($"Зарегестрирован пользователь с Unique_Id: {account.Unique_Id}, Unique_Code: {account.Unique_Code}");

                //Добавим аккаунт в список заререстрированнных
                accounts.Add(account);

                //Отправим ответ в виде уникального кода клиента
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(account.Unique_Code);
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
    }
}
