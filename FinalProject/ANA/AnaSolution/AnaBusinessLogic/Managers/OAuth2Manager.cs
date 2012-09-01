using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Ana.Contracts.Business;
using Ana.Contracts.Repository;
using Ana.Domain;
using Ana.Utils;
using Ana.Web.Models;

namespace Ana.Business.Managers
{
    public class OAuth2Manager : BaseManager, IOAuth2Manager
    {
        private IEmailProvider _emailProvider;
        private IUserManager _userManager;
        private IDeveloperRepository _developerRepository;
        private IGrantRepository _grantRepository;

        public OAuth2Manager(IEmailProvider emailProvider
            ,IUserManager userManager
            ,IDeveloperRepository developerRepository
            ,IGrantRepository grantRepository)
        {
            _emailProvider = emailProvider;
            _userManager = userManager;
            _developerRepository = developerRepository;
            _developerRepository.CreateIfNotExist();

            _grantRepository = grantRepository;
            _grantRepository.CreateIfNotExist();
        }

        public OAuthModel GetMyDeveloperSettings()
        {
            var user = CurrentUser();

            var dev =_developerRepository.Query
                .Where(d => d.PartitionKey == user.RowKey)
                .FirstOrDefault();

            //Creates configuration
            if (dev == null)
            {
                dev = new Developer()
                          {
                              PartitionKey = user.RowKey,
                              RowKey = ShortGuid.NewGuid(),
                              enabled = false,
                              client_id = ShortGuid.NewGuid(),
                          };
                _developerRepository.AddEntity(dev);
            }


            return new OAuthModel()
                       {
                           id = dev.RowKey,
                           enabled = dev.enabled,
                           application_name = dev.application_name,
                           user_name = dev.PartitionKey,
                           client_id = dev.client_id,
                           redirect_uri = dev.redirect_uri,
                           secret = dev.secret,
                       };
        }

        public void UpdateDeveloperSettings(OAuthModel authModel)
        {
            var user = CurrentUser();
            var dev = _developerRepository.Query
                .Where(d => d.PartitionKey == user.RowKey
                            && d.RowKey == authModel.id)
                            .FirstOrDefault();

            
            dev.redirect_uri = authModel.redirect_uri;

            dev.enabled = authModel.enabled;

            if (dev.application_name != authModel.application_name)
            {
                dev.application_name = authModel.application_name;
                //regenerates secrets and client_id
                dev.secret = ShortGuid.NewGuid();
                dev.client_id = ShortGuid.NewGuid();
            }
            _developerRepository.AddOrUpdateEntity(dev);

            //TODO complete this
           _emailProvider.SendEmail(user.Email, "Developer Configurations", authModel.secret );
        }

        public GrantModel GetGrant(string clientId)
        {
            var user = CurrentUser();
            var grant = _grantRepository.Query
                .Where(g => g.user == user.RowKey && g.client_id == clientId)
                .FirstOrDefault();

            if(grant == null)
            {
                var token = ShortGuid.NewGuid();
                grant = new Grant()
                            {
                                client_id = clientId,
                                user = user.RowKey,
                                code = ShortGuid.NewGuid(),
                                token = token,
                                created = DateTime.Now,
                                expires = DateTime.Now.AddHours(1),
                                PartitionKey = token,
                                RowKey = token
                            };
                _grantRepository.AddOrUpdateEntity(grant);
            }

            return new GrantModel()
                       {
                           client_id = grant.client_id,
                           user = grant.user,
                           code = grant.code,
                           token = grant.RowKey,
                           created = grant.created,
                           expires = grant.expires,
                       };
        }

        public GrantModel GetGrant(string clientId, string code, string secret)
        {
            var grant = _grantRepository.Query
                .Where(g => g.code == code && g.client_id == clientId)
                .FirstOrDefault();

            
            var dev = _developerRepository.Query
                .Where(d => d.client_id == clientId)
                .FirstOrDefault();

            
            if (grant == null || dev == null || dev.secret != secret)
                return null;


            return new GrantModel()
                       {
                           client_id = grant.client_id,
                           user = grant.user,
                           code = grant.code,
                           token = grant.RowKey,
                           created = grant.created,
                           expires = grant.expires,
                       };
        }

        public void UpdateGrant(GrantModel grantModel)
        {
            throw new NotImplementedException();
        }

        public GrantModel ValidateUserWithToken(string token)
        {
            var grant = _grantRepository.Query
                .Where(g => g.PartitionKey == token && g.RowKey == token)
                .FirstOrDefault();

            if (grant == null)
                throw new AccessViolationException("invalid token");
            //do more validations

            var user = _userManager.GetUser(grant.user);
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user.name), null);
                    
            _userManager.SetCurrentUser(user);

            return new GrantModel()
                       {
                           user = grant.user,
                           client_id = grant.client_id,
                           token = grant.RowKey,
                           code = grant.code,
                           created = grant.created,
                           expires = grant.expires,
                       };
        }


        public bool IsClientValid(string clientId, string redirectUri)
        {
            var dev = _developerRepository.Query
                       .Where(d => d.client_id == clientId)
                       .FirstOrDefault();

            return (dev != null && dev.redirect_uri == redirectUri);
        }
    }
}
