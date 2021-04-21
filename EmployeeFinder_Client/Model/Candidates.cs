using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Client.Model
{
    public class Candidates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public int SpecialisationId { get; set; }
        public string Specialisation { get; set; }
        public string Portfolio { get; set; }
        public string Login { get; set; }
    }
}
