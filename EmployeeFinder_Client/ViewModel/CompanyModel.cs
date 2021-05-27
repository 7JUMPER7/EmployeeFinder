using EmployeeFinder_Client.Model;
using EmployeeFinder_Client.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Linq;
using System.Threading;

namespace EmployeeFinder_Client.ViewModel
{
    public class CompanyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private IMainWindowsCodeBehind _MainCodeBehind;
        private TcpClient client;

        public List<string> CityFilter { get; set; }
        public List<string> SpecFilter { get; set; }
        public ObservableCollection<CandidatesChosen> Candidates { get; set; }
        public List<CandidatesChosen> AllCandidates { get; set; }

        public bool NewMessage { get; set; }

        /// <summary>
        /// конструктор страницы
        /// </summary>
        public CompanyModel(IMainWindowsCodeBehind codeBehind, TcpClient _client)
        {
            ObservableCollection<Candidates> SelectedEmployee = new ObservableCollection<Candidates>();
            client = _client;

            List<Cities> cities = ReceiveCities();
            CityFilter = cities.Select(c => c.Name).ToList();
            CityFilter.Add("Все");
            List<Specialisations> specialisations = ReceiveSpecs();
            SpecFilter = specialisations.Select(s => s.Name).ToList();
            SpecFilter.Add("Все");
            var candidates = ReceiveCandidates();

            AllCandidates = candidates
                .Join(cities,
                cand => cand.CityId,
                city => city.Id,
                (cand, city) => new Candidates
                {
                    Id = cand.Id,
                    Name = cand.Name,
                    City = city.Name,
                    Age = cand.Age,
                    SpecialisationId = cand.SpecialisationId,
                    Portfolio = cand.Portfolio,
                    Login = cand.Login
                })
                .Join(specialisations,
                cand => cand.SpecialisationId,
                spec => spec.Id,
                (cand, spec) => new CandidatesChosen
                {
                    Id = cand.Id,
                    Name = cand.Name,
                    City = cand.City,
                    Age = cand.Age,
                    Specialisation = spec.Name,
                    Portfolio = cand.Portfolio,
                    Login = cand.Login
                }).ToList();
            Candidates = new ObservableCollection<CandidatesChosen>(AllCandidates);
            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
        }


        /// <summary>
        /// Сортировка по городам
        /// </summary>
        private string _SelectedCity;
        public string SelectedCity
        {
            get
            {
                return _SelectedCity;
            }
            set
            {
                _SelectedCity = value;
                UpdateTable();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedCity)));
            }
        }
        /// <summary>
        /// Сортировка по специализации
        /// </summary>
        private string _SelectedSpec;
        public string SelectedSpec
        {
            get
            {
                return _SelectedSpec;
            }
            set
            {
                _SelectedSpec = value;
                UpdateTable();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedSpec)));
            }
        }


        /// <summary>
        /// Выбранный работник
        /// </summary>
        private CandidatesChosen _SelectedEmployee;
        public CandidatesChosen SelectedEmployee
        {
            get { return _SelectedEmployee; }
            set
            {
                _SelectedEmployee = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedEmployee)));
            }
        }


        /// <summary>
        /// Значения фильтра возраста
        /// </summary>
        private int _FromAgeFilter;
        public int FromAgeFilter
        {
            get { return _FromAgeFilter; }
            set
            {
                _FromAgeFilter = value;
                UpdateTable();
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
                UpdateTable();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ToAgeFilter)));
            }
        }

        /// <summary>
        /// Избранные кандидаты
        /// </summary>
        private bool _FavoriteСandidates;
        public bool FavoriteСandidates
        {
            get { return _FavoriteСandidates; }
            set
            {
                if (_FavoriteСandidates == value) return;
                _FavoriteСandidates = value;
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
            Messager messager = new Messager(CurrentUser.CurrentUserLogin, client);
            messager.Height = 450;
            messager.Width = 600;
            messager.Show();
        }

        /// <summary>
        /// Новое сообщение выбранному кандидату
        /// </summary>
        private RelayCommand _NewMessageToCandidate;
        public RelayCommand NewMessageToCandidate
        {
            get
            {
                return _NewMessageToCandidate = _NewMessageToCandidate ??
                  new RelayCommand(OnMessagerr2UC, CanMessager2UC);
            }
        }
        private bool CanMessager2UC()
        {
            return true;
        }
        private void OnMessagerr2UC()
        {
            try
            {
                Message newMessage = new Message()
                {
                    FromWhom = CurrentUser.CurrentUserLogin,
                    ToWhom = SelectedEmployee.Login,
                    MessageText = "Здравствуйте!",
                    obj = DateTime.Now.ToShortTimeString(),
                    MessageProcessing = "RECM"
                };
                MessagesAsistant.SendMessage(client, newMessage);

                Message answer = MessagesAsistant.ReadMessage(client);
                if (answer.MessageProcessing == "SAVM")
                {
                    Messager messager = new Messager(CurrentUser.CurrentUserLogin, client);
                    messager.Height = 450;
                    messager.Width = 600;
                    messager.Show();
                }
                else
                {
                    _MainCodeBehind.ShowErrorWindow("Не удалось отправить");
                }
            }
            catch (Exception)
            {
                _MainCodeBehind.ShowErrorWindow("Ошибка");
            }

        }

        /// <summary>
        /// Копирование информации о работнике в буфер обмена
        /// </summary>
        private RelayCommand _CopyEmployeeInfoCommand;
        public RelayCommand CopyEmployeeInfoCommand
        {
            get
            {
                return _CopyEmployeeInfoCommand = _CopyEmployeeInfoCommand ??
                  new RelayCommand(OnCopyUC, CanCopyUC);
            }
        }
        private bool CanCopyUC()
        {
            return true;
        }
        private void OnCopyUC()
        {
            string info = "ФИО: " + SelectedEmployee.Name + '\n';
            info += "Специализация: " + SelectedEmployee.Specialisation + '\n';
            info += "Возраст: " + SelectedEmployee.Age + '\n';
            info += "Город: " + SelectedEmployee.City + '\n';
            info += "Портфолио: " + SelectedEmployee.Portfolio + '\n';
            Clipboard.SetText(info);
            _MainCodeBehind.ShowSuccessWindow("Скопировано");
        }

        /// <summary>
        /// Добавления кандидата в избранное
        /// </summary>
        private RelayCommand _AddToWishList;
        public RelayCommand AddToWishList
        {
            get
            {
                return _AddToWishList = _AddToWishList ??
                  new RelayCommand(OnAddToWishUC, CanAddToWishUC);
            }
        }
        private bool CanAddToWishUC()
        {
            return true;
        }
        private void OnAddToWishUC()
        {
            Message newMessage = new Message()
            {
                MessageProcessing = "ADWL",
                MessageText = SelectedEmployee.Id.ToString(),
                FromWhom = CurrentUser.CurrentUserLogin
            };
            MessagesAsistant.SendMessage(client, newMessage);

            Thread thread = new Thread(WishListListener);
            thread.IsBackground = true;
            thread.Start();
        }
        private void WishListListener()
        {
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "ADED")
            {
                Action action = () => _MainCodeBehind.ShowSuccessWindow("Добавлено в избранное");
                System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
            else if (answer.MessageProcessing == "REMV")
            {
                Action action = () => _MainCodeBehind.ShowSuccessWindow("Удалено из избранного");
                System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
            else
            {
                Action action = () => _MainCodeBehind.ShowErrorWindow("Ошибка");
                System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
        }


        /// <summary>
        /// Обновление таблицы с работниками
        /// </summary>
        private void UpdateTable()
        {
            var candidates = AllCandidates;
            if (SelectedCity != null)
            {
                candidates = candidates.Where(c => c.City == SelectedCity || SelectedCity=="Все").ToList();
            }
            if (SelectedSpec != null)
            {
                candidates = candidates.Where(c => c.Specialisation == SelectedSpec || SelectedSpec == "Все").ToList();
            }
            if (FromAgeFilter != 0)
            {
                candidates = candidates.Where(c => c.Age >= FromAgeFilter).ToList();
            }
            if (ToAgeFilter != 0)
            {
                candidates = candidates.Where(c => c.Age <= ToAgeFilter).ToList();
            }
            Candidates = new ObservableCollection<CandidatesChosen>(candidates); 
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Candidates)));

        }

        /// <summary>
        /// Получает массив доступных названий городов от сервера
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<Cities> ReceiveCities()
        {
            MessagesAsistant.SendMessage(client, new Message() { Login = CurrentUser.CurrentUserLogin, MessageProcessing = "RECC" });
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "RECC")
            {
                if (answer.obj is JArray && answer.obj != null)
                {
                    return (answer.obj as JArray).ToObject<List<Cities>>();
                }
            }
            return null;
        }
        /// <summary>
        /// Получает массив доступных названий специализаций от сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<Specialisations> ReceiveSpecs()
        {
            MessagesAsistant.SendMessage(client, new Message() { Login = CurrentUser.CurrentUserLogin, MessageProcessing = "RECS" });
            Message answer = MessagesAsistant.ReadMessage(client);
            if (answer.MessageProcessing == "RECS")
            {
                if (answer.obj is JArray && answer.obj != null)
                {
                    return (answer.obj as JArray).ToObject<List<Specialisations>>();
                }
            }
            return null;
        }
        /// <summary>
        /// Получает массив всех работников от сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<Candidates> ReceiveCandidates()
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

        private RelayCommand _RefreshCandidate;
        public RelayCommand RefreshCandidate
        {
            get
            {
                return _RefreshCandidate = _RefreshCandidate ??
                  new RelayCommand(OnRefreshCandidate, CanRefreshCandidate);
            }
        }
        private bool CanRefreshCandidate()
        {
            return true;
        }
        private void OnRefreshCandidate()
        {
            //var candidates = new List<Candidates>();
            //Action action = () => candidates = ReceiveCandidates();
            //System.Windows.Application.Current.Dispatcher.Invoke(action);
        }
    }
}
