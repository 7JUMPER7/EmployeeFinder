using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Client.Model
{
    class DataAccess
    {
        public List<Message> bufusers = new List<Message>()
        {
            new Message() {FromWhom="User1", ToWhom="User2", MessageText="wf"},
            new Message() {FromWhom="User2", ToWhom="User1", MessageText="wg"},
            new Message() {FromWhom="User3", ToWhom="User1", MessageText="123"},
            new Message() {FromWhom="User4", ToWhom="User1", MessageText="456"},
            new Message() {FromWhom="User5", ToWhom="User1", MessageText="asd"},
            new Message() {FromWhom="User1", ToWhom="User5", MessageText="4we56"},
            new Message() {FromWhom="User1", ToWhom="User4", MessageText="asg"},
            new Message() {FromWhom="User4", ToWhom="User1", MessageText="af"},
            new Message() {FromWhom="User2", ToWhom="User1", MessageText="ag"},
            new Message() {FromWhom="User1", ToWhom="User3", MessageText="4asf56"},
            new Message() {FromWhom="User2", ToWhom="User1", MessageText="qwf"},
        };
    }
}
