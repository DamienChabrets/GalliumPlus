
using System.Text;
using BC = BCrypt.Net.BCrypt;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;

namespace Couche_Métier.Utilitaire
{
    /// <summary>
    /// Permet de crypter un string en SHA256
    /// </summary>
    public static class CryptString
    {
        /// <summary>
        /// Hash un message
        /// </summary>
        /// <param name="message"> message à hashé </param>
        /// <returns> le message hashé </returns>
        public static string Hash(string message)
        {
            return BC.HashPassword(message);
        }

        /// <summary>
        /// Permet de vérifier qu'un mot de passe et un hashage sont les mêmes
        /// </summary>
        public static bool Verify(string message,string encoded)
        {
            return BC.Verify(message, encoded); 
        }

        public static byte[] SaltAndHash(string password, string salt)
        {

            byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            Argon2id argon2id = new(saltedPassword);
            argon2id.MemorySize = 19456;
            argon2id.Iterations = 2;
            argon2id.DegreeOfParallelism = 1;
            return argon2id.GetBytes(32);
        }
    }
}
