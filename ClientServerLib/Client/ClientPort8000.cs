using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLib.Client
{
    public class ClientPort8000
    {
        public string Status { get; set; }
        private string ipAddress;
        private int port;

        public ClientPort8000(string _ipAddress, int _port)
        {
            ipAddress = _ipAddress;
            port = _port;
        }

        public Account RegistrationAccount(Account account)
        {
            TcpClient client = new TcpClient(ipAddress, port);
            NetworkStream stream = client.GetStream();

            //Передаем уникальный ID на сервер 1
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write("registration");
            writer.Write(account.Unique_Id);
            writer.Flush();

            //Взамен получаем уникальный код
            BinaryReader reader = new BinaryReader(stream);
            account.Unique_Code = reader.ReadString();
            Status = "OK";

            reader.Close();
            writer.Close();

            return account;
        }
    }
}
