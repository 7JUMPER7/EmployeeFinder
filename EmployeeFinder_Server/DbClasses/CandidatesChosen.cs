using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server.DbClasses
{
    public class CandidatesChosen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string Specialisation { get; set; }
        public string Portfolio { get; set; }
        public string Login { get; set; }
    }
}
