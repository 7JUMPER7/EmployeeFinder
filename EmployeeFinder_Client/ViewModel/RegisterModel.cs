using System;
using System.ComponentModel;

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
            //Логика
        }
    }
}
