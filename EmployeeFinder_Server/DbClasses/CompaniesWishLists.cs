using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server.DbClasses
{
    class CompaniesWishLists
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateId { get; set; }
    }
}
