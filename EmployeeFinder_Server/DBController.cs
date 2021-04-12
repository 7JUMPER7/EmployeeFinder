using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeFinder_Server.DbClasses;

namespace EmployeeFinder_Server
{
    class DBController
    {
        private DataBaseContext dataBase;

        public DBController()
        {
            dataBase = new DataBaseContext();
            //dataBase.Messages.Add(new Messages()); //УБРАТЬ ПОСЛЕ ПЕРВОГО СОЗДАНИЕ БД
        }

        private Message IsLoginCorrect(Message message)
        {
            Message answer = new Message();

            switch (message.MessageProcessing)
            {
                case "LOGC": //Вход как компания
                    {
                        foreach (Companies company in dataBase.Companies)
                        {
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
                        }

                        if (message.MessageProcessing == null)
                        {
                            answer.MessageProcessing = "LOGN";
                        }
                        break;
                    }
                case "LOGE": //Вход как работник
                    {

                        break;
                    }
                default:
                    break;
            }

            return answer;
        }
    }
}
