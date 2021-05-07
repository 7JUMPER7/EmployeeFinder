using EmployeeFinder_Client.ViewModel;
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
    public interface IMainWindowsCodeBehind
    {
        void ShowMessage(string message);
    }
    /// <summary>
    /// Interaction logic for Messager.xaml
    /// </summary>
    public partial class Messager : Window, IMainWindowsCodeBehind
    {
        public Messager()
        {
            InitializeComponent();
            buttonMinimized.Click += (s, e) => WindowState = WindowState.Minimized;
            buttonClose.Click += (s, e) => Close();

            this.Loaded += Messager_Loaded;
        }

        private void Messager_Loaded(object sender, RoutedEventArgs e)
        {
            MessagerView messagerView = new MessagerView();
            MessagerModel messagerModel = new MessagerModel();
            messagerView.DataContext = messagerModel;
            this.OutputView.Content = messagerView;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
