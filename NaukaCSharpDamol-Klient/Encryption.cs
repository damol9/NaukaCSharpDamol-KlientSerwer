﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace NaukaCSharpDamol_Klient
{
    class Encryption
    {
        public static string EncryptSHA512Managed(string password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(password);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            return Convert.ToBase64String(hash);
        }
    }
}