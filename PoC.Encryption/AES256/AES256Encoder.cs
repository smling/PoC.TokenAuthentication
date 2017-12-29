using PoC.Encryption.Bases;
using PoC.Encryption.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Encryption.AES256
{
    public class AES256Encoder :BaseEncoder
    {
        private CipherMode _cipherMode = CipherMode.CBC;
        private int _keySize = 256;
        private int _blockSize = 128;
        public override byte[] Decrypt(byte[] cipherBytes, byte[] key)
        {
            byte[] decryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                //    using (RijndaelManaged AES = InitialAlgrithm(key))
                //    {
                //        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                //        {
                //            cs.Write(cipherBytes, 0, cipherBytes.Length);
                //            cs.Close();
                //        }
                //        decryptedBytes = ms.ToArray();
                //    }
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var sessionkey = new Rfc2898DeriveBytes(key, saltBytes, 1000);
                    AES.Key = sessionkey.GetBytes(AES.KeySize / 8);
                    AES.IV = sessionkey.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
           
            return decryptedBytes;
        }

        public override byte[] Encrypt(byte[] plainBytes, byte[] key)
        {
            byte[] encryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = InitialAlgrithm(key))
                {
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        private RijndaelManaged InitialAlgrithm(byte[] key)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            Rfc2898DeriveBytes sessionKey = new Rfc2898DeriveBytes(key, saltBytes, 1000);
            RijndaelManaged AES = new RijndaelManaged()
            {
                KeySize = _keySize,
                BlockSize = _blockSize,
                Mode = _cipherMode
            };
            AES.Key = sessionKey.GetBytes(AES.KeySize / saltBytes.Length);
            AES.IV = sessionKey.GetBytes(AES.BlockSize / saltBytes.Length);
            return AES;
        }
    }
}
