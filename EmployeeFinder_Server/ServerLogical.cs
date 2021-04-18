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

            MessageBox.Show((controller.GetCandidates() as List<Candidates>)[0].SpecialisationId.ToString());
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
                    case "LOGC":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                           {
                               Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked for login as a company");
                           });
                            MessagesAsistent.SendMessage(client, controller.IsLoginCorrectCompany(message));
                        }
                        break;
                    case "LOGE":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked for login as an employee");
                            });
                            MessagesAsistent.SendMessage(client, controller.IsLoginCorrectEmployee(message));
                        }
                        break;
                    case "REGC":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked to register as a company");
                            });
                            MessagesAsistent.SendMessage(client, controller.RegisterCompany(message));
                        }
                        break;
                    case "REGE":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked to register as an employee");
                            });
                            MessagesAsistent.SendMessage(client, controller.RegisterEmployee(message));
                        }
                        break;
                    case "RECE":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked for employees");
                            });
                            MessagesAsistent.SendMessage(client, MessageGetCandidates("RECE", message, controller.GetCandidates()));
                        }
                        break;
                    case "RECC":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked for cities");
                            });
                            MessagesAsistent.SendMessage(client, MessageGetCandidates("RECC", message, controller.GetCities()));
                        }
                        break;
                    case "RECS":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked for specializations");
                            });
                            MessagesAsistent.SendMessage(client, MessageGetCandidates("RECS", message, controller.GetSpecialisations()));
                        }
                        break;
                    case "PUBL":
                        {
                            Form.ConsoleBox.Invoke((MethodInvoker)delegate
                            {
                                Form.ConsoleBox.Items.Add($"{DateTime.Now}: {message.Login} asked for updating CV");
                            });
                            MessagesAsistent.SendMessage(client, controller.SaveEmployeeInfo(message)); break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
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