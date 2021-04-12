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
            if (message.MessageProcessing == null)
                answer.MessageProcessing = "LOGN";
            return answer;
        }

        /// <summary>
        /// Проверяет есть ли роботник с такими данными в базе данных
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
            if (message.MessageProcessing == null)
                answer.MessageProcessing = "LOGN";
            return answer;
        }

        /// <summary>
        /// Возвращает список кандидатов
        /// </summary>
        /// <returns>список кандидатов</returns>
        public object GetCandidates()
        {
            return dataBase.Candidates.ToList();
        }

        /// <summary>
        /// Возвращает список городов
        /// </summary>
        /// <returns>список городов</returns>
        public object GetCities()
        {
            return dataBase.Cities.ToList();
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
        /// Возвращает список сообщений
        /// </summary>
        /// <returns>список сообщений</returns>
        public object GetMessages()
        {
            return dataBase.Messages.ToList();
        }
    }
}