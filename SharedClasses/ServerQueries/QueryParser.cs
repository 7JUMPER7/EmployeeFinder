using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.ServerQueries
{
    static class QueryParser
    {
        public static Query Parse(string queryString)
        {
            string[] tokens = queryString.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens[0] == "GET")
            {
                switch (tokens[1])
                {
                    case "CV":
                        string city = tokens[2] == " " ? null : tokens[2];
                        string spec = tokens[3] == " " ? null : tokens[3];
                        int min = Convert.ToInt32(tokens[4]);
                        int max = Convert.ToInt32(tokens[5]);
                        return new GetCVQuery()
                        {
                            City = city,
                            Specialization = spec,
                            minAge = min,
                            maxAge = max
                        };
                    case "LOGIN":
                        return new GetLoginQuery()
                        {
                            Login = tokens[2],
                            Password = tokens[3],
                            IsCompany = Convert.ToBoolean(tokens[4])
                        };
                    default:
                        throw new ArgumentException("wrong query word");
                }
            }
            else if (tokens[0] == "POST") //uncomment when add DELETE word
            {
                switch(tokens[1])
                {
                    case "CV":
                        string fullName = tokens[2];
                        string spec = tokens[3];
                        string city = tokens[4];
                        int age = Convert.ToInt32(tokens[5]);
                        string portf = tokens[6];
                        return new PostCVQuery
                        {
                            FullName = fullName,
                            Specialization = spec,
                            City = city,
                            Age = age,
                            Portfolio = portf
                        };

                    case "CANDIDATE":
                        return new PostRegisterCandidateQuery()
                        {
                            Login = tokens[2],
                            Password = tokens[3]
                        };
                    case "COMPANY":
                        return new PostRegisterCompanyQuery()
                        {
                            Login = tokens[2],
                            Password = tokens[3],
                            CompanyName = tokens[4]
                        };
                    default:
                        throw new ArgumentException("wrong query word");
                }
            }
            else
            {
                throw new NotImplementedException("delete not implemented");
            }
        }
    }
}
