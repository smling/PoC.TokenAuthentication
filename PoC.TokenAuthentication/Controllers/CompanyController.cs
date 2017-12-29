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
    public class CompanyController : Controller
    {
        CompanyRepository _IRegister;
        public CompanyController()
        {
            _IRegister = new CompanyRepository();
        }

        // GET: Register
        public ActionResult Index()
        {
            var RegisterList = _IRegister.ListofCompanies(Convert.ToInt32(Session["UserID"]));
            return View(RegisterList);
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            var Company = _IRegister.CheckIsCompanyRegistered(Convert.ToInt32(Session["UserID"]));
            if (Company)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(Company RegisterCompany)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", RegisterCompany);
                }

                if (_IRegister.ValidateCompanyName(RegisterCompany))
                {
                    ModelState.AddModelError("", "Company is Already Registered");
                    return View("Create", RegisterCompany);
                }
                //RegisterCompany.UserID = Convert.ToInt32(Session["UserID"]);
                RegisterCompany.CreatedAt = DateTime.Now;
                _IRegister.Add(RegisterCompany);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}