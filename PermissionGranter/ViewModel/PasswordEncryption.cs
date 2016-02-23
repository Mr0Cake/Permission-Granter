using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using PermissionGranter.Model;

namespace PermissionGranter.ViewModel
{
    public static class PasswordEncryption
    {
        public static readonly int Iterations = 10;

        public static void EncryptPassword(ref string password, int iterations, out string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 20, Iterations)) // 20-byte salt
            {
                
                byte[] saltBytes = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(20); // 20-byte key

                salt = Convert.ToBase64String(saltBytes);
                password = Convert.ToBase64String(key);
            }
        }

        

        public static bool PasswordMatch(string password, string dbHash, string dbSalt, int iterations)
        {
            bool passwordMatch=false;
            // load encodedSalt and encodedKey from database for the given username
            byte[] salt = Convert.FromBase64String(dbSalt);
            byte[] key = Convert.FromBase64String(dbHash);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] testKey = deriveBytes.GetBytes(20); // 20-byte key
                passwordMatch = testKey.SequenceEqual(key);
            }
            return passwordMatch;
        }
        

    }
}
