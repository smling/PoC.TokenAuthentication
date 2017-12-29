using PoC.TokenAuthentication.Context;
using PoC.TokenAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Repositories
{
    public class CompanyRepository : Bases.BaseRepository
    {
        LocalContext _context;
        public CompanyRepository()
        {
            _context = new LocalContext();
        }

        public IEnumerable<Company> ListofCompanies(long UserID)
        {
            try
            {
                var CompanyList = (from companies in _context.Companies
                                   where companies.InChargeUserID == UserID
                                   select companies).ToList();
                return CompanyList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Add(Company entity)
        {
            try
            {
                _context.Companies.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Delete(Company entity)
        {
            try
            {
                var itemToRemove = _context.Companies.SingleOrDefault(x => x.CompanyID == entity.CompanyID);
                _context.Companies.Remove(itemToRemove);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Company FindCompanyByUserId(long UserID)
        {
            try
            {
                var Company = _context.Companies.SingleOrDefault(x => x.InChargeUserID == UserID);
                return Company;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool ValidateCompanyName(Company registercompany)
        {
            try
            {
                var result = (from company in _context.Companies
                              where company.CompanyName == registercompany.CompanyName &&
                                    company.EmailAddress == registercompany.EmailAddress
                              select company).Count();
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


        public bool CheckIsCompanyRegistered(long UserID)
        {
            try
            {
                bool companyExists = _context.Companies.Any(x => x.InChargeUserID == UserID);
                if (companyExists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}