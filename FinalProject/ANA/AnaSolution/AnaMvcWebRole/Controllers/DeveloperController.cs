using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ana.Contracts.Business;
using Ana.Web.Models;

namespace Ana.Web.Controllers
{
    [Authorize]
    public class DeveloperController : Controller
    {
        private IOAuth2Manager _oauth2Manager;
        public DeveloperController(IOAuth2Manager oauth2Manager)
        {
            _oauth2Manager = oauth2Manager;
        }


        //
        // GET: /Developer/
        public ActionResult Index()
        {
            return View(_oauth2Manager.GetMyDeveloperSettings());
        }

        // GET: /Developer/Edit
        public ActionResult Edit()
        {
            return View(_oauth2Manager.GetMyDeveloperSettings());
        }

        [HttpPost]
        public ActionResult Edit(OAuthModel model)
        {
            if (ModelState.IsValid)
            {
                _oauth2Manager.UpdateDeveloperSettings(model);

                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}
