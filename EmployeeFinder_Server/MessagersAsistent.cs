using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server
{
    internal static class MessagersAsistent
    {
        /// <summary>
        /// Получение даных из TcpClient и их конвертация в строку
        /// </summary>
        /// <param name="client">клиент у каторого мы получаем данные</param>
        /// <returns>Конвертированая строка</returns>
        public static string ReadMessage(TcpClient client)
        {
            byte[] bytes = new byte[256];
            NetworkStream network = client.GetStream();
            network.Read(bytes, 0, bytes.Length);
            return Encoding.Unicode.GetString(bytes).Replace("\0", "");
        }

        /// <summary>
        /// Отправка сообщений конкретному клиенту
        /// </summary>
        /// <param name="network">Поток по каторому будут отправленные данные</param>
        /// <param name="s">сообщение для клиента</param>
        public static void SendMessage(NetworkStream network, string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            network.Write(bytes, 0, bytes.Length);
        }
    }
}