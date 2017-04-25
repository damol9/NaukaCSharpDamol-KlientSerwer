using System.Windows;

namespace NaukaCSharpDamol_Klient
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        private void Window2_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isWindow2Active = false;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;
            if (login != "" && password != "" && PasswordRepeat.Password != "")
            {
                if (password == PasswordRepeat.Password)
                {
                    if (PasswordTools.isLoginProper(login) && PasswordTools.isPasswordProper(password))
                    {
                        if (PasswordTools.isPasswordStrong(password))
                        {
                            if (RulesCheck.IsChecked == true)
                            {
                                int excode = MainWindow.Query("register " + login + ' ' + Encryption.EncryptSHA512Managed(password));
                                if (excode == 0)
                                {
                                    MessageBox.Show("Nieznany błąd, skontaktuj się z administratorem.", "Błąd");
                                }
                                else if (excode == 1)
                                {
                                    MessageBox.Show("Zarejestrowano.", "Info");
                                }
                                else if (excode == 2)
                                {
                                    MessageBox.Show("Taki użytkownik już istnieje.", "Błąd");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Musisz zaakceptować regulamin.", "Błąd");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hasło nie spełnia wymogów. Hasło musi posiadać 8-20 znaków, przynajmniej jedną cyfrę, jedną małą oraz jedną dużą literę.", "Błąd");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login lub hasło zawierają niedozwolone znaki.", "Błąd");
                    }
                }
                else
                {
                    MessageBox.Show("Podane hasła nie zgadzają się.", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Żadne z pól nie może być puste.", "Błąd");
            }
        }
    }
}