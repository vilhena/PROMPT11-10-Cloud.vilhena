using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;
using Ana.Repository;
using Ana.Contracts.Business;
using Ana.Contracts.Repository;
using Ana.Utils;
using Ana.Web.Models;

namespace Ana.Business.Managers
{
    public class UserManager : BaseManager, IUserManager
    {
        private IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }

            userRepository.CreateIfNotExist();
            _userRepository = userRepository;
        }

        public UserModel GetUser(string id)
        {
            UserModel ret = null;

            try
            {
                var user = _userRepository
                        .Query
                        .Where(u => u.PartitionKey == "ana" && u.RowKey == id)
                        .FirstOrDefault();

                ret = new UserModel()
                        {
                            id = user.RowKey,
                            email = user.Email,
                            name = user.UserName,
                            identity_provider = user.IdentityProvider
                        };
            }
            catch (Exception)
            {

            }
            return ret;
        }

        public UserModel GetUserByUsername(string username)
        {
            var user = _userRepository.Query
                .Where(u => u.UserName == username)
                .FirstOrDefault();

            return new UserModel()
            {
                id = user.RowKey,
                email = user.Email,
                name = user.UserName,
                identity_provider = user.IdentityProvider
            };
        }

        public void RegisterNewUser(RegisterClaimsModel user, string id)
        {
            var newUser = new User()
            {
                PartitionKey = "ana",
                RowKey = id,
                UserName = user.Name,
                DisplayName = user.Name,
                Email = user.Email,
                IdentityProvider = user.IdentityProvider,
            };

            _userRepository.AddOrUpdateEntity(newUser);
        }

        public void RegisterNewUser(RegisterModel user, string id)
        {
            var newUser = new User()
            {
                PartitionKey = "ana",
                RowKey = id,
                UserName = user.UserName,
                DisplayName = user.UserName,
                Email = user.Email,
                IdentityProvider = "ana",
            };

            _userRepository.AddOrUpdateEntity(newUser);
        }




        public void UpdateMyUserProfile(UserModel user)
        {
            var userToUpdate = _userRepository.Query
                .Where(u => u.PartitionKey == "ana" && u.RowKey == user.id).FirstOrDefault();

            userToUpdate.DisplayName = user.name;
            userToUpdate.Email = user.email;

            _userRepository.AddOrUpdateEntity(userToUpdate);
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            var users = _userRepository.Query
                .Where(u => u.PartitionKey == "ana")
                .ToList();

            return users.Select(u => new UserModel()
            {
                id = u.RowKey,
                name = u.UserName,
                email = u.Email,
                identity_provider = u.IdentityProvider
            });
        }


        public void SetCurrentUser(UserModel user)
        {
            var curr = _userRepository
                        .Query
                        .Where(u => u.PartitionKey == "ana" && u.RowKey == user.id)
                        .FirstOrDefault();

            base.SetCurrentUser(curr);
        }


    }
}
