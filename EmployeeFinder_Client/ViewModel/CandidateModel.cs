using System;
using EmployeeFinder_Client.View;
using System.ComponentModel;

namespace EmployeeFinder_Client.ViewModel
{
    public class CandidateModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IMainWindowsCodeBehind _MainCodeBehind;

        /// <summary>
        //конструктор страницы
        /// </summary>
        public CandidateModel(IMainWindowsCodeBehind codeBehind)
        {
            NewMessage = true;
            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
        }

        //Индикатор нового сообщения
        public bool NewMessage { get; set; }

        /// <summary>
        /// Ввод имени
        /// </summary>
        private string _InputName;
        public string InputName
        {
            get { return _InputName; }
            set
            {
                _InputName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputName)));
            }
        }

        /// <summary>
        /// Ввод специализации
        /// </summary>
        private string _InputSpecialisation;
        public string InputSpecialisation
        {
            get { return _InputSpecialisation; }
            set
            {
                _InputSpecialisation = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputSpecialisation)));
            }
        }

        /// <summary>
        /// Ввод возраста
        /// </summary>
        private int _InputAge;
        public int InputAge
        {
            get { return _InputAge; }
            set
            {
                _InputAge = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputAge)));
            }
        }

        /// <summary>
        /// Ввод города
        /// </summary>
        private string _InputCity;
        public string InputCity
        {
            get { return _InputCity; }
            set
            {
                _InputCity = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputCity)));
            }
        }

        /// <summary>
        /// Ввод портфолио
        /// </summary>
        private string _InputPortfolio;
        public string InputPortfolio
        {
            get { return _InputPortfolio; }
            set
            {
                _InputPortfolio = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputPortfolio)));
            }
        }

        /// <summary>
        /// Публикация резюме
        /// </summary>
        private RelayCommand _PublicationResumeCommand;
        public RelayCommand PublicationResumeCommand
        {
            get
            {
                return _PublicationResumeCommand = _PublicationResumeCommand ??
                  new RelayCommand(OnPublUC, CanPublUC);
            }
        }
        private bool CanPublUC()
        {
            return true;
        }
        private void OnPublUC()
        {
            //логика
        }

        /// <summary>
        /// Удаление резюме
        /// </summary>
        private RelayCommand _DeleteResumeCommand;
        public RelayCommand DeleteResumeCommand
        {
            get
            {
                return _DeleteResumeCommand = _DeleteResumeCommand ??
                  new RelayCommand(OnDelUC, CanDelUC);
            }
        }
        private bool CanDelUC()
        {
            return true;
        }
        private void OnDelUC()
        {
            //логика
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
    }
}
