using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server.DbClasses
{
    class Messages
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateId { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public bool ToCompany { get; set; }
    }
}
