using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Client
{
    public class Candidates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string Specialisation { get; set; }
        public string Portfolio { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public object tcpClient { get; set; }
    }
}
