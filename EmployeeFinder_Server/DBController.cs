using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeFinder_Server.DbClasses;

namespace EmployeeFinder_Server
{
    internal class DBController
    {
        private DataBaseContext dataBase;

        public DBController()
        {
            dataBase = new DataBaseContext();
            //dataBase.Messages.Add(new Messages()); //УБРАТЬ ПОСЛЕ ПЕРВОГО СОЗДАНИЕ БД
        }

        public void AddMessage(Message message)
        {
            Messages messages = new Messages();
            messages.Message = message.MessageText;
            messages.Time = message.obj.ToString();
            if (ThisIsACandidate(message.FromWhom))
            {
                messages.CandidateId = GetIdCandidate(message.FromWhom);
                messages.CompanyId = GetIdCompanies(message.ToWhom);
                messages.ToCompany = true;
            }
            else
            {
                messages.CandidateId = GetIdCandidate(message.ToWhom);
                messages.CompanyId = GetIdCompanies(message.FromWhom);
                messages.ToCompany = false;
            }
            dataBase.Messages.Add(messages);
            dataBase.SaveChanges();
        }

        public bool ThisIsACandidate(string login)
        {
            List<Candidates> buf = dataBase.Candidates.ToList();
            foreach (var item in buf)
                if (item.Login == login)
                    return true;
            return false;
        }

        public object GetTcpClient(Message message)
        {
            if (ThisIsACandidate(message.FromWhom))
            {
                foreach (var item in dataBase.Candidates.ToList())
                    if (item.Login == message.FromWhom)
                        return item.Client;
            }
            else
            {
                foreach (var item in dataBase.Companies.ToList())
                    if (item.Login == message.FromWhom)
                        return item.Client;
            }
            return null;
        }

        public int GetIdCandidate(string login)
        {
            foreach (var item in dataBase.Candidates)
                if (item.Login == login)
                    return item.Id;

            return -1;
        }

        public int GetIdCompanies(string login)
        {
            foreach (var item in dataBase.Companies)
                if (item.Login == login)
                    return item.Id;

            return -1;
        }

        /// <summary>
        /// Проверка наличия новых сообщений для пользователя
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message IsNewMeesagesAvailable(Message message)
        {
            Message answer = new Message();

            int lastCount = GetMessagesCountByLogin(message.Login) - Int32.Parse(message.MessageText);

            if (lastCount > 0)
                answer = GetAllMessages(message.Login, lastCount);
            else
                answer.MessageProcessing = "ALUP"; //All is updated
            return answer;
        }
        private int GetMessagesCountByLogin(string login)
        {
            int counter = 0;

            int id = GetIdCandidate(login);
            if (id == -1)
            {
                id = GetIdCompanies(login);
            }

            if (id != -1)
            {
                foreach (Messages item in dataBase.Messages.ToList())
                {
                    if (item.CandidateId == id || item.CompanyId == id)
                    {
                        counter++;
                    }
                }
                return counter;
            }
            return -1;
        }

        /// <summary>
        /// Получение всех сообщений для пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="count">Кол-во последних сообщений которые нужно получить</param>
        /// <returns></returns>
        public Message GetAllMessages(string login, int count = -1)
        {
            Message answer = new Message();
            List<Message> messages = new List<Message>();

            int id = GetIdCandidate(login);
            if (id == -1)
            {
                id = GetIdCompanies(login);
            }

            if (id != -1)
            {
                List<Messages> bufMessages = dataBase.Messages.ToList();
                foreach (Messages item in bufMessages)
                {
                    if (item.CandidateId == id || item.CompanyId == id)
                    {
                        if (item.ToCompany)
                        {
                            messages.Add(new Message()
                            {
                                MessageText = item.Message,
                                FromWhom = dataBase.Candidates.Where(c => c.Id == item.CandidateId).FirstOrDefault().Login,
                                ToWhom = dataBase.Companies.Where(c => c.Id == item.CompanyId).FirstOrDefault().Login,
                                obj = item.Time,
                                MessageProcessing = "BUFM"
                            });
                        }
                        else
                        {
                            messages.Add(new Message()
                            {
                                MessageText = item.Message,
                                FromWhom = dataBase.Companies.Where(c => c.Id == item.CompanyId).FirstOrDefault().Login,
                                ToWhom = dataBase.Candidates.Where(c => c.Id == item.CandidateId).FirstOrDefault().Login,
                                obj = item.Time,
                                MessageProcessing = "BUFM"
                            });
                        }
                    }
                }

                if (count != -1)
                {
                    answer.obj = messages.GetRange(messages.Count - count, count);
                }
                else
                {
                    answer.obj = messages;
                }
                answer.MessageProcessing = "ALOK";
            }
            else
            {
                answer.MessageProcessing = "EROR";
            }
            return answer;
        }

        public Message AddOrRemoveCandidateFromWishList(Message message)
        {
            Message answer = new Message();

            int companyId = dataBase.Companies.Where(c => c.Login == message.FromWhom).FirstOrDefault().Id;
            int candidateId = Int32.Parse(message.MessageText);
            if (companyId != 0)
            {
                CompaniesWishLists wishItem = dataBase.CompaniesWishLists.Where(w => w.CompanyId == companyId && w.CandidateId == candidateId).FirstOrDefault();
                if (wishItem != null)
                {
                    dataBase.CompaniesWishLists.Remove(wishItem);
                    answer.MessageProcessing = "REMV";
                }
                else
                {
                    CompaniesWishLists newWish = new CompaniesWishLists() { CompanyId = companyId, CandidateId = Int32.Parse(message.MessageText) };
                    dataBase.CompaniesWishLists.Add(newWish);
                    answer.MessageProcessing = "ADED";
                }
                dataBase.SaveChanges();
            }
            else
            {
                answer.MessageProcessing = "EROR";
            }
            return answer;
        }

        /// <summary>
        /// Проверяет есть ли филиал с такими данными в базе данных
        /// </summary>
        /// <param name="message">Сообщение с данными о филиале</param>
        /// <returns>Сообщение с обработаными данными</returns>
        public Message IsLoginCorrectCompany(Message message, TcpClient client)
        {
            Message answer = new Message();
            foreach (Companies company in dataBase.Companies)
                if (message.Login == company.Login)
                {
                    //Если пароль совпал
                    if (message.Password == company.Password)
                    {
                        answer.MessageProcessing = "ALOK";
                        company.Client = client;
                        dataBase.SaveChanges();
                    }
                    //Если пароль не совпал
                    else
                        answer.MessageProcessing = "PASS";
                    break;
                }
            if (answer.MessageProcessing == null)
                answer.MessageProcessing = "LOGN";
            return answer;
        }

        /// <summary>
        /// Проверяет есть ли роботник с такими данными в базе данных. ALOK - всё ок, PASS - неверный пароль, LOGN - неверный логин.
        /// </summary>
        /// <param name="message">Сообщение с данными о пользователи</param>
        /// <returns>Сообщение с обработаными данными</returns>
        public Message IsLoginCorrectEmployee(Message message, TcpClient client)
        {
            Message answer = new Message();
            foreach (Candidates candidate in dataBase.Candidates)
                if (message.Login == candidate.Login)
                {
                    //Если пароль совпал
                    if (message.Password == candidate.Password)
                    {
                        answer.MessageProcessing = "ALOK";
                        candidate.Client = client;
                        dataBase.SaveChanges();
                    }
                    //Если пароль не совпал
                    else
                        answer.MessageProcessing = "PASS";
                    break;
                }
            if (answer.MessageProcessing == null)
                answer.MessageProcessing = "LOGN";
            return answer;
        }

        /// <summary>
        /// Добавляет новую компанию в БД. Возвращает ALOK если всё хорошо, LOGN - логин занят, EROR - ошибка.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message RegisterCompany(Message message, TcpClient client)
        {
            Message answer = new Message();
            bool LoginIsNotAvailable = false;

            foreach (Companies company in dataBase.Companies)
            {
                if (message.Login == company.Login)
                {
                    LoginIsNotAvailable = true;
                    break;
                }
            }

            if (LoginIsNotAvailable == true)
            {
                answer.MessageProcessing = "LOGN";
            }
            else
            {
                Companies company = new Companies()
                {
                    Client = client,
                    Login = message.Login,
                    Password = message.Password,
                    Name = message.MessageText
                };
                try
                {
                    dataBase.Companies.Add(company);
                    dataBase.SaveChanges();
                    answer.MessageProcessing = "ALOK";
                }
                catch (Exception ex)
                {
                    answer.MessageProcessing = "EROR";
                    answer.MessageText = ex.Message;
                }
            }
            return answer;
        }

        /// <summary>
        /// Добавляет нового работника в БД. Возвращает ALOK если всё хорошо, LOGN - логин занят, EROR - ошибка.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message RegisterEmployee(Message message, TcpClient client)
        {
            Message answer = new Message();
            bool LoginIsNotAvailable = false;

            foreach (Candidates candidate in dataBase.Candidates)
            {
                if (message.Login == candidate.Login)
                {
                    LoginIsNotAvailable = true;
                    break;
                }
            }

            if (LoginIsNotAvailable == true)
            {
                answer.MessageProcessing = "LOGN";
            }
            else
            {
                Candidates candidate = new Candidates()
                {
                    Client = client,
                    Login = message.Login,
                    Password = message.Password
                };
                try
                {
                    dataBase.Candidates.Add(candidate);
                    dataBase.SaveChanges();
                    answer.MessageProcessing = "ALOK";
                }
                catch (Exception ex)
                {
                    answer.MessageProcessing = "EROR";
                    answer.MessageText = ex.Message;
                }
            }
            return answer;
        }

        /// <summary>
        /// Сохраняет анкету работника в базу данных. Возвращает ALOK если всё хорошо, EROR - ошибка.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message SaveEmployeeInfo(Message message)
        {
            Message answer = new Message();
            //Структура строки:
            // 0    1        2        3            4                5
            //Id|||Name|||CityName|||Age|||SpecialisationName|||Portfolio
            string[] EmployeeInfo = message.MessageText.Split(new string[] { "|||" }, StringSplitOptions.None);

            Candidates candidate = dataBase.Candidates.Where(c => c.Login == message.Login).FirstOrDefault();
            candidate.Name = EmployeeInfo[1];
            candidate.Age = Int32.Parse(EmployeeInfo[3]);
            candidate.Portfolio = EmployeeInfo[5];

            try
            {
                //Проверка города
                int CityId = IsCityInTable(EmployeeInfo[2]);
                if (CityId != -1)
                    candidate.CityId = CityId;
                else
                    candidate.CityId = CreateAndAddCity(EmployeeInfo[2]).Id;

                //Проверка специализации
                int SpecId = IsSpecialisationInTable(EmployeeInfo[4]);
                if (SpecId != -1)
                    candidate.SpecialisationId = SpecId;
                else
                    candidate.SpecialisationId = CreateAndAddSpecialisation(EmployeeInfo[4]).Id;
                dataBase.SaveChanges();

                //Формирование ответа
                answer.MessageProcessing = "ALOK";
            }
            catch (Exception ex)
            {
                answer.MessageProcessing = "EROR";
                answer.MessageText = ex.Message;
            }
            return answer;
        }

        private int IsCityInTable(string cityName)
        {
            Cities city = dataBase.Cities.Where(c => c.Name == cityName).FirstOrDefault();
            if (city != null)
                return city.Id;
            return -1;
        }

        private Cities CreateAndAddCity(string cityName)
        {
            Cities city = new Cities() { Name = cityName };
            dataBase.Cities.Add(city);
            dataBase.SaveChanges();
            return city;
        }

        private int IsSpecialisationInTable(string specName)
        {
            Specialisations specialisation = dataBase.Specialisations.Where(s => s.Name == specName).FirstOrDefault();
            if (specialisation != null)
                return specialisation.Id;
            return -1;
        }

        private Specialisations CreateAndAddSpecialisation(string specName)
        {
            Specialisations specialisation = new Specialisations() { Name = specName };
            dataBase.Specialisations.Add(specialisation);
            dataBase.SaveChanges();
            return specialisation;
        }

        /// <summary>
        /// Удаление работника
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message DeleteCandidate(Message message)
        {
            Message answer = new Message();
            Candidates candidate = dataBase.Candidates.Where(c => c.Login == message.Login).FirstOrDefault();
            if (candidate == null)
            {
                answer.MessageProcessing = "EROR";
            }
            else
            {
                try
                {
                    DeleteCityIfCandidateOne(candidate.CityId);
                    DeleteSpecIfCandidateOne(candidate.SpecialisationId);
                    dataBase.Candidates.Remove(candidate);
                    dataBase.SaveChanges();
                    answer.MessageProcessing = "ALOK";
                }
                catch (Exception)
                {
                    answer.MessageProcessing = "EROR";
                }
            }
            return answer;
        }
        private void DeleteCityIfCandidateOne(int cityId)
        {
            DbClasses.Cities city = dataBase.Cities.Where(c => c.Id == cityId).FirstOrDefault();
            int count = dataBase.Candidates.Where(c => c.CityId == city.Id).Count();
            if (count == 1)
            {
                dataBase.Cities.Remove(city);
            }
        }
        private void DeleteSpecIfCandidateOne(int specId)
        {
            DbClasses.Specialisations spec = dataBase.Specialisations.Where(s => s.Id == specId).FirstOrDefault();
            int count = dataBase.Candidates.Where(c => c.SpecialisationId == spec.Id).Count();
            if (count == 1)
            {
                dataBase.Specialisations.Remove(spec);
            }
        }

        /// <summary>
        /// Возвращает список кандидатов
        /// </summary>
        /// <returns>список кандидатов</returns>
        public object GetCandidates()
        {
            //List<Candidates> buf = dataBase.Candidates.ToList();
            //foreach (Candidates item in buf)
            //{
            //    item.Client = null;
            //}
            //return buf;
            return dataBase.Candidates.ToList();
        }

        public object GetCandidatesName()
        {
            return dataBase.Candidates.ToList();
        }

        public object GetCandidateByLogin(string login)
        {
            Candidates buf = dataBase.Candidates.Where(c => c.Login == login).FirstOrDefault();

            Specialisations specialisations = dataBase.Specialisations.Where(s => s.Id == buf.SpecialisationId).FirstOrDefault();
            string specialisation = (specialisations != null) ? specialisations.Name : "";
            Cities cities = dataBase.Cities.Where(c => c.Id == buf.CityId).FirstOrDefault();
            string city = (cities != null) ? cities.Name : "";

            CandidatesChosen toSend = new CandidatesChosen()
            {
                Login = buf.Login,
                Name = buf.Name,
                Specialisation = specialisation,
                Age = buf.Age,
                City = city,
                Portfolio = buf.Portfolio
            };
            return toSend;
        }

        /// <summary>
        /// Возвращает список городов
        /// </summary>
        /// <returns>список городов в виде обьекта</returns>
        public object GetCities()
        {
            return dataBase.Cities.ToList();
        }

        /// <summary>
        /// Возвращает список названий городов
        /// </summary>
        /// <returns>список с названиями городов</returns>
        public object GetCitiesName()
        {
            return dataBase.Cities.Select(c => c.Name).ToList();
        }

        /// <summary>
        /// Возвращает список компаний
        /// </summary>
        /// <returns>список компаний</returns>
        public object GetCompanies()
        {
            return dataBase.Companies.ToList();
        }

        /// <summary>
        /// Возвращает список пожеланий компании
        /// </summary>
        /// <returns>список пожеланий компании</returns>
        public object GetCompaniesWishLists()
        {
            return dataBase.CompaniesWishLists.ToList();
        }

        /// <summary>
        /// Возвращает список специальностей
        /// </summary>
        /// <returns>список специальностей</returns>
        public object GetSpecialisations()
        {
            return dataBase.Specialisations.ToList();
        }

        /// <summary>
        /// Возвращает список названий Специализаций
        /// </summary>
        /// <returns>список с названиями Специализаций</returns>
        public object GetSpecialisationsName()
        {
            return dataBase.Specialisations.Select(s => s.Name).ToList();
        }

        /// <summary>
        /// Возвращает список сообщений
        /// </summary>
        /// <returns>список сообщений</returns>
        public object GetMessages()
        {
            return dataBase.Messages.ToList();
        }
    }
}