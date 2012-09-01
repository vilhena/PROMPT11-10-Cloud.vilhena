using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ana.Contracts.Business;
using Ana.Web.Models;

namespace Ana.Web.ControllersApi
{
    public class OAuth2Controller : Controller
    {
        private IOAuth2Manager _oauth2Manager;
        public OAuth2Controller(IOAuth2Manager oauth2Manager)
        {
            _oauth2Manager = oauth2Manager;
        }

        // GET: /OAuth2/Auth
        [Authorize]
        public ActionResult Auth(string response_type, string client_id, string redirect_uri)
        {
            if (response_type != "code")
            {
                throw new HttpException(400, "Invalid request");
            }

            //validate client_id and redirect_uri
            if (!_oauth2Manager.IsClientValid(client_id, redirect_uri))
            {
                throw new HttpException(404, "Client application not found");
            }

            return View(new OAuthApproveModel()
                            {
                                client_id = client_id,
                                redirect_uri = redirect_uri
                            });
        }

        [Authorize]
        public ActionResult Reprove()
        {
            throw new HttpException(401, "User does not autorize acess to data");
        }

        //
        // POST: /OAuth2/Approval
        [Authorize]
        [HttpPost]
        public ActionResult Approval(string client_id, string redirect_uri)
        {
            var userGrant = _oauth2Manager.GetGrant(client_id);

            return new RedirectResult(string.Format("{0}?code={1}", redirect_uri, userGrant.code));
        }

        //
        // POST: /OAuth2/Token
        [HttpPost]
        public ActionResult Token(string code, string redirect_uri, string grant_type, string client_id, string client_secret)
        {
            var grant = _oauth2Manager.GetGrant(client_id, code, client_secret);
            

            if(grant == null)
                throw new HttpException(400, "Invalid request");

            return new JsonResult()
            {
                Data = new
                {
                    access_token = grant.token,
                    token_type = "Bearer",
                    expires_in = 3600, //todo use created and updater
                    refresh_token = "1"
                }
            };
        }

    }
}