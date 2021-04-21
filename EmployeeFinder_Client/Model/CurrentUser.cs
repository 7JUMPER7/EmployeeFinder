using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Client.Model
{
    static class CurrentUser
    {
        public static string CurrentUserLogin { get; set; }
        public static bool IsCurrentUserCompany { get; set; }
    }
}
