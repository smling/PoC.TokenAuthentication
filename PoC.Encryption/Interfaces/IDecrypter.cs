using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Encryption.Interfaces
{
    public interface IDecrypter
    {
        byte[] Decrypt(byte[] cipherBytes, byte[] key);
    }
}
