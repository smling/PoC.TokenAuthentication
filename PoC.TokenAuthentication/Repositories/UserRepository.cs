using PoC.TokenAuthentication.Context;
using PoC.TokenAuthentication.Models;
using PoC.TokenAuthentication.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Repositories
{
    public class UserRepository : BaseRepository
    {

        LocalContext _context;
        public UserRepository()
        {
            _context = new LocalContext();
        }

        public void Add(User registeruser)
        {
            _context.Users.Add(registeruser);
            _context.SaveChanges();
        }

        public long GetLoggedUserID(User registeruser)
        {
            long usercount = (from User in _context.Users
                              where User.UserName == registeruser.UserName &&
                                    User.Password == registeruser.Password
                              select User.UserID).FirstOrDefault();

            return usercount;
        }

        public bool ValidateRegisteredUser(User registeruser)
        {
            long usercount = (from User in _context.Users
                              where User.UserName == registeruser.UserName &&
                              User.Password == registeruser.Password
                              select User).Count();
            if (usercount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateUsername(User registeruser)
        {
            long usercount = (from User in _context.Users
                              where User.UserName == registeruser.UserName
                              select User).Count();
            if (usercount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}