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

        public List<string> CityFilter { get; set; }
        public List<string> SpecFilter { get; set; }
        public ObservableCollection<CandidatesChosen> Candidates { get; set; }
        public List<CandidatesChosen> AllCandidates { get; set; }

        public bool NewMessage { get; set; }

        /// <summary>
        //конструктор страницы
        /// </summary>
        public CompanyModel(IMainWindowsCodeBehind codeBehind)
        {
            //DataAccess data = new DataAccess();
            //CityFilter = data.CityFilter;
            //SpecFilter = data.SpecFilter;

            //Тестовые клиенты
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
            List<Cities> cities = ReceiveCities(client);
            CityFilter = cities.Select(c => c.Name).ToList();
            client.Close();

            client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
            List<Specialisations> specialisations = ReceiveSpecs(client);
            SpecFilter = specialisations.Select(s => s.Name).ToList();
            client.Close();

            client = new TcpClient();
            client.Connect("127.0.0.1", 1024);
            var candidates = ReceiveCandidates(client);
            client.Close();
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
        //Значения фильтра возраста
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
