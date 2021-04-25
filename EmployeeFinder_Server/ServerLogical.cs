using EmployeeFinder_Server.DbClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace EmployeeFinder_Server
{
    public class ServerLogical
    {
        private Form1 Form { get; set; }
        private DBController controller;

        public ServerLogical(Form1 form)
        {
            Form = form;
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
            try
            {
                bool isOnline = true;
                while (isOnline)
                {
                    Message message = MessagesAsistent.ReadMessage(client);
                    switch (message.MessageProcessing)
                    {
                        case "LOGC": { ConsoleWrite(client, controller.IsLoginCorrectCompany(message), "asked for login as a company"); break; }
                        case "LOGE": { ConsoleWrite(client, controller.IsLoginCorrectEmployee(message), "asked for login as an employee"); break; }
                        case "REGC": { ConsoleWrite(client, controller.RegisterCompany(message), "asked to register as an company"); break; }
                        case "REGE": { ConsoleWrite(client, controller.RegisterEmployee(message), "asked to register as an employee"); break; }
                        case "RECE": { ConsoleWrite(client, MessageGetCandidates("RECE", message, controller.GetCandidates()), "asked for employees"); break; }
                        case "RECC": { ConsoleWrite(client, MessageGetCandidates("RECC", message, controller.GetCities()), "asked for cities"); break; }
                        case "RECS": { ConsoleWrite(client, MessageGetCandidates("RECS", message, controller.GetSpecialisations()), "asked for specializations"); break; }
                        case "PUBL": { ConsoleWrite(client, controller.SaveEmployeeInfo(message), "asked for updating CV"); break; }
                        case "RCBL": { ConsoleWrite(client, MessageGetCandidates("RCBL", message, controller.GetCandidateByLogin(message.Login)), "asked for candidate by login"); break; } //Receive candidate by login
                        case "DELE": { ConsoleWrite(client, controller.DeleteCandidate(message), $"try to delete"); break; } //Delete employee
                        case "EXIT": { client.Close(); isOnline = false; ConsoleWrite(message, "close connection"); break; } //Close connection
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrite(ex.Message);
            }
        }

        /// <summary>
        /// отображает данные в консоли сервера
        /// </summary>
        /// <param name="client">клиент каторему нужно отправить сообщение</param>
        /// <param name="message">сообщение каторое нужно отправить клиенту</param>
        /// <param name="messageText">сообщение каторое нужно отобразить на консоли сервера</param>
        private void ConsoleWrite(TcpClient client, Message message, string messageText)
        {
            lock (Form)
            {
                Form.ConsoleBox.Invoke((MethodInvoker)delegate
                {
                    Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} {messageText}");
                });
            }
            MessagesAsistent.SendMessage(client, message);
        }
        /// <summary>
        /// отображает данные в консоли сервера
        /// </summary>
        /// <param name="message">сообщение каторое нужно отправить клиенту</param>
        /// <param name="messageText">сообщение каторое нужно отобразить на консоли сервера</param>
        private void ConsoleWrite(Message message, string messageText)
        {
            lock (Form)
            {
                Form.ConsoleBox.Invoke((MethodInvoker)delegate
                {
                    Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} {messageText}");
                });
            }
        }
        /// <summary>
        /// отображает данные в консоли сервера
        /// </summary>
        /// <param name="messageText">сообщение каторое нужно отобразить на консоли сервера</param>
        private void ConsoleWrite(string messageText)
        {
            lock (Form)
            {
                Form.ConsoleBox.Invoke((MethodInvoker)delegate
                {
                    Form.ConsoleBox.Items.Add($"{DateTime.Now}: {messageText}");
                });
            }
        }

        /// <summary>
        ///  Добавляет колекцию данных, в виде обьекта, в клас Message
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="obj">колекция в виде обьекта</param>
        /// <returns>сообщение в каторое сохранена колекция</returns>
        private Message MessageGetCandidates(string key, Message message, object obj)
        {
            message.MessageProcessing = key;
            message.obj = obj;
            return message;
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

        ~ServerLogical()
        {
            using (StreamWriter sw = new StreamWriter($"{DateTime.Today.Year}_{DateTime.Today.Month}_{DateTime.Today.Day}_logs.txt", true))
            {
                foreach (var item in Form.ConsoleBox.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }
    }
}