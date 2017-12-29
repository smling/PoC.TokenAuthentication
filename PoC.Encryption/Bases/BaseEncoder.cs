using PoC.Encryption.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Encryption.Bases
{
    public abstract class BaseEncoder : IDecrypter, IEncrypter
    {
        public abstract byte[] Encrypt(byte[] plainBytes, byte[] key);
        public abstract byte[] Decrypt(byte[] cipherBytes, byte[] key);

        public string EncryptInString(string plainText, string key)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = this.Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }
        public string DecryptInString(string chiperText, string key)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(chiperText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = this.Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
    }
}
