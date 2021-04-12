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
    }
}