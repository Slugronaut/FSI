using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SAOT
{
    /// <summary>
    /// Offers basic service for generating salted hash values.
    /// Used primarily for password creation and authentication.
    /// </summary>
    public static class Crypto
    {
        public const int SaltIterations = 100000;
        public const int SaltLength = 16;
        public const int HashLength = 20;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SaltedHashOf(string input)
        {
            //generate a new salt every time, then salt n hash the password before
            //combining the final result with the salt for later recomposition
            //during comparisons.
            //If we really wanted to get dogg-nasty we'd also store the iteration count
            //and hell, maybe even an algo id or version number along with the hash and salt.
            byte[] salt;
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(salt = new byte[SaltLength]);
                using (var pbkdf2 = new Rfc2898DeriveBytes(input, salt, SaltIterations))
                {
                    var hash = pbkdf2.GetBytes(HashLength);

                    var result = new byte[HashLength + SaltLength];
                    Array.Copy(salt, 0, result, 0, SaltLength);
                    Array.Copy(hash, 0, result, SaltLength, HashLength);

                    return Convert.ToBase64String(result);
                }
            }
        }

        /// <summary>
        /// Compares a raw input string to a generated salthash.
        /// Returns <c>true</c> if the input matches the salthash and 
        /// <c>false</c> otherwise.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="saltHash"></param>
        /// <returns></returns>
        public static bool MatchesHash(string input, string saltHash)
        {
            byte[] hashBytes = Convert.FromBase64String(saltHash);
            byte[] salt = new byte[SaltLength];

            Array.Copy(hashBytes, 0, salt, 0, SaltLength);

            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, SaltIterations);
            byte[] hash = pbkdf2.GetBytes(HashLength);

            for(int i = 0; i < HashLength; i++)
            {
                if (hashBytes[i + SaltLength] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
