using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Encryption.Interfaces
{
    public interface IEncrypter
    {
        byte[] Encrypt(byte[] plainBytes, byte[] key);
    }
}
