using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.ServerQueries
{
    public class GetCVQuery : GetQuery
    {
        public string City { get; set; } = null;
        public string Specialization { get; set; } = null;
        public int minAge { get; set; }
        public int maxAge { get; set; }

        public override string FormQuery()
        {
            return $"GET&&CV&&{City ?? " "}&&{Specialization ?? " "}&&{minAge}&&{maxAge}";
        }

    }
}
