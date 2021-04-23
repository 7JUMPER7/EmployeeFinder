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

            List<Cities> cities = ReceiveCities(client);
            CityFilter = cities.Select(c => c.Name).ToList();
            List<Specialisations> specialisations = ReceiveSpecs(client);
            SpecFilter = specialisations.Select(s => s.Name).ToList();
            var candidates = ReceiveCandidates(client);

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
        public ObservableCollection<Candidates> SelectedEmployee;

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
            string info = "ФИО: " + SelectedEmployee[0].Name + '\n';
            info += "Специализация: " + SelectedEmployee[0].Age + '\n';
            info += "Возраст: " + _InputAge + '\n';
            info += "Город: " + _InputCity + '\n';
            info += "Портфолио: " + _InputPortfolio + '\n';
            Clipboard.SetText(info);
            _MainCodeBehind.ShowSuccessWindow("Скопировано");
        }


        /// <summary>
        /// Обновление таблицы с работниками
        /// </summary>
        private void UpdateTable()
        {
            var candidates = AllCandidates;
            if (SelectedCity != null)
            {
                candidates = candidates.Where(c => c.City == SelectedCity).ToList();
            }
            if (SelectedSpec != null)
            {
                candidates = candidates.Where(c => c.Specialisation == SelectedSpec).ToList();
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
        private List<Cities> ReceiveCities(TcpClient client)
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
        private List<Specialisations> ReceiveSpecs(TcpClient client)
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
        private List<Candidates> ReceiveCandidates(TcpClient client)
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
    }
}
