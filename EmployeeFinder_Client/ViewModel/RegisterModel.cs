using EmployeeFinder_Client.Model;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace EmployeeFinder_Client.ViewModel
{
    public class RegisterModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IMainWindowsCodeBehind _MainCodeBehind;

        /// <summary>
        //конструктор страницы
        /// </summary>
        public RegisterModel(IMainWindowsCodeBehind codeBehind)
        {
            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
        }

        /// <summary>
        /// Ввод логина
        /// </summary>
        private string _InputLogin;
        public string InputLogin
        {
            get { return _InputLogin; }
            set
            {
                _InputLogin = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputLogin)));
            }
        }

        /// <summary>
        /// Ввод пароля
        /// </summary>
        private string _InputPassword;
        public string InputPassword
        {
            get { return _InputPassword; }
            set
            {
                _InputPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputPassword)));
            }
        }

        /// <summary>
        /// Повторный ввод пароля
        /// </summary>
        private string _InputReapeatPassword;
        public string InputReapeatPassword
        {
            get { return _InputReapeatPassword; }
            set
            {
                _InputReapeatPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputReapeatPassword)));
            }
        }

        /// <summary>
        /// Ввод названия компании
        /// </summary>
        private string _InputCompanyName;
        public string InputCompanyName
        {
            get { return _InputCompanyName; }
            set
            {
                _InputCompanyName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputCompanyName)));
            }
        }

        /// <summary>
        /// Проверка является ли компанией
        /// </summary>
        private bool _IsLikeCompanyCheck;

        public bool IsLikeCompanyCheck
        {
            get { return _IsLikeCompanyCheck; }
            set
            {
                if (_IsLikeCompanyCheck == value) return;
                _IsLikeCompanyCheck = value;
            }
        }

        /// <summary>
        /// Завершение регистрации
        /// </summary>
        private RelayCommand _RegisterCommand;
        public RelayCommand RegisterCommand
        {
            get
            {
                return _RegisterCommand = _RegisterCommand ??
                  new RelayCommand(OnRegUC, CanRegUC);
            }
        }
        private bool CanRegUC()
        {
            return true;
        }
        private void OnRegUC()
        {
            if (InputPassword == InputReapeatPassword)
            {
                TcpClient client = new TcpClient();
                try
                {
                    client.Connect("127.0.0.1", 1024);
                }
                catch (SocketException)
                {
                    _MainCodeBehind.ShowErrorWindow("Сервер не отвечает");
                    return;
                }
                if (InputLogin.Contains(" "))
                {
                    _MainCodeBehind.ShowErrorWindow("Есть пробелы в логине");
                    return;
                }
                Thread thread = new Thread(new ParameterizedThreadStart(CheckForLogin));
                thread.IsBackground = true;
                thread.Start(client);

                Message message = new Message()
                {
                    Login = InputLogin,
                    Password = InputPassword
                };
                if (IsLikeCompanyCheck)
                {
                    //Reg like company
                    message.MessageProcessing = "REGC";
                    message.MessageText = InputCompanyName;
                }
                else
                {
                    //Reg like employee
                    message.MessageProcessing = "REGE";
                }
                MessagesAsistant.SendMessage(client, message);
            }
            else
            {
                _MainCodeBehind.ShowErrorWindow("Пароли не совпадают.");
            }
        }
        //Метод ожидание ответа сервера для потока
        private void CheckForLogin(object obj)
        {
            TcpClient client = obj as TcpClient;
            Message answer = MessagesAsistant.ReadMessage(client);
            switch (answer.MessageProcessing)
            {
                case "ALOK": //Всё правильно
                    {
                        if (_IsLikeCompanyCheck == true)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => _MainCodeBehind.LoadView(ViewType.CompanyWindow)));
                        }
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => _MainCodeBehind.LoadView(ViewType.CandidateWindow)));
                        }
                        CurrentUser.CurrentUserLogin = InputLogin;
                        CurrentUser.IsCurrentUserCompany = IsLikeCompanyCheck;
                        _MainCodeBehind.ShowSuccessWindow("Упешно");
                        break;
                    }
                case "LOGN": //Неверный логин
                    {
                        _MainCodeBehind.ShowErrorWindow("Логин занят");
                        break;
                    }
                default: //Прочая ошибка
                    {
                        _MainCodeBehind.ShowErrorWindow("Что-то пошло не так");
                    }
                    break;
            }
        }
    }
}
