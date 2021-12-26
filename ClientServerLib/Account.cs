using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLib
{
    public class Account
    {
        /// <summary>
        /// Уникальный ID
        /// </summary>
        public string Unique_Id { get; set; }

        /// <summary>
        /// Уникальный код
        /// </summary>
        public string Unique_Code { get; set; }

        /// <summary>
        /// Сообщение отправленное через сервер с портом 8001
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// Генерация униального ID
        /// </summary>
        public void UniqueIdGeneration()
        {
            Unique_Id = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Генерация уникального кода
        /// </summary>
        /// <returns></returns>
        public void UniqueCodeGeneration()
        {
            Unique_Code = Guid.NewGuid().ToString();
        }
    }
}
