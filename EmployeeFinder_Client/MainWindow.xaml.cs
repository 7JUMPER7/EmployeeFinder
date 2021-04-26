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
using System.Threading;
using System.Windows.Media.Animation;
using System.Net.Sockets;

namespace EmployeeFinder_Client
{
    public interface IMainWindowsCodeBehind
    {
        void ShowMessage(string message);
        void ShowSuccessWindow(string message);
        void ShowErrorWindow(string message);
        void LoadView(ViewType typeView, TcpClient client);
        bool GetIsConnected();
        TcpClient GetClient();
        void CloseWindow();
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
        TcpClient client;
        bool IsConnected = false;

        

        public MainWindow()
        {
            InitializeComponent();


            buttonMinimized.Click += (s, e) => WindowState = WindowState.Minimized;
            buttonClose.Click += (s, e) => CloseWindow();

            this.Loaded += MainWindow_Loaded;
        }
        public void CloseWindow()
        {
            MessagesAsistant.SendMessage(client, new Message() { MessageProcessing = "EXIT" });
            this.Close();
        }


        /// <summary>
        /// Попытка подключиться к серверу
        /// </summary>
        private void TryToConnect()
        {
            while (!IsConnected)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", 1024);
                    IsConnected = client.Connected;
                    ShowSuccessWindow("Подключено к серверу");
                }
                catch (Exception)
                {
                    ShowErrorWindow("Сервер не отвечает");
                    Thread.Sleep(3000);
                }
            }
        }
        public bool GetIsConnected()
        {
            return IsConnected;
        }
        public TcpClient GetClient()
        {
            return client;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadView(ViewType.LogInPage, client);
            new Thread(() => { TryToConnect(); }) { IsBackground = true }.Start();
        }

        public void LoadView(ViewType typeView, TcpClient client)
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
                    RegisterModel modelRegister = new RegisterModel(this, client);
                    viewRegister.DataContext = modelRegister;
                    this.OutputView.Content = viewRegister;
                    break;

                case ViewType.CompanyWindow:
                    CompanyWindow viewCompany = new CompanyWindow();
                    CompanyModel modelCompany = new CompanyModel(this, client);
                    viewCompany.DataContext = modelCompany;
                    this.OutputView.Content = viewCompany;
                    break;

                case ViewType.CandidateWindow:
                    CandidateWindow viewCandidate = new CandidateWindow();
                    CandidateModel modelCandidate = new CandidateModel(this, client);
                    viewCandidate.DataContext = modelCandidate;
                    this.OutputView.Content = viewCandidate;
                    break;
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Отображение окна позитивной информации
        /// </summary>
        /// <param name="message"></param>
        public void ShowSuccessWindow(string message)
        {
            ThreadPool.QueueUserWorkItem(SuccessWindowAnimation, message);
        }
        /// <summary>
        /// Отображение окна негативной информации
        /// </summary>
        /// <param name="message"></param>
        public void ShowErrorWindow(string message)
        {
            ThreadPool.QueueUserWorkItem(ErrorWindowAnimation, message);
        }
        private void SuccessWindowAnimation(object obj)
        {
            string message = obj.ToString();
            Action action = () => InfoWindowBorder.Background = new SolidColorBrush(Color.FromRgb(169, 251, 215));
            this.Dispatcher.Invoke(action);
            action = () => InfoWindowText.Foreground = new SolidColorBrush(Color.FromRgb(42, 145, 106));
            this.Dispatcher.Invoke(action);
            action = () => InfoWindowBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(42, 145, 106));
            this.Dispatcher.Invoke(action);

            action = () => InfoWindowText.Content = message;
            this.Dispatcher.Invoke(action);
            action = () => InfoWindow.BeginAnimation(MarginProperty, InAnimation());
            this.Dispatcher.Invoke(action);
            Thread.Sleep(1500);
            action = () => InfoWindow.BeginAnimation(MarginProperty, OutAnimation());
            this.Dispatcher.Invoke(action);
        }
        private void ErrorWindowAnimation(object obj)
        {
            string message = obj.ToString();
            Action action = () => InfoWindowBorder.Background = new SolidColorBrush(Color.FromRgb(236, 131, 133));
            this.Dispatcher.Invoke(action);
            action = () => InfoWindowText.Foreground = new SolidColorBrush(Color.FromRgb(156, 25, 27));
            this.Dispatcher.Invoke(action);
            action = () => InfoWindowBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(156, 25, 27));
            this.Dispatcher.Invoke(action);

            action = () => InfoWindowText.Content = message;
            this.Dispatcher.Invoke(action);
            action = () => InfoWindow.BeginAnimation(MarginProperty, InAnimation());
            this.Dispatcher.Invoke(action);
            Thread.Sleep(1500);
            action = () => InfoWindow.BeginAnimation(MarginProperty, OutAnimation());
            this.Dispatcher.Invoke(action);
        }
        private ThicknessAnimation InAnimation()
        {
            ThicknessAnimation Animanion = new ThicknessAnimation();
            Animanion.From = new Thickness(0, 5, -270, 10);
            Animanion.To = new Thickness(0, 5, 10, 10);
            Animanion.AccelerationRatio = 0.1;
            Animanion.DecelerationRatio = 0.9;
            Animanion.SpeedRatio = 1.3;
            return Animanion;
        }
        private ThicknessAnimation OutAnimation()
        {
            ThicknessAnimation Animanion = new ThicknessAnimation();
            Animanion.From = new Thickness(0, 5, 10, 10);
            Animanion.To = new Thickness(0, 5, -270, 10);
            Animanion.AccelerationRatio = 0.9;
            Animanion.DecelerationRatio = 0.1;
            Animanion.SpeedRatio = 1.3;
            return Animanion;
        }
    }
}
