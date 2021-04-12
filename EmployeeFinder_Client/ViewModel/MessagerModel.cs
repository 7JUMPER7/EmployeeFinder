using System;
using System.ComponentModel;

namespace EmployeeFinder_Client.ViewModel
{
    public class MessagerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IMainWindowsCodeBehind _MainCodeBehind;

        /// <summary>
        //конструктор страницы
        /// </summary>
        public MessagerModel(IMainWindowsCodeBehind codeBehind)
        {
            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
        }
    }
}
