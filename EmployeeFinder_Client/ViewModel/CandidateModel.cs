using System;
using EmployeeFinder_Client.View;
using System.ComponentModel;
using System.Net.Sockets;
using EmployeeFinder_Client.Model;
using System.Threading;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace EmployeeFinder_Client.ViewModel
{
    public class CandidateModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IMainWindowsCodeBehind _MainCodeBehind;
        private TcpClient client;

        /// <summary>
        /// конструктор страницы
        /// </summary>
        public CandidateModel(IMainWindowsCodeBehind codeBehind, TcpClient client)
        {
            NewMessage = true;
            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
            this.client = client;

            MessagesAsistant.SendMessage(this.client, new Message() { MessageProcessing = "RCBL", Login = CurrentUser.CurrentUserLogin });
            Message message = MessagesAsistant.ReadMessage(this.client);
            if (message.MessageProcessing == "RCBL")
            {
                CandidatesChosen candidate = (message.obj as JObject).ToObject<CandidatesChosen>();
                InputName = candidate.Name;
                InputSpecialisation = candidate.Specialisation;
                InputAge = candidate.Age;
                InputCity = candidate.City;
                InputPortfolio = candidate.Portfolio;
            }
        }

        /// <summary>
        /// Индикатор нового сообщения
        /// </summary>
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
            Message message = new Message()
            {
                Login = CurrentUser.CurrentUserLogin,
                MessageProcessing = "PUBL"
            };
            message.MessageText = $"d|||{InputName}|||{InputCity}|||{InputAge}|||{InputSpecialisation}|||{InputPortfolio}";
            Thread thread = new Thread(new ParameterizedThreadStart(PublishResult));
            thread.IsBackground = true;
            thread.Start(client);

            MessagesAsistant.SendMessage(client, message);
        }

        private void PublishResult(object obj)
        {
            TcpClient client = obj as TcpClient;
            Message answer = MessagesAsistant.ReadMessage(client);
            switch (answer.MessageProcessing)
            {
                case "ALOK":
                    _MainCodeBehind.ShowSuccessWindow("Успешно сохранено");
                    break;
                case "EROR":
                    _MainCodeBehind.ShowErrorWindow("Ошибка сохранения");
                    break;
            }
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
            Message message = new Message()
            {
                Login = CurrentUser.CurrentUserLogin,
                MessageProcessing = "DELE"
            };
            Thread thread = new Thread(new ParameterizedThreadStart(DeleteInfo));
            thread.IsBackground = true;
            thread.Start(client);

            MessagesAsistant.SendMessage(client, message);
        }
        private void DeleteInfo(object obj)
        {
            TcpClient client = obj as TcpClient;
            Message answer = MessagesAsistant.ReadMessage(client);
            switch (answer.MessageProcessing)
            {
                case "ALOK":
                    _MainCodeBehind.ShowSuccessWindow("Успешно удалено");
                    Thread.Sleep(700);
                    Action action = () => _MainCodeBehind.CloseWindow();
                    System.Windows.Application.Current.Dispatcher.Invoke(action);
                    break;
                case "EROR":
                    _MainCodeBehind.ShowErrorWindow("Ошибка удаления");
                    break;
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
            messager.Height = 450;
            messager.Width = 600;
            messager.Show();
        }
    }
}
