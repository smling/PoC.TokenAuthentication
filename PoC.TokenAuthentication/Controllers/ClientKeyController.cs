using PoC.TokenAuthentication.Filters;
using PoC.TokenAuthentication.Models;
using PoC.TokenAuthentication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoC.TokenAuthentication.Controllers
{
    [ValidateSession]
    public class ClientKeyController : Controller
    {
        ClientKeyRepository _IClientKeys;
        CompanyRepository _IRegisterCompany;
        public ClientKeyController()
        {
            _IClientKeys = new ClientKeyRepository();
            _IRegisterCompany = new CompanyRepository();
        }

        // GET: ApplicationKeys/GenerateKeys
        [HttpGet]
        public ActionResult GenerateKeys()
        {
            try
            {
                ClientKey clientkeys = new ClientKey();

                // Validating ClientID and ClientSecert already Exists
                var keyExists = _IClientKeys.IsUniqueKeyAlreadyGenerate(Convert.ToInt64(Session["UserID"]));

                if (keyExists)
                {
                    // Getting Generate ClientID and ClientSecert Key By UserID
                    clientkeys = _IClientKeys.GetGenerateUniqueKeyByUserID(Convert.ToInt64(Session["UserID"]));
                }
                else
                {
                    string clientID = string.Empty;
                    string clientSecert = string.Empty;
                    long companyId = 0;

                    var company = _IRegisterCompany.FindCompanyByUserId(Convert.ToInt64(Session["UserID"]));
                    companyId = company.CompanyID;

                    //Generate Keys
                    _IClientKeys.GenerateUniqueKey(out clientID, out clientSecert);

                    //Saving Keys Details in Database
                    clientkeys.ClientKeyID = 0;
                    clientkeys.CompanyID = companyId;
                    clientkeys.CreatedAt = DateTime.Now;
                    clientkeys.ClientKeyName = clientID;
                    clientkeys.ClientKeySecret = clientSecert;
                    clientkeys.UserID = Convert.ToInt64(Session["UserID"]);
                    _IClientKeys.SaveClientIDandClientSecert(clientkeys);

                }

                return View(clientkeys);
            }
            catch (Exception)
            {
                throw;
            }
        }


        // POST: ApplicationKeys/GenerateKeys
        [HttpPost]
        public ActionResult GenerateKeys(ClientKey clientkeys)
        {
            try
            {
                string clientID = string.Empty;
                string clientSecert = string.Empty;

                //Generate Keys
                _IClientKeys.GenerateUniqueKey(out clientID, out clientSecert);

                //Updating ClientID and ClientSecert 
                var company = _IRegisterCompany.FindCompanyByUserId(Convert.ToInt32(Session["UserID"]));
                clientkeys.CompanyID = company.CompanyID;
                clientkeys.CreatedAt = DateTime.Now;
                clientkeys.ClientKeyName = clientID;
                clientkeys.ClientKeySecret = clientSecert;
                clientkeys.UserID = Convert.ToInt32(Session["UserID"]);
                _IClientKeys.UpdateClientIDandClientSecert(clientkeys);
                return RedirectToAction("GenerateKeys");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}