using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EmployeeFinder_Client
{
    internal static class MessagesAsistant
    {
        /// <summary>
        /// Получение даных из TcpClient и их конвертация в класс MyMessage
        /// </summary>
        /// <param name="client">клиент у каторого мы получаем данные</param>
        /// <returns>обьект класса MyMessage</returns>
        public static Message ReadMessage(TcpClient client)
        {
            string json = ReadFromStream(client.GetStream());
            Message buf = JsonConvert.DeserializeObject<Message>(json);
            //Message buf = new BinaryFormatter().Deserialize(client.GetStream()) as Message;
            return buf;
        }

        /// <summary>
        /// Отппавка сообщения конкретному клиенту
        /// </summary>
        /// <param name="client"> клиент каторому вы будите отправлять сообщение</param>
        /// <param name="message">обьект класса сообщения</param>
        public static void SendMessage(TcpClient client, Message message)
        {
            string json = JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented);
            WriteToStream(client.GetStream(), json);
            //new BinaryFormatter().Serialize(client.GetStream(), message);
        }

        //Работа с потоком
        private static string ReadFromStream(NetworkStream stream)
        {
            byte[] buf = new byte[1024];
            int len = 0, sum = 0;
            List<byte> allBytes = new List<byte>();
            do
            {
                len = stream.Read(buf, 0, buf.Length);
                allBytes.AddRange(buf);
                sum += len;
            } while (len >= buf.Length);
            return Encoding.Unicode.GetString(allBytes.ToArray(), 0, sum);
        }
        private static void WriteToStream(NetworkStream stream, string message)
        {
            byte[] buf = Encoding.Unicode.GetBytes(message);
            stream.Write(buf, 0, buf.Length);
        }
    }
}