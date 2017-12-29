using PoC.Encryption;
using PoC.TokenAuthentication.Context;
using PoC.TokenAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Repositories
{
    public class ClientKeyRepository: Bases.BaseRepository
    {
        LocalContext _context;
        public ClientKeyRepository()
        {
            _context = new LocalContext();
        }

        public void GenerateUniqueKey(out string ClientID, out string ClientSecert)
        {
            ClientID = KeyGenerator.GetUniqueKey();
            ClientSecert = KeyGenerator.GetUniqueKey();
        }

        public bool IsUniqueKeyAlreadyGenerate(long UserID)
        {
            bool keyExists = _context.ClientKeys.Any(clientkeys => clientkeys.UserID.Equals(UserID));

            if (keyExists)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int SaveClientIDandClientSecert(ClientKey ClientKeys)
        {
            _context.ClientKeys.Add(ClientKeys);
            return _context.SaveChanges();
        }

        public ClientKey GetGenerateUniqueKeyByUserID(long UserID)
        {
            var clientkey = (from ckey in _context.ClientKeys
                             where ckey.UserID == UserID
                             select ckey).FirstOrDefault();
            return clientkey;
        }


        public int UpdateClientIDandClientSecert(ClientKey ClientKeys)
        {
            _context.Entry(ClientKeys).State = EntityState.Modified;
            _context.SaveChanges();
            return _context.SaveChanges();
        }

    }
}