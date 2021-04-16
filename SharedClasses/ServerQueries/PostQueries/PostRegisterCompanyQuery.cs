using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.ServerQueries
{
    public class PostRegisterCompanyQuery : PostQuery
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public override string FormQuery()
        {
            return $"POST&&COMPANY&&{Login}&&{Password}&&{CompanyName}";
        }
    }
}
