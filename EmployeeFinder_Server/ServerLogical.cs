using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeFinder_Server
{
    public class ServerLogical
    {
        private DBController controller;

        public ServerLogical()
        {
            controller = new DBController();
            NewThread();
        }

        /// <summary>
        /// Ожидает подключение новых пользователей и в новом потоке начинает с ними работать
        /// </summary>
        private void NewThread()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1024);
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
                        MessageBox.Show(ex.Message);
                    }
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        /// <summary>
        /// Пием сообщений и их перенаправление
        /// </summary>
        /// <param name="client"> клиент от каторого пришло сообщение</param>
        /// <param name="controller">клас взаемодействий с базой данных</param>
        private void Logical(TcpClient client, DBController controller)
        {
            Message message = MessagesAsistent.ReadMessage(client);
            try
            {
                switch (message.MessageProcessing)
                {
                    case "LOGC": { MessagesAsistent.SendMessage(client, controller.IsLoginCorrectCompany(message)); } break;
                    case "LOGE": { MessagesAsistent.SendMessage(client, controller.IsLoginCorrectEmployee(message)); } break;
                    case "REGC": { MessagesAsistent.SendMessage(client, controller.RegisterCompany(message)); } break;
                    case "REGE": { MessagesAsistent.SendMessage(client, controller.RegisterEmployee(message)); } break;
                    case "SMES": { } break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает список кандидатов
        /// </summary>
        /// <returns>список кандидатов</returns>
        public object GetCandidates()
        {
            return controller.GetCandidates();
        }

        /// <summary>
        /// Возвращает список городов
        /// </summary>
        /// <returns>список городов</returns>
        public object GetCities()
        {
            return controller.GetCities();
        }

        /// <summary>
        /// Возвращает список компаний
        /// </summary>
        /// <returns>список компаний</returns>
        public object GetCompanies()
        {
            return controller.GetCompanies();
        }

        /// <summary>
        /// Возвращает список пожеланий компании
        /// </summary>
        /// <returns>список пожеланий компании</returns>
        public object GetCompaniesWishLists()
        {
            return controller.GetCompaniesWishLists();
        }

        /// <summary>
        /// Возвращает список специальностей
        /// </summary>
        /// <returns>список специальностей</returns>
        public object GetSpecialisations()
        {
            return controller.GetSpecialisations();
        }

        /// <summary>
        /// Возвращает список сообщений
        /// </summary>
        /// <returns>список сообщений</returns>
        public object GetMessages()
        {
            return controller.GetMessages();
        }
    }
}