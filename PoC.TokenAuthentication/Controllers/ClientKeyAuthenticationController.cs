using PoC.TokenAuthentication.Models;
using PoC.TokenAuthentication.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoC.TokenAuthentication.Controllers
{
    public class ClientKeyAuthenticationController : ApiController
    {
        ClientKeyAuthenticationRepository _IAuthenticate;
        public ClientKeyAuthenticationController()
        {
            _IAuthenticate = new ClientKeyAuthenticationRepository();
        }

        // POST: api/Authenticate
        public HttpResponseMessage Authenticate([FromBody]ClientKey ClientKeys)
        {
            if (string.IsNullOrEmpty(ClientKeys.ClientKeyName) && string.IsNullOrEmpty(ClientKeys.ClientKeySecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return message;
            }
            else
            {
                if (_IAuthenticate.ValidateKeys(ClientKeys))
                {
                    var clientkeys = _IAuthenticate.GetClientKeysDetailsbyCLientIDandClientSecert(ClientKeys.ClientKeyName, ClientKeys.ClientKeySecret);

                    if (clientkeys == null)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                        message.Content = new StringContent("InValid Keys");
                        return message;
                    }
                    else
                    {
                        if (_IAuthenticate.IsTokenAlreadyExists(clientkeys.CompanyID))
                        {
                            _IAuthenticate.DeleteGenerateToken(clientkeys.CompanyID);

                            return GenerateandSaveToken(clientkeys);
                        }
                        else
                        {
                            return GenerateandSaveToken(clientkeys);
                        }
                    }
                }
                else
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("InValid Keys");
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.NotAcceptable };
                }
            }
        }


        [NonAction]
        private HttpResponseMessage GenerateandSaveToken(ClientKey clientkeys)
        {
            var IssuedOn = DateTime.Now;
            var newToken = _IAuthenticate.GenerateToken(clientkeys, IssuedOn);
            Token token = new Token();
            token.TokenID = 0;
            token.TokenKey = newToken;
            token.CompanyID = clientkeys.CompanyID;
            token.IssuedAt = IssuedOn;
            token.ExpiredAt = DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpiry"]));
            token.CreatedAt = DateTime.Now;
            var result = _IAuthenticate.InsertToken(token);

            if (result == 1)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                response.Headers.Add("Token", newToken);
                response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["TokenExpiry"]);
                response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return response;
            }
            else
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Error in Creating Token");
                return message;
            }
        }
    }
}