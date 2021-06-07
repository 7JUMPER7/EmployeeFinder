using EmployeeFinder_Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
        public string login;
        public TcpClient client;

        public Messager(string _login, TcpClient _client)
        {
            InitializeComponent();
            buttonMinimized.Click += (s, e) => WindowState = WindowState.Minimized;
            buttonClose.Click += (s, e) => Close();

            login = _login;
            client = _client;

            this.Loaded += Messager_Loaded;
        }

        private void Messager_Loaded(object sender, RoutedEventArgs e)
        {
            MessagerView messagerView = new MessagerView();
            MessagerModel messagerModel = new MessagerModel(login, client);
            messagerView.DataContext = messagerModel;
            this.OutputView.Content = messagerView;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
