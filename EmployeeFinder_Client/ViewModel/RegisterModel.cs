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
        /// Ввод пароля
        /// </summary>
        private string _InputPassword2;
        public string InputPassword2
        {
            get { return _InputPassword2; }
            set
            {
                _InputPassword2 = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputPassword2)));
            }
        }

        /// <summary>
        /// Переход к отображению RegisterPage
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
