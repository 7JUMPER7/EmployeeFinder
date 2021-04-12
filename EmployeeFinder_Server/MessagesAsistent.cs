using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server
{
    internal static class MessagesAsistent
    {
        /// <summary>
        /// Получение даных из TcpClient и их конвертация в класс MyMessage
        /// </summary>
        /// <param name="client">клиент у каторого мы получаем данные</param>
        /// <returns>обьект класса MyMessage</returns>
        public static Message ReadMessage(TcpClient client)
        {
            return new BinaryFormatter().Deserialize(client.GetStream()) as Message;
        }

        /// <summary>
        /// Отппавка сообщения конкретному клиенту
        /// </summary>
        /// <param name="client"> клиент каторому вы будите отправлять сообщение</param>
        /// <param name="message">обьект класса сообщения</param>
        public static void SendMessage(TcpClient client, Message message)
        {
            new BinaryFormatter().Serialize(client.GetStream(), message);
        }
    }
}