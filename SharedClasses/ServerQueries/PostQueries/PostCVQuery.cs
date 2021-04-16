using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.ServerQueries
{
    public class PostCVQuery : PostQuery
    {
        private string firstName;
        private string lastName;
        private string middleName;
        public string FullName
        {
            get
            {
                return $"{firstName} {lastName} {middleName}";
            }
            set
            {
                string[] words = value.Split(' ');
                switch(words.Length)
                {
                    case 1:
                        firstName = words[0];
                        break;
                    case 2:
                        firstName = words[0];
                        lastName = words[1];
                        break;
                    case 3:
                        firstName = words[0];
                        lastName = words[1];
                        middleName = words[2];
                        break;
                    default:
                        throw new ArgumentException("string is empty or contains too many words");
                }
            }
        }
        public string Specialization { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string Portfolio { get; set; }

        public override string FormQuery()
        {
            return $"POST&&CV&&{FullName}&&{Specialization}&&{City}&&{Age}&&{Portfolio}";
        }
    }
}
