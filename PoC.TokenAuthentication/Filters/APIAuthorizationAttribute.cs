using PoC.Encryption.AES256;
using PoC.Encryption.Bases;
using PoC.TokenAuthentication.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PoC.TokenAuthentication.Filters
{
    public class APIAuthorizationAttribute : AuthorizeAttribute
    {
        private LocalContext db = new LocalContext();
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            try
            {
                var encodedString = actionContext.Request.Headers.GetValues("Token").First();

                bool validFlag = false;

                if (!string.IsNullOrEmpty(encodedString))
                {
                    BaseEncoder encoder = new AES256Encoder();
                    var key = encoder.DecryptInString(encodedString,Constants.TOKEN_ENCODE_KEY);

                    string[] parts = key.Split(new char[] { ':' });

                    var UserID = Convert.ToInt64(parts[0]);       // UserID
                    var RandomKey = parts[1];                     // Random Key
                    var CompanyID = Convert.ToInt64(parts[2]);    // CompanyID
                    long ticks = long.Parse(parts[3]);            // Ticks
                    DateTime IssuedOn = new DateTime(ticks);
                    var ClientID = parts[4];                      // ClientID 


                    // By passing this parameter 
                    var registerModel = (from register in db.ClientKeys
                                         where register.CompanyID == CompanyID
                                         && register.UserID == UserID
                                         && register.ClientKeyName == ClientID
                                         select register).FirstOrDefault();

                    if (registerModel != null)
                    {
                        // Validating Time
                        var ExpiresOn = (from token in db.Tokens
                                         where token.CompanyID == CompanyID
                                         select token.ExpiredAt).FirstOrDefault();

                        if ((DateTime.Now > ExpiresOn))
                        {
                            validFlag = false;
                        }
                        else
                        {
                            validFlag = true;
                        }
                    }
                    else
                    {
                        validFlag = false;
                    }
                }
                return validFlag;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}