using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.ServerQueries
{
    [Serializable]
    public class PostRegisterCandidateQuery : PostQuery
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public override string FormQuery()
        {
            return $"POST&&CANDIDATE&&{Login}&&{Password}";
        }
    }
}
