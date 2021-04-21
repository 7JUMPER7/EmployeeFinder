using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace EmployeeFinder_Client
{
    //public class InfoWindow
    //{
    //    //Окна уведомлений
    //    private static void ShowSuccessWindow(string message, Grid InfoWindowGrid)
    //    {
    //        ThreadPool.QueueUserWorkItem(SuccessWindowAnimation, message);
    //    }
    //    private static void ShowErrorWindow(string message)
    //    {
    //        ThreadPool.QueueUserWorkItem(ErrorWindowAnimation, message);
    //    }
    //    private static void SuccessWindowAnimation(object obj)
    //    {
    //        string message = obj.ToString();
    //        Action action = () => InfoWindowBorder.Background = new SolidColorBrush(Color.FromRgb(169, 251, 215));
    //        this.Dispatcher.Invoke(action);
    //        action = () => InfoWindowText.Foreground = new SolidColorBrush(Color.FromRgb(42, 145, 106));
    //        this.Dispatcher.Invoke(action);
    //        action = () => InfoWindowBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(42, 145, 106));
    //        this.Dispatcher.Invoke(action);

    //        action = () => InfoWindowText.Content = message;
    //        this.Dispatcher.Invoke(action);
    //        action = () => InfoWindow.BeginAnimation(MarginProperty, InAnimation());
    //        this.Dispatcher.Invoke(action);
    //        Thread.Sleep(1500);
    //        action = () => InfoWindow.BeginAnimation(MarginProperty, OutAnimation());
    //        this.Dispatcher.Invoke(action);
    //    }
    //    private static void ErrorWindowAnimation(object obj)
    //    {
    //        string message = obj.ToString();
    //        Action action = () => InfoWindowBorder.Background = new SolidColorBrush(Color.FromRgb(236, 131, 133));
    //        this.Dispatcher.Invoke(action);
    //        action = () => InfoWindowText.Foreground = new SolidColorBrush(Color.FromRgb(156, 25, 27));
    //        this.Dispatcher.Invoke(action);
    //        action = () => InfoWindowBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(156, 25, 27));
    //        this.Dispatcher.Invoke(action);

    //        action = () => InfoWindowText.Content = message;
    //        this.Dispatcher.Invoke(action);
    //        action = () => InfoWindow.BeginAnimation(MarginProperty, InAnimation());
    //        this.Dispatcher.Invoke(action);
    //        Thread.Sleep(1500);
    //        action = () => InfoWindow.BeginAnimation(MarginProperty, OutAnimation());
    //        this.Dispatcher.Invoke(action);
    //    }
    //    private ThicknessAnimation InAnimation()
    //    {
    //        ThicknessAnimation Animanion = new ThicknessAnimation();
    //        Animanion.From = new Thickness(0, 5, -270, 10);
    //        Animanion.To = new Thickness(0, 5, 10, 10);
    //        Animanion.AccelerationRatio = 0.1;
    //        Animanion.DecelerationRatio = 0.9;
    //        Animanion.SpeedRatio = 1.3;
    //        return Animanion;
    //    }
    //    private ThicknessAnimation OutAnimation()
    //    {
    //        ThicknessAnimation Animanion = new ThicknessAnimation();
    //        Animanion.From = new Thickness(0, 5, 10, 10);
    //        Animanion.To = new Thickness(0, 5, -270, 10);
    //        Animanion.AccelerationRatio = 0.9;
    //        Animanion.DecelerationRatio = 0.1;
    //        Animanion.SpeedRatio = 1.3;
    //        return Animanion;
    //    }
    //}
}
