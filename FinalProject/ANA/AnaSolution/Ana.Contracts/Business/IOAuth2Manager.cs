using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Web.Models;

namespace Ana.Contracts.Business
{
    public interface IOAuth2Manager
    {
        OAuthModel GetMyDeveloperSettings();
        void UpdateDeveloperSettings(OAuthModel authModel);

        GrantModel GetGrant(string client_id);
        GrantModel GetGrant(string client_id, string code, string secret);
        void UpdateGrant(GrantModel grantModel);

        GrantModel ValidateUserWithToken(string token);

        bool IsClientValid(string clientId, string redirectUri);
    }
}
