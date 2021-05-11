using EmployeeFinder_Client.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace EmployeeFinder_Client.ViewModel
{
    public class MessagerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ObservableCollection<Message> ChatList { get; set; }
        public ObservableCollection<User> users { get; set; }
        private string Login { get; set; }
        private TcpClient Client;
        private bool IsFormClosed;

        /// <summary>
        /// Выбранный контакт
        /// </summary>
        private User _SelectedUser;
        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                ListMessage();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedUser)));
            }
        }


        /// <summary>
        /// конструктор страницы
        /// </summary>
        public MessagerModel(string login, TcpClient client)
        {
            IsFormClosed = false;

            Client = client;
            Message message = new Message() { MessageProcessing = "SAMG", Login = login };
            MessagesAsistant.SendMessage(Client, message);

            users = new ObservableCollection<User>();
            ChatList = new ObservableCollection<Message>();           
            Login = login;
            ReceiveAllMessages();

            Thread updateThread = new Thread(Updater);
            updateThread.IsBackground = true;
            updateThread.Start();
        }
        private void ReceiveAllMessages()
        {
            Message answer = MessagesAsistant.ReadMessage(Client);
            if (answer.MessageProcessing == "ALOK")
            {
                UsersList((answer.obj as JArray).ToObject<List<Message>>());
            }
        }
        private void Updater()
        {
            while (!IsFormClosed)
            {
                Thread.Sleep(1500);
                int messagesCount = 0;
                foreach (User user in users)
                {
                    messagesCount += user.messages.Count;
                }
                MessagesAsistant.SendMessage(Client, new Message() { Login = this.Login, MessageProcessing = "UPNM", MessageText = messagesCount.ToString() });
                Message answer = MessagesAsistant.ReadMessage(Client);
                if (answer.MessageProcessing == "ALOK")
                {
                    Action action = () => UsersList((answer.obj as JArray).ToObject<List<Message>>());
                    System.Windows.Application.Current.Dispatcher.Invoke(action);
                    action = () => ListMessage();
                    System.Windows.Application.Current.Dispatcher.Invoke(action);
                }                  
            }
        }

        /// <summary>
        /// Ввод сообщения
        /// </summary>
        private string _InputMessageUser;
        public string InputMessageUser
        {
            get { return _InputMessageUser; }
            set
            {
                _InputMessageUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputMessageUser)));
            }
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        private RelayCommand _SendMessageCommand;
        public RelayCommand SendMessageCommand
        {
            get
            {
                return _SendMessageCommand = _SendMessageCommand ??
                  new RelayCommand(OnSendUC, CanSendUC);
            }
        }
        private bool CanSendUC()
        {
            return true;
        }
        private void OnSendUC()
        {
            if (_SelectedUser != null)
            {
                if (_InputMessageUser != "" && _InputMessageUser != " ")
                {
                    Message message = new Message()
                    {
                        FromWhom = $"{Login}",
                        ToWhom = $"{_SelectedUser.Сontact}",
                        MessageText = $"{_InputMessageUser}",
                        SentMessage = true,
                        MessageProcessing = "RECM",
                        obj = DateTime.Now.ToShortTimeString()
                    };
                    SelectedUser.messages.Add(message);

                    //Отправка на сервер
                    MessagesAsistant.SendMessage(Client, message);

                    Thread thread = new Thread(ReceiveMethod);
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }
        private void ReceiveMethod()
        {
            Message answer = MessagesAsistant.ReadMessage(Client);
            if (answer.MessageProcessing == "SAVM")
            {
                InputMessageUser = "";
                Action action = () => ListMessage();
                System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
        }


        /// <summary>
        /// Отправка сообщения
        /// </summary>
        private RelayCommand _CloseWindow;
        public RelayCommand CloseWindow
        {
            get
            {
                return _CloseWindow = _CloseWindow ??
                  new RelayCommand(OnClose, CanClose);
            }
        }
        private bool CanClose()
        {
            return true;
        }
        private void OnClose()
        {
            IsFormClosed = true;
        }

        //Сортировка сообщений по контактам
        private void UsersList(List<Message> messages)
        {
            //users.Clear();
            foreach (var item in messages)
            {
                if (item.FromWhom == Login)
                {
                    if (users.Count == 0)
                    {
                        User user = new User(item.ToWhom);
                        item.SentMessage = true;
                        user.messages.Add(item);
                        users.Add(user);
                    }
                    else
                    {
                        bool checkCont = false;
                        foreach (var item1 in users)
                        {
                            if (item.ToWhom == item1.Сontact)
                            {
                                item.SentMessage = true;
                                item1.messages.Add(item);
                                checkCont = true;
                                break;
                            }
                        }
                        if (checkCont == false)
                        {
                            User user = new User(item.ToWhom);
                            item.SentMessage = true;
                            user.messages.Add(item);
                            users.Add(user);
                        }
                    }
                }
                else if (item.ToWhom == Login)
                {
                    if (users.Count == 0)
                    {
                        User user = new User(item.FromWhom);
                        user.messages.Add(item);
                        users.Add(user);
                    }
                    else
                    {
                        bool checkCont = false;
                        foreach (var item1 in users)
                        {
                            if (item.FromWhom == item1.Сontact)
                            {
                                item1.messages.Add(item);
                                checkCont = true;
                                break;
                            }
                        }
                        if (checkCont == false)
                        {
                            User user = new User(item.FromWhom);
                            user.messages.Add(item);
                            users.Add(user);
                        }
                    }
                }
            }
        }

        //Список сообщений выбранного контакта
        public void ListMessage()
        {
            ChatList.Clear();
            foreach (var item in SelectedUser.messages)
            {
                ChatList.Add(item);
            }
        }
    }
}
