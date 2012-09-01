using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Business;
using System.Web;
using System.Web.Security;
using Ana.Contracts.Repository;
using Ana.Domain;

namespace Ana.Business.Managers
{
    public class HttpContextUserProvider: IUserProvider
    {
        public Domain.User GetUser()
        {
            var wrapper = new HttpContextWrapper(HttpContext.Current);
            

            if (wrapper.Session != null && wrapper.User.Identity.IsAuthenticated &&
                    wrapper.Session["CURRENT_USER"] != null)
            {
                var user = wrapper.Session["CURRENT_USER"] as User;

                return user;
            }

            if (wrapper.Items["CURRENT_USER"] != null && wrapper.User.Identity.IsAuthenticated)
                return wrapper.Items["CURRENT_USER"] as Domain.User;


            throw new AccessViolationException("User is not loged on");
        }

        public string GetUserId()
        {
            return this.GetUser().RowKey;
        }


        public bool HasUser()
        {
            var wrapper = new HttpContextWrapper(HttpContext.Current);

            if (wrapper.Session != null)
                return (wrapper.User.Identity.IsAuthenticated &&
                        wrapper.Session["CURRENT_USER"] != null &&
                        wrapper.Session["CURRENT_USER"] is User);

            return (wrapper.Items["CURRENT_USER"] != null && wrapper.User.Identity.IsAuthenticated);

        }



        public void SetUser(User user)
        {
            var wrapper = new HttpContextWrapper(HttpContext.Current);
            if (wrapper.Session != null)
            {
                wrapper.Session.Add("CURRENT_USER", user);
            }

            wrapper.Items["CURRENT_USER"] = user;
        }
    }
    
}
