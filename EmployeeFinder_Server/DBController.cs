using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Проверяет есть ли филиал с такими данными в базе данных
        /// </summary>
        /// <param name="message">Сообщение с данными о филиале</param>
        /// <returns>Сообщение с обработаными данными</returns>
        public Message IsLoginCorrectCompany(Message message)
        {
            Message answer = new Message();
            foreach (Companies company in dataBase.Companies)
                if (message.Login == company.Login)
                {
                    //Если пароль совпал
                    if (message.Password == company.Password)
                        answer.MessageProcessing = "ALOK";
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
        public Message IsLoginCorrectEmployee(Message message)
        {
            Message answer = new Message();
            foreach (Candidates candidate in dataBase.Candidates)
                if (message.Login == candidate.Login)
                {
                    //Если пароль совпал
                    if (message.Password == candidate.Password)
                        answer.MessageProcessing = "ALOK";
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
        public Message RegisterCompany(Message message)
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
                    throw;
                }
            }
            return answer;
        }

        /// <summary>
        /// Добавляет нового работника в БД. Возвращает ALOK если всё хорошо, LOGN - логин занят, EROR - ошибка.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message RegisterEmployee(Message message)
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
                    throw;
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
        /// Возвращает список кандидатов
        /// </summary>
        /// <returns>список кандидатов</returns>
        public object GetCandidates()
        {
            return dataBase.Candidates.ToList();
        }
        public object GetCandidatesName()
        {
            return dataBase.Candidates.ToList();
        }
        public object GetCandidateByLogin(string login)
        {
            Candidates buf = dataBase.Candidates.Where(c => c.Login == login).FirstOrDefault();
            CandidatesChosen toSend = new CandidatesChosen()
            {
                Login = buf.Login,
                Name = buf.Name,
                Specialisation = dataBase.Specialisations.Where(s => s.Id == buf.SpecialisationId).FirstOrDefault().Name,
                Age = buf.Age,
                City = dataBase.Cities.Where(c => c.Id == buf.CityId).FirstOrDefault().Name,
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