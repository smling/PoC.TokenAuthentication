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
    public class AuthenticationController : Controller
    {
        private BaseEncoder encoder = new AES256Encoder();
        private UserRepository _IRegisterUser;
        public AuthenticationController()
        {
            _IRegisterUser = new UserRepository();
        }

        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Login(User RegisterUser)
        {
            try
            {

                if (string.IsNullOrEmpty(RegisterUser.UserName) && (string.IsNullOrEmpty(RegisterUser.Password)))
                {
                    ModelState.AddModelError("", "Enter Username and Password");
                }
                else if (string.IsNullOrEmpty(RegisterUser.UserName)) 
                {
                    ModelState.AddModelError("", "Enter Username");
                }
                else if (string.IsNullOrEmpty(RegisterUser.Password))
                {
                    ModelState.AddModelError("", "Enter Password");
                }
                else
                {
                    //RegisterUser.Password = EncryptionLibrary.EncryptText(RegisterUser.Password);
                    RegisterUser.Password = encoder.EncryptInString(RegisterUser.Password, Constants.USER_ENCODE_KEY);
                    if (_IRegisterUser.ValidateRegisteredUser(RegisterUser))
                    {
                        var UserID = _IRegisterUser.GetLoggedUserID(RegisterUser);
                        Session["UserID"] = UserID;
                        return RedirectToAction("Create", "Company");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect user name / password.");
                        return View("Login", RegisterUser);
                    }
                }
                return View("Login", RegisterUser);
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }
    }
}