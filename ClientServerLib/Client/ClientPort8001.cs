using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLib.Client
{
    public class ClientPort8001
    {
        public string Status { get; set; }
        private string ipAddress;
        private int port;

        public ClientPort8001(string _ipAddress, int _port)
        {
            ipAddress = _ipAddress;
            port = _port;
        }
        public Account SendMessage(Account account)
        {
            TcpClient client = new TcpClient(ipAddress, port);
            NetworkStream stream = client.GetStream();

            //Передаем сообщение, уникальный код и уникальный id
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(account.Unique_Id);
            writer.Write(account.Unique_Code);
            writer.Write(account.Message);

            //Взамен получаем сообщение об отправке
            BinaryReader reader = new BinaryReader(stream);
            Status = reader.ReadString();

            reader.Close();
            writer.Close();

            return account;
        }
    }
}
