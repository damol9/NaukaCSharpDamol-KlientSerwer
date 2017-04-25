using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace NaukaCSharpDamol_ServerLauncher
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static DispatcherTimer Timer1;
        public MainWindow()
        {
            InitializeComponent();
            Timer1 = new DispatcherTimer();
            Timer1.Tick += new EventHandler(CheckAndUpdate);
            Timer1.Interval = new TimeSpan(0, 0, 1);
            Timer1.Start();

        }
        private void CheckAndUpdate(Object source, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("NaukaCSharpDamol-Serwer");
            if (processes.Length > 0)
            {
                Status.Content = "ON";
            }
            else
            {
                Status.Content = "OFF";
            }
        }
        private void ServerStart()
        {
            try
            {
                Process.Start("NaukaCSharpDamol-Serwer.exe");
            }
            catch(Win32Exception ex)
            {
                MessageBox.Show("Can't find .exe of server.\n\n\nDEBUG INFO:\n\n" + ex, "Error");
            }
        }
        private void ServerStop()
        {

            Process[] processes = Process.GetProcessesByName("NaukaCSharpDamol-Serwer");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ServerStop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ServerStart();
        }
    }
}