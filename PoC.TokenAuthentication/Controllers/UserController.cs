using PoC.Encryption.AES256;
using PoC.Encryption.Bases;
using PoC.TokenAuthentication.Models;
using PoC.TokenAuthentication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoC.TokenAuthentication.Controllers
{
    public class UserController : Controller
    {
        private BaseEncoder encoder = new AES256Encoder();

        UserRepository repository;
        public UserController()
        {
            repository = new UserRepository();
        }
        // GET: RegisterUser/Create
        public ActionResult Create()
        {
            return View(new User());
        }

        // POST: RegisterUser/Create
        [HttpPost]
        public ActionResult Create(User RegisterUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", RegisterUser);
                }

                // Validating Username 
                if (repository.ValidateUsername(RegisterUser))
                {
                    ModelState.AddModelError("", "User is Already Registered");
                    return View("Create", RegisterUser);
                }
                RegisterUser.CreatedAt = DateTime.Now;

                // Encrypting Password with AES 256 Algorithm
                //RegisterUser.Password = EncryptionLibrary.EncryptText(RegisterUser.Password);

                RegisterUser.Password = encoder.EncryptInString(RegisterUser.Password, Constants.USER_ENCODE_KEY);

                // Saving User Details in Database
                repository.Add(RegisterUser);
                TempData["UserMessage"] = "User Registered Successfully";
                ModelState.Clear();
                return View("Create", new User());
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}