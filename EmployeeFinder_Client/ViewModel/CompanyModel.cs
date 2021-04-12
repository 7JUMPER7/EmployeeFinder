using EmployeeFinder_Client.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EmployeeFinder_Client.ViewModel
{
    public class CompanyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private IMainWindowsCodeBehind _MainCodeBehind;

        public string[] CityFilter { get; set; }
        public string[] SpecFilter { get; set; }

        /// <summary>
        //конструктор страницы
        /// </summary>
        public CompanyModel(IMainWindowsCodeBehind codeBehind)
        {
            DataAccess data = new DataAccess();
            CityFilter = data.CityFilter;
            SpecFilter = data.SpecFilter;

            if (codeBehind == null) throw new ArgumentNullException(nameof(codeBehind));
            _MainCodeBehind = codeBehind;
        }


        /// <summary>
        //Значения фильтра
        /// </summary>
        private int _FromAgeFilter;
        public int FromAgeFilter
        {
            get { return _FromAgeFilter; }
            set
            {
                _FromAgeFilter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FromAgeFilter)));
            }
        }

        private int _ToAgeFilter;
        public int ToAgeFilter
        {
            get { return _ToAgeFilter; }
            set
            {
                _ToAgeFilter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FromAgeFilter)));
            }
        }


    }
}
