using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeFinder_Client.View
{
    /// <summary>
    /// Interaction logic for CompanyWindow.xaml
    /// </summary>
    public partial class CompanyWindow : UserControl
    {
        public CompanyWindow()
        {
            InitializeComponent();
        }

        private void NumberInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AllEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.CandidatesChosen candidate = AllEmployees.SelectedItem as Model.CandidatesChosen;
            if (candidate != null)
            {
                ENameBox.Text = candidate.Name;
                ESpecBox.Text = candidate.Specialisation;
                EAgeBox.Text = candidate.Age.ToString();
                ECityBox.Text = candidate.City;
                EPortBox.Text = candidate.Portfolio;
            }
        }
    }
}
