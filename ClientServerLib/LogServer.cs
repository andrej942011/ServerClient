using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLib
{
    public class LogServer
    {
        private string path;
        public LogServer(string _path)
        {
            path = _path;
        }
        public void WriteLog(string text, string unique_Id)
        {
            string time = DateTime.Now.ToString();
            string textLog = $"{time} | {unique_Id} | {text}";
            //Console.WriteLine(textLog);

            WriteFile(textLog);
        }

        private void WriteFile(string text)
        {
            try
            {
                File.AppendAllText(path + $"LogFile_{DateTime.Now.ToShortDateString()}.txt", text + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} LogErr: {ex.Message}");
            }
            
        }
    }
}
