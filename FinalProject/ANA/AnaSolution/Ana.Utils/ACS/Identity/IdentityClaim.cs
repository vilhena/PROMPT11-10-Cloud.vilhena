using System.Web;
using Microsoft.IdentityModel.Claims;

namespace Ana.Utils.ACS.Identity
{
        public class IdentityClaim
        {
            public const string ACSProviderClaim = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

            public IdentityClaim(IClaimsIdentity identity)
            {

                if (identity != null)
                {
                    foreach (var claim in identity.Claims)
                    {
                        if (claim.ClaimType == ClaimTypes.NameIdentifier)
                        {
                            IdentityValue = claim.Value;
                        }
                        if (claim.ClaimType == ACSProviderClaim)
                        {
                            IdentityProvider = claim.Value;
                        }
                        if(claim.ClaimType == ClaimTypes.Email)
                        {
                            Email = claim.Value;
                        }
                        if(claim.ClaimType == ClaimTypes.Name)
                        {
                            Name = claim.Value;
                        }

                    }
                }

            }

            public IdentityClaim()
            {
                IdentityProvider = HttpContext.Current.Session["IdentityProvider"] as string;
                IdentityValue = HttpContext.Current.Session["IdentityValue"] as string;
                Email = HttpContext.Current.Session["Email"] as string;
                Name = HttpContext.Current.Session["Name"] as string;
            }

            public bool HasIdentity
            {
                get
                {
                    return (!string.IsNullOrEmpty(IdentityProvider) && (!string.IsNullOrEmpty(IdentityValue)));
                }
            }

            public string IdentityProvider { get; private set; }
            public string IdentityValue { get; private set; }
            public string Name { get; set; }
            public string Email { get; set; }

            public void SaveToSession()
            {
                HttpContext.Current.Session["IdentityProvider"] = IdentityProvider;
                HttpContext.Current.Session["IdentityValue"] = IdentityValue;
                HttpContext.Current.Session["Email"] = Email;
                HttpContext.Current.Session["Name"] = Name;
            }

            public static void ClearSession()
            {
                HttpContext.Current.Session.Remove("IdentityProvider");
                HttpContext.Current.Session.Remove("IdentityValue");
                HttpContext.Current.Session.Remove("Email");
                HttpContext.Current.Session.Remove("Name");
            }

            public static string ProviderNiceName(string identityProivder)
            {
                if (identityProivder.ToLower().Contains("windowslive"))
                    return "Windows Live";

                if (identityProivder.ToLower().Contains("facebook"))
                    return "Facebook";

                if (identityProivder.ToLower().Contains("yahoo"))
                    return "Yahoo";

                if (identityProivder.ToLower().Contains("google"))
                    return "Google";

                return identityProivder;
            }
        }


}