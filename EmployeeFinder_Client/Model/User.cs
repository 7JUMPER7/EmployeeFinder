using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Client.Model
{
    public class User
    {
        public string Сontact { get; set; }
        public ObservableCollection<Message> messages { get; set; }

        public User(string _Сontact)
        {
            Сontact = _Сontact;
            messages = new ObservableCollection<Message>();
        }
    }
}
