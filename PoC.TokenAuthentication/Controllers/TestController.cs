using PoC.TokenAuthentication.Context;
using PoC.TokenAuthentication.Filters;
using PoC.TokenAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PoC.TokenAuthentication.Controllers
{
    [APIAuthorization]
    public class TestController : ApiController
    {
        private LocalContext db = new LocalContext();

        // GET: api/LatestMusic
        public List<User> GetUsers()
        {
            try
            {
                var listofSongs = db.Users.ToList();
                return listofSongs;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}