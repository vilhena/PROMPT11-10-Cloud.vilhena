using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Ana.Utils.ACS.Hrd;
using Ana.Utils.ACS.Identity;
using Ana.Web.Models;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Web;
using System.Linq;
using Ana.Contracts.Business;
using Ana.Domain;
using Ana.Utils;

namespace Ana.Web.Controllers
{
    public class AccountController : Controller
    {

        private const string CLAIMS_PRIVATE_KEY = "AAddHU(sh!$%$e3807UOASIJD0AS()Q=(HDAsj";

        private IUserManager _userManager;

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
        }


        public JsonResult SearchUser(string name)
        {
            //change this to search by name
            var users = _userManager.GetAllUsers()
                .Where(u => u.name.ToLower().Contains(name.ToLower()))
                .Take(10)
                .Select(u=>u.name);

            return Json(users.ToArray());
        }



        /// <summary>
        /// Action used to sign user in from ACS
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SignIn(FormCollection forms)
        {
            //Extract claims
            var principal = HttpContext.User;
            var claim = new IdentityClaim(principal.Identity as IClaimsIdentity);

            //Delete session cookie so the module cannot reset principal
            var sam = FederatedAuthentication.SessionAuthenticationModule;
            sam.DeleteSessionTokenCookie();

            if (claim.HasIdentity )
            {
                //Ger returnUrl
                string returnUrl = GetUrlFromContext(forms) ?? Url.Action("Index", "Home", null);

                var user = Membership.GetUser(DeterministicGuid.GetDeterministicGuid(claim.IdentityValue));
                
                //If the identity exists, sets the new user identity on the Membership
                if (user != null)
                {
                    //Find user if the user is allready registered
                    var identity = _userManager.GetUser(user.ProviderUserKey.ToString());

                    if (identity == null)
                    {
                        //Clears Membership
                        Membership.DeleteUser(user.UserName);

                        ModelState.AddModelError("", "Please there was a problem with your username. Try again");
                        return RedirectToAction("LogOn", new { ReturnUrl = returnUrl });
                    }

                    if (Membership.ValidateUser(identity.name, CLAIMS_PRIVATE_KEY))
                    {

                        FormsAuthentication.SetAuthCookie(identity.name, false);
                        IdentityClaim.ClearSession();

                        _userManager.SetCurrentUser(identity);

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please there was a problem with your username. Try again");
                        return RedirectToAction("LogOn", new { ReturnUrl = returnUrl });
                    }
                }
                else
                {
                    //Save identity values for processing in the association page
                    claim.SaveToSession();

                    var model = new RegisterClaimsModel()
                                    {
                                        Email = claim.Email,
                                        Name = claim.Name
                                    };

                    return RedirectToAction("RegisterClaims", new {ReturnUrl = returnUrl});

                }

            }

            return RedirectToAction("LogOn", "Account");
        }


        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            var hrdClient = new HrdClient();


            //get Providers
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var request = new HrdRequest(fam.Issuer, fam.Realm, context: Request.QueryString["ReturnUrl"]);
            var hrdIdentityProviders = hrdClient.GetHrdResponse(request);

            var logon = new LogOnModel()
                            {
                                Providers = hrdIdentityProviders
                                    .Select(hrd => new IdentityProviderModel()
                                                       {
                                                           Name = hrd.Name,
                                                           LoginUrl = hrd.LoginUrl,
                                                           LogoutUrl = hrd.LogoutUrl
                                                       })
                            };

            return View(logon);
        }


        //logon using user forms membership, in this case user provides user and password

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                
                
                if (Membership.ValidateUser(model.UserName, model.Password))
                {

                    var user = Membership.GetUser(model.UserName);

                    //Find user if the user is allready registered
                    var identity = _userManager.GetUser(user.ProviderUserKey.ToString());
                        //UsersStorage.UserIdentities.Find(u => u.Id == user.ProviderUserKey.ToString());


                    if (identity != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        _userManager.SetCurrentUser(identity);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User is not allowed to login.");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            //get Providers
            var hrdClient = new HrdClient();
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var request = new HrdRequest(fam.Issuer, fam.Realm, context: Request.QueryString["ReturnUrl"]);
            var hrdIdentityProviders = hrdClient.GetHrdResponse(request);
            model.Providers = hrdIdentityProviders
                                    .Select(hrd => new IdentityProviderModel()
                                                       {
                                                           Name = hrd.Name,
                                                           LoginUrl = hrd.LoginUrl,
                                                           LogoutUrl = hrd.LogoutUrl
                                                       });

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult RegisterClaims(string returnUrl)
        {
            var claim = new IdentityClaim();
            
            if (!claim.HasIdentity)
            {
                return RedirectToAction("LogOn", new {ReturnUrl = returnUrl});
            }

            var model = new RegisterClaimsModel()
                            {
                                Email = claim.Email,
                                Name = claim.Name,
                                IdentityProvider = IdentityClaim.ProviderNiceName(claim.IdentityProvider),
                            };

            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterClaims(RegisterClaimsModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var claim = new IdentityClaim();

                MembershipCreateStatus createStatus;

                Membership.CreateUser(model.Name, CLAIMS_PRIVATE_KEY, model.Email, null, null, true, DeterministicGuid.GetDeterministicGuid(claim.IdentityValue), out createStatus);

                if (createStatus == MembershipCreateStatus.Success && Membership.ValidateUser(model.Name,CLAIMS_PRIVATE_KEY))
                {
                    
                    if (claim.HasIdentity)
                    {
                        var user = Membership.GetUser(model.Name);

                        model.IdentityProvider = claim.IdentityProvider;
                        _userManager.RegisterNewUser(model, user.ProviderUserKey.ToString());

                        IdentityClaim.ClearSession();

                        _userManager.SetCurrentUser(_userManager.GetUser(user.ProviderUserKey.ToString()));
                    }
                    FormsAuthentication.SetAuthCookie(model.Name, false /* createPersistentCookie */);

                    

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    var user = Membership.GetUser(model.UserName);

                    _userManager.RegisterNewUser(model, user.ProviderUserKey.ToString());

                    IdentityClaim.ClearSession();   
   
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);

                    _userManager.SetCurrentUser(_userManager.GetUser(user.ProviderUserKey.ToString()));

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var user = _userManager.CurrentUser();

            return View(new UserModel
            {
                id = user.RowKey,
                name = user.UserName,
                email = user.Email,
                identity_provider = user.IdentityProvider
            });
        }



        /// <summary>
        /// Gets from the form the context
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private static string GetUrlFromContext(FormCollection form)
        {
            WSFederationMessage message = WSFederationMessage.CreateFromNameValueCollection(new Uri("http://www.notused.com"), form);
            return (message != null ? message.Context : null);
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

    }
}
