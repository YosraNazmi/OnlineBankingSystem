using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ABC_Bank.Helper
{
    public static class HashClass
    {
       
        private const int ITERATION = 10000;
        private const int NumBytes = 32;

        public static string HashGenerator(string password, byte[] salt = null, bool justHash = false)
        {

            if (salt == null || salt.Length != 16)
            {
                salt = new byte[128 / 8];
                using (var random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(salt);
                }
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: ITERATION,
                numBytesRequested: NumBytes

                ));

            if (justHash) return hashed;

            return $"{hashed}:{Convert.ToBase64String(salt)}";
        }

        public static bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
        {
            var passwordAndHashed = hashedPasswordWithSalt.Split(':');
            if (passwordAndHashed == null || passwordAndHashed.Length != 2)
                return false;
            var salt = Convert.FromBase64String(passwordAndHashed[1]);
            if (salt == null)
            {
                return false;
            }
            var checkPasswordHashed = HashGenerator(passwordToCheck, salt, true);
            if (string.Compare(passwordAndHashed[0], checkPasswordHashed) == 0)
            {
                return true;
            }
            return false;
        }
    }

}

