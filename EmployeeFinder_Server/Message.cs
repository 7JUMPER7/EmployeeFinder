using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server
{
    [Serializable]
   public  class Message
    {
        private string messageProcessing;
        
        /// <summary>
        /// Логин пользователяы
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// должен хранить в себе Особую четырех символьную команду каторая определяет что делать с остальным содержимим класса
        /// </summary>
        public string MessageProcessing
        {
            get
            {
                return messageProcessing;
            }
            set
            {
                if (value.Length == 4)
                    foreach (char c in value)
                        if (char.IsUpper(c))
                            messageProcessing = value;
            }
        }
        /// <summary>
        /// текст сообщения
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// от кого сообщение
        /// </summary>
        public string FromWhom { get; set; }

        /// <summary>
        /// Кому сообщение
        /// </summary>
        public string ToWhom { get; set; }

        /// <summary>
        /// файл
        /// </summary>
        public byte[] File { get; set; }
        /// <summary>
        /// клиент
        /// </summary>
        public TcpClient client { get; set; }
        /// <summary>
        /// обьект
        /// </summary>
        public object obj { get; set; }

        public Message()
        {
            File = null;
            client = null;
        }

        public override string ToString()
        {
            return MessageText;
        }
    }
}
