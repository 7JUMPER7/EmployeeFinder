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

            if (_IsLikeCompanyCheck == true)
            {
                _MainCodeBehind.LoadView(ViewType.CompanyWindow);
            }
            else
            {
                _MainCodeBehind.LoadView(ViewType.CandidateWindow);
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
