using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace TNDStudios.Web.Blogs.Core.Helpers
{
    /// <summary>
    /// Cryptography algorythms used locally for this project
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// Calculate a hash for a given string (usually a password)
        /// </summary>
        /// <param name="input">The string to be hashed</param>
        /// <returns>The hash of the string</returns>
        public string CalculateHash(String input)
        {
            // Generate a salt for the hash
            byte[] salt = GenerateSalt(16);

            // Use the Key Derivation library to genate a hash using "Password-Based Key Derivation Function 2"
            // https://en.wikipedia.org/wiki/PBKDF2
            byte[] bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            // Return the Hash and the salt combo
            return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
        }

        /// <summary>
        /// Check that a given hash matches the input string
        /// </summary>
        /// <param name="hash">The hash to use to check</param>
        /// <param name="input">The string to check</param>
        /// <returns>If it matches</returns>
        public bool CheckMatch(string hash, String input)
        {
            try
            {
                // Pull apart the hash to seperate the salt and the hash parts
                var parts = hash.Split(':');

                // Get the salt
                var salt = Convert.FromBase64String(parts[0]);

                // use the key derivation algorithm with the salt (vector) to re-run the hash
                var bytes = KeyDerivation.Pbkdf2(input.ToString(), salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

                // Check equality of the resulting hash to see if they match
                return parts[1].Equals(Convert.ToBase64String(bytes));
            }
            catch
            {
                // Failed to work, so failed in general
                return false;
            }
        }

        /// <summary>
        /// Generate a salt to attach and use in the hash
        /// </summary>
        /// <param name="length">The lenght of the required salt</param>
        /// <returns>The salt as bytes</returns>
        private byte[] GenerateSalt(int length)
        {
            // Generate a new byte array
            byte[] salt = new byte[length];

            // Use the random number generator (not the standard Random function which is too predictable)
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt); // Populate the salt
            }

            // Return the salt bytes
            return salt;
        }
    }
}
