using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Ana.Contracts.Business;
using Ana.Web.Models;
using StructureMap;

namespace Ana.Web.Authentication
{
    public class OAuth2Attribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string token = actionContext.Request.Headers.Authorization.Parameter;

                var authManager = ObjectFactory.GetInstance<IOAuth2Manager>();
                
                try
                {
                    var grant = authManager.ValidateUserWithToken(token);
                    
                }
                catch (AccessViolationException ex)
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }

            }

            base.OnActionExecuting(actionContext);
        }
    }

}
