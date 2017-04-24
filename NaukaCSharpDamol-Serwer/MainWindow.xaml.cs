#define DEBUG

using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace NaukaCSharpDamol_Serwer
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string connStr = "server=localhost;user=root;database=tsl;port=3306;password=;";
        public static MySqlDataReader rdr;
        private static MySqlConnection conn = new MySqlConnection(connStr);
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1024);
        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public MainWindow()
        {
            Window1 Okno = new Window1();
            Okno.Show();
            MySQL_Init(Okno);
            Server_Start();
        }
        private void Server_Start()
        {
            listener.Start();
            while (true)
            {
                BackgroundWorker t = new BackgroundWorker();
                t.DoWork += new DoWorkEventHandler(Listen);
                t.RunWorkerAsync();
                resetEvent.WaitOne();
            }

        }
        private void Server_Stop()
        {
            resetEvent.Close();
        }
        private void Listen(object sender, DoWorkEventArgs e)
        {
            TcpClient newClient = listener.AcceptTcpClient();
            BinaryWriter writer = new BinaryWriter(newClient.GetStream());
            BinaryReader reader = new BinaryReader(newClient.GetStream());
            string query = reader.ReadString();
#if(DEBUG)
            MessageBox.Show(query,"DEBUGonSERVER - query");
#endif
            int excode = ProcessQuery(query);
            writer.Write(excode);
            newClient.Close();
            resetEvent.Set();
        }
        private int ProcessQuery(string query)
        {
            string[] query_split = query.Split(' ');
            if (query_split[0] == "connection_test")
            {
                return 1;
            }
            else if (query_split[0] == "login")
            {
                return login(query_split[1], query_split[2]);
            }
            else if (query_split[0] == "register")
            {
                return register(query_split[1], query_split[2]);
            }
            else
            {
                return 0;
            }
        }
        private int login(string login, string password)
        {
            string DBResult_login, DBResult_password;
            DBResult_login = MySQL_LoginQuery("SELECT konta.login FROM konta WHERE konta.login = \"" + login + "\";");
            rdr.Close();
            if (DBResult_login == "")
            {
                return 3;
            }
            else
            {
                DBResult_password = MySQL_LoginQuery("SELECT konta.password FROM konta WHERE konta.login = \"" + login + "\";");
                rdr.Close();
                if (DBResult_password == password && DBResult_login == login)
                {
                    MySQL_UpdateQuery("UPDATE `konta` SET `LastLogin` = CURRENT_TIMESTAMP WHERE `konta`.`login` = \"" + login + "\";");
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        private int register(string login, string password)
        {
            string DBResult_login = MySQL_LoginQuery("SELECT konta.login FROM konta WHERE konta.login = \"" + login + "\";");
            rdr.Close();
            if (DBResult_login.ToLower() != login.ToLower())
            {

                try
                {
                    MySQL_UpdateQuery("INSERT INTO `konta` (`id`, `login`, `password`, `DateRegistered`, `LastLogin`) VALUES(NULL, '" + login + "', '" + password + "', CURRENT_TIMESTAMP, '')");
                    return 1;
                }
                catch (Exception ex)
                {
#if (DEBUG)
                    MessageBox.Show(ex.ToString(), "DEBUG INFORMATION");
                    MessageBox.Show(ex.GetType().ToString(), "Error - debug");
#endif
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        
        }
        private void MySQL_Init(Window1 uchwyt)
        {
            try
            {
                conn.Open();
                Thread.Sleep(1000);
                uchwyt.Close();

            }
            catch (Exception ex)
            {
#if (DEBUG)
                MessageBox.Show(ex.ToString(), "DEBUG INFORMATION");
#endif

                MessageBox.Show("Can't connect to MySQL database. Exiting.");
                Environment.Exit(0);
            }
        }
        public static string MySQL_LoginQuery(string query)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read()) return rdr[0].ToString();
                else return "";


            }
            catch (Exception ex)
            {
#if (DEBUG)
                MessageBox.Show(ex.ToString(), "DEBUG INFORMATION");
                MessageBox.Show(ex.GetType().ToString(), "Error - debug");
#endif
                MessageBox.Show("There was an error when processing your request.", "Error");
                return "";

            }
        }
        public static void MySQL_UpdateQuery(string query)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
#if (DEBUG)
                MessageBox.Show(ex.ToString(), "DEBUG INFORMATION");
                MessageBox.Show(ex.GetType().ToString(), "Error - debug");
#endif
                MessageBox.Show("There was an error when processing your request.", "Error");

            }
        }
    }
}
