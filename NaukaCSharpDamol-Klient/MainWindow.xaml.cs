#define DEBUG
#undef DEBUG

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace NaukaCSharpDamol_Klient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private int timeleft;
        public static bool isWindow2Active=false;
        private static DispatcherTimer Timer1;
        public MainWindow()
        {
            Window1 Okno = new Window1();
            Okno.Show();
            ConnectionInit(Okno);
            InitializeComponent();
        }
        private void ConnectionInit(Window1 uchwyt)
        {

            try
            {
                int result = Query("connection_test");
                Thread.Sleep(1000);
                if (result == 1) uchwyt.Close();
            }
            catch (Exception ex)
            {
#if (DEBUG)
                MessageBox.Show(ex.ToString(), "DEBUG INFORMATION");
#endif

                MessageBox.Show("Can't connect to server. Server is propably offline. Exiting.");
                Environment.Exit(0);
            }
        }
        public static int Query(string query)
        {
            TcpClient externalClient = new TcpClient();
            externalClient.Connect("127.0.0.1", 1024);
            BinaryWriter writer = new BinaryWriter(externalClient.GetStream());
            BinaryReader reader = new BinaryReader(externalClient.GetStream());
            writer.Write(query);
            int excode = reader.ReadInt32();
            //MessageBox.Show(excode.ToString(), "Klient");
            externalClient.Close();
            return excode;
        }
        private void Odliczanie(Object source, EventArgs e)
        {
            if (timeleft > 0)
            {
                TimerUpdate();
                timeleft--;
            }
            else
            {
                Licznik.Content = "";
                Timer1.Stop();
                LoginButton.IsEnabled = true;
            }
        }
        private void TimerUpdate()
        {
            Licznik.Content = timeleft;
        }
        private void Start_Odliczania()
        {
            CzyscPola();
            timeleft = 5;
            Timer1 = new DispatcherTimer();
            Timer1.Tick += new EventHandler(Odliczanie);
            Timer1.Interval = new TimeSpan(0, 0, 1);
            Timer1.Start();
            LoginButton.IsEnabled = false;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox1.ToString() != "" || PasswordBox1.Password != "")
            {
                if (PasswordTools.isLoginProper(LoginBox1.Text) && PasswordTools.isPasswordProper(PasswordBox1.Password))
                {
                    int excode = Query("login " + LoginBox1.Text + ' ' + Encryption.EncryptSHA512Managed(PasswordBox1.Password));
                    if (excode == 0)
                    {
                        Start_Odliczania();
                        MessageBox.Show("Nieznany błąd, skontaktuj się z administratorem.", "Błąd");
                    }
                    else if (excode == 1)
                    {
                        MessageBox.Show("Zalogowano!", "Info");
                        LoginButton.IsEnabled = false;
                        LogoutButton.IsEnabled = true;
                        LoggedAs.Content = LoginBox1.Text;
                        CzyscPola();
                    }
                    else if (excode == 2)
                    {
                        Start_Odliczania();
                        MessageBox.Show("Podałeś błędne hasło.", "Błąd");
                    }
                    else if (excode == 3)
                    {
                        Start_Odliczania();
                        MessageBox.Show("Nie ma takiego użytkownika.", "Błąd");
                    }
                }
                else
                {
                    MessageBox.Show("Login lub hasło zawierają niedozwolone znaki.", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Żadne z pól nie może być puste.", "Błąd");
            }
        }

        private void CzyscPola()
        {
            LoginBox1.Text = "";
            PasswordBox1.Password = "";
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CzyscPola();
            LoggedAs.Content = "---";
            LoginButton.IsEnabled = true;
            LogoutButton.IsEnabled = false;
        }

        private void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isWindow2Active)
            {
                Window2 okno2 = new Window2();
                isWindow2Active = true;
            }
        }
    }
}
