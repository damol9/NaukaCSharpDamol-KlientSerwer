using System.IO;
using System.Net.Sockets;
using System.Windows;

namespace NaukaCSharpDamol_Klient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void Query(string query)
        {
            TcpClient externalClient = new TcpClient();
            externalClient.Connect("127.0.0.1", 1024);
            BinaryWriter writer = new BinaryWriter(externalClient.GetStream());
            BinaryReader reader = new BinaryReader(externalClient.GetStream());
            writer.Write(query);
            MessageBox.Show(reader.ReadInt32().ToString(), "Klient");
            externalClient.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Query(TextBox.Text);
        }
    }
}
