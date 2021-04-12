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
using EmployeeFinder_Client.ViewModel;
using EmployeeFinder_Client.View;


namespace EmployeeFinder_Client
{
    public interface IMainWindowsCodeBehind
    {
        void ShowMessage(string message);
        void LoadView(ViewType typeView);
    }

    public enum ViewType
    {
        LogInPage,
        RegisterPage,
        CompanyWindow,
        CandidateWindow,
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowsCodeBehind
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadView(ViewType.LogInPage);
        }

        public void LoadView(ViewType typeView)
        {
            switch (typeView)
            {
                case ViewType.LogInPage:
                    LogInPage viewLogin = new LogInPage();
                    LogInModel modelLogin = new LogInModel(this);
                    viewLogin.DataContext = modelLogin;
                    this.OutputView.Content = viewLogin;
                    break;

                case ViewType.RegisterPage:
                    RegisterPage viewRegister = new RegisterPage();
                    RegisterModel modelRegister = new RegisterModel(this);
                    viewRegister.DataContext = modelRegister;
                    this.OutputView.Content = viewRegister;
                    break;

                case ViewType.CompanyWindow:
                    CompanyWindow viewCompany = new CompanyWindow();
                    CompanyModel modelCompany = new CompanyModel(this);
                    viewCompany.DataContext = modelCompany;
                    this.OutputView.Content = viewCompany;
                    break;

                case ViewType.CandidateWindow:
                    CandidateWindow viewCandidate = new CandidateWindow();
                    CandidateModel modelCandidate = new CandidateModel(this);
                    viewCandidate.DataContext = modelCandidate;
                    this.OutputView.Content = viewCandidate;
                    break;

            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
