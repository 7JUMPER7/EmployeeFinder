using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.ServerQueries
{
    public class GetLoginQuery : GetQuery
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsCompany { get; set; }

        public override string FormQuery()
        {
            return $"GET&&LOGIN&&{Login}&&{Password}&&{IsCompany}";
        }
    }
}
