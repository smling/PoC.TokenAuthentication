using PoC.Encryption;
using PoC.Encryption.AES256;
using PoC.Encryption.Bases;
using PoC.TokenAuthentication.Context;
using PoC.TokenAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Repositories
{
    public class ClientKeyAuthenticationRepository: Bases.BaseRepository
    {
        LocalContext _context;
        BaseEncoder _encoder = new AES256Encoder();
        public ClientKeyAuthenticationRepository()
        {
            _context = new LocalContext();
        }

        public ClientKey GetClientKeysDetailsbyCLientIDandClientSecert(string clientID, string clientSecert)
        {
            try
            {
                var result = (from clientkeys in _context.ClientKeys
                              where clientkeys.ClientKeyName == clientID && clientkeys.ClientKeySecret == clientSecert
                              select clientkeys).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ValidateKeys(ClientKey ClientKeys)
        {
            try
            {
                var result = (from clientkeys in _context.ClientKeys
                              where clientkeys.ClientKeyName == ClientKeys.ClientKeyName && clientkeys.ClientKeySecret == ClientKeys.ClientKeySecret
                              select clientkeys).Count();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsTokenAlreadyExists(long CompanyID)
        {
            try
            {
                var result = (from token in _context.Tokens
                              where token.CompanyID == CompanyID
                              select token).Count();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteGenerateToken(long CompanyID)
        {
            try
            {
                var token = _context.Tokens.SingleOrDefault(x => x.CompanyID == CompanyID);
                _context.Tokens.Remove(token);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateToken(ClientKey ClientKeys, DateTime IssuedOn)
        {
            try
            {
                
                string randomnumber =
                   string.Join(":", new string[]
                   {   Convert.ToString(ClientKeys.UserID),
                KeyGenerator.GetUniqueKey(),
                Convert.ToString(ClientKeys.CompanyID),
                Convert.ToString(IssuedOn.Ticks),
                ClientKeys.ClientKeyName
                   });

                return _encoder.EncryptInString(randomnumber, Constants.TOKEN_ENCODE_KEY);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int InsertToken(Token token)
        {
            try
            {
                _context.Tokens.Add(token);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}