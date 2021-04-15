using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Input;
using EmployeeFinder_Client.View;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Threading;
using EmployeeFinder_Client.Model;
using System.Resources;

namespace EmployeeFinder_Client.ViewModel
{
    public class LogInModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IMainWindowsCodeBehind _MainCodeBehind;

        /// <summary>
        //конструктор страницы
        /// </summary>
        public LogInModel(IMainWindowsCodeBehind codeBehind)
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
        /// Переход к отображению CompanyWindow или CandidateWindow
        /// </summary>
        private RelayCommand _LoginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return _LoginCommand = _LoginCommand ??
                  new RelayCommand(OnLoadUC, CanLoadUC);
            }
        }

        private bool CanLoadUC()
        {
            return true;
        }

        private void OnLoadUC()
        {
            //
            //логика проверки логина и пароля
            //

            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
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
                //LogIn like company
                message.MessageProcessing = "LOGC";
            }
            else
            {
                //LogIn like employee
                message.MessageProcessing = "LOGE";
            }
            MessagesAsistent.SendMessage(client, message);
        }

        //Метод ожидание ответа сервера для потока
        private void CheckForLogin(object obj)
        {
            TcpClient client = obj as TcpClient;
            Message answer = MessagesAsistent.ReadMessage(client);
            switch (answer.MessageProcessing)
            {
                case "ALOK": //Всё правильно
                    {
                        if (_IsLikeCompanyCheck == true)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => _MainCodeBehind.LoadView(ViewType.CompanyWindow)));
                            DataAccess.Login = InputLogin;
                            DataAccess.Password = InputPassword;
                        }    
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => _MainCodeBehind.LoadView(ViewType.CandidateWindow)));
                            DataAccess.Login = InputLogin;
                            DataAccess.Password = InputPassword;
                        }
                        _MainCodeBehind.ShowSuccessWindow("Упешно");
                        break;
                    }
                case "LOGN": //Неверный логин
                    {
                        _MainCodeBehind.ShowErrorWindow("Неверный логин");
                        break;
                    }
                case "PASS": //Неверный пароль
                    {
                        _MainCodeBehind.ShowErrorWindow("Неверный пароль");
                        break;
                    }
                default: //Прочая ошибка

                    break;
            }
        }

        /// <summary>
        /// Переход к отображению RegisterPage
        /// </summary>
        private RelayCommand _RegisterPageCommand;
        public RelayCommand RegisterPageCommand

        {
            get
            {
                return _RegisterPageCommand = _RegisterPageCommand ??
                  new RelayCommand(OnRegisterUC, CanRegisterUC);
            }
        }

        private bool CanRegisterUC()
        {
            return true;
        }

        private void OnRegisterUC()
        {
            _MainCodeBehind.LoadView(ViewType.RegisterPage);
        }
    }
}