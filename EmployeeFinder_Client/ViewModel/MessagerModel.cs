using EmployeeFinder_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;

namespace EmployeeFinder_Client.ViewModel
{
    public class MessagerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        //private IMainWindowsCodeBehind _MainCodeBehind;

        public ObservableCollection<Message> ChatList { get; set; }
        public ObservableCollection<User> users { get; set; }

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

        private string Login { get; set; }
        /// <summary>
        //конструктор страницы
        /// </summary>
        public MessagerModel(string login, TcpClient client)//IMainWindowsCodeBehind codeBehind)
        {
            Message message = new Message() { MessageProcessing = "SAMG", Login = login };
            MessagesAsistant.SendMessage(client, message);

            //DataAccess dataAccess = new DataAccess();//сообщения для тестов
            users = new ObservableCollection<User>();
            ChatList = new ObservableCollection<Message>();
            
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "ALOK")
            {
                UsersList(answer.obj as List<Message>);
            }
            
            //Login = "User1";//заглушка для тестов
            //UsersList(dataAccess.bufusers);//сообщения для тестов 

            //if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            //_MainCodeBehind = codeBehind;
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
                if (_InputMessageUser != "")
                {
                    Message message = new Message()
                    {
                        FromWhom = $"{Login}",
                        ToWhom = $"{_SelectedUser}",
                        MessageText = $"{_InputMessageUser}",
                        SentMessage = true
                    };
                    SelectedUser.messages.Add(message);
                    ListMessage();
                    //
                    //Отправка на сервер тут
                    //
                    InputMessageUser = "";
                }
            }
        }

        //Сортировка сообщений по контактам
        private void UsersList(List<Message> messages)
        {
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
