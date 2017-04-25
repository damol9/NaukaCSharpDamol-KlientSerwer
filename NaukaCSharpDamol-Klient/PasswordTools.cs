namespace NaukaCSharpDamol_Klient
{
    class PasswordTools
    {
        public static bool isLoginProper(string login)
        {
            for (int i = 0; i < login.Length; i++)
            {
                if(! ( (login[i]>='A'&&login[i]<='Z') || (login[i] >= 'a' && login[i] <= 'z') || (login[i] >= '0' && login[i] <= '9') ) )
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isPasswordProper(string password)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i]==' ')
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isPasswordStrong(string password)
        {
            if (password.Length >= 8 && password.Length <= 20)
            {
                bool tCapital = false, tNormal = false, tNumber = false;
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] >= 'A' && password[i] <= 'Z')
                    {
                        tCapital = true;
                    }
                    else if (password[i] >= 'a' && password[i] <= 'z')
                    {
                        tNormal = true;
                    }
                    else if (password[i] >= '0' && password[i] <= '9')
                    {
                        tNumber = true;
                    }
                }
                //MessageBox.Show(tCapital.ToString()+tNormal.ToString()+tNumber.ToString());
                if (tCapital == true && tNormal == true && tNumber == true)
                    return true;
                else return false;
            }
            else return false;
        }
    }
}
