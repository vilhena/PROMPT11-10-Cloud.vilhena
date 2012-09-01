using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;
using Ana.Web.Models;

namespace Ana.Contracts.Business
{
    public interface IUserManager : IManager
    {
        UserModel GetUser(string id);
        UserModel GetUserByUsername(string username);
        void RegisterNewUser(RegisterClaimsModel user, string id);
        void RegisterNewUser(RegisterModel user, string id);

        void UpdateMyUserProfile(UserModel user);
        IEnumerable<UserModel> GetAllUsers();
        void SetCurrentUser(UserModel user);
    }
}
