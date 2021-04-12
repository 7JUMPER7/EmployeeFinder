using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeFinder_Server
{
    internal class ServerLogical
    {
        private static DBController controller;

        private ServerLogical()
        {
            controller = new DBController();
            NewThread();
        }

        /// <summary>
        /// Ожидает подключение новых пользователей и в новом потоке начинает с ними работать
        /// </summary>
        private static void NewThread()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8081);
            server.Start();

            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (server.AcceptTcpClient() is TcpClient client)
                            new Thread(() => { Logical(client, controller); })
                            {
                                IsBackground = true
                            }.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }).Start();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="controller"></param>
        private static void Logical(TcpClient client, DBController controller)
        {
            Message message = MessagesAsistent.ReadMessage(client);
            try
            {
                while (true)
                {
                    switch (message.MessageProcessing)
                    {
                        case "LOGC": { MessagesAsistent.SendMessage(client, controller.IsLoginCorrectCompany(message)); } break;
                        case "LOGE": { MessagesAsistent.SendMessage(client, controller.IsLoginCorrectEmployee(message)); } break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}