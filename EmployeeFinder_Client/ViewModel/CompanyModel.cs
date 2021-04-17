﻿using EmployeeFinder_Client.Model;
using EmployeeFinder_Client.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace EmployeeFinder_Client.ViewModel
{
    public class CompanyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private IMainWindowsCodeBehind _MainCodeBehind;

        public List<string> CityFilter { get; set; }
        public List<string> SpecFilter { get; set; }
        public List<Candidates> Candidates { get; set; }

        public bool NewMessage { get; set; }

        /// <summary>
        //конструктор страницы
        /// </summary>
        public CompanyModel(IMainWindowsCodeBehind codeBehind)
        {
            //DataAccess data = new DataAccess();
            //CityFilter = data.CityFilter;
            //SpecFilter = data.SpecFilter;

            //Тестовые клиенты
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
            CityFilter = ReceiveCities(client);
            client.Close();

            client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
            SpecFilter = ReceiveSpecs(client);
            client.Close();

            client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
            Candidates = ReceiveCandidates(client);
            client.Close();

            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
        }

        /// <summary>
        //Значения фильтра возраста
        /// </summary>
        private int _FromAgeFilter;
        public int FromAgeFilter
        {
            get { return _FromAgeFilter; }
            set
            {
                _FromAgeFilter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FromAgeFilter)));
            }
        }
        private int _ToAgeFilter;
        public int ToAgeFilter
        {
            get { return _ToAgeFilter; }
            set
            {
                _ToAgeFilter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FromAgeFilter)));
            }
        }

        /// <summary>
        /// Открытие нового окна Messager
        /// </summary>
        private RelayCommand _OpenMessagerCommand;
        public RelayCommand OpenMessagerCommand
        {
            get
            {
                return _OpenMessagerCommand = _OpenMessagerCommand ??
                  new RelayCommand(OnMessagerrUC, CanMessagerUC);
            }
        }
        private bool CanMessagerUC()
        {
            return true;
        }
        private void OnMessagerrUC()
        {
            Messager messager = new Messager();
            messager.Height = 400;
            messager.Width = 400;
            messager.Show();
        }


        /// <summary>
        /// Получает массив доступных названий городов от сервера
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<string> ReceiveCities(TcpClient client)
        {
            MessagesAsistant.SendMessage(client, new Message() { Login = CurrentUser.CurrentUserLogin, MessageProcessing = "RECC" });
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "RECC")
            {
                if (answer.obj is JArray && answer.obj != null)
                {
                    return (answer.obj as JArray).ToObject<List<string>>();
                }
            }
            return null;
        }
        /// <summary>
        /// Получает массив доступных названий специализаций от сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<string> ReceiveSpecs(TcpClient client)
        {
            MessageBox.Show(CurrentUser.CurrentUserLogin);
            MessagesAsistant.SendMessage(client, new Message() { Login = CurrentUser.CurrentUserLogin, MessageProcessing = "RECS" });
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "RECS")
            {
                if (answer.obj is JArray && answer.obj != null)
                {
                    return (answer.obj as JArray).ToObject<List<string>>();
                }
            }
            return null;
        }
        /// <summary>
        /// Получает массив всех работников от сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<Candidates> ReceiveCandidates(TcpClient client)
        {
            MessagesAsistant.SendMessage(client, new Message() { Login = CurrentUser.CurrentUserLogin, MessageProcessing = "RECE" });
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "RECE")
            {
                if (answer.obj is JArray && answer.obj != null)
                {
                    return (answer.obj as JArray).ToObject<List<Candidates>>();
                }
            }
            return null;
        }
    }
}
