using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Business;
using Ana.Domain;
using Ana.Utils;

namespace Ana.Business.Managers
{
    public abstract class BaseManager : IManager
    {
        private IUserProvider _userProvider;

        public BaseManager()
            :this(new HttpContextUserProvider())
        {
        }

        public BaseManager(IUserProvider userProvider)
        {
            if (userProvider == null)
            {
                throw new ArgumentNullException("userProvider");
            }

            this._userProvider = userProvider;
        }

        public User CurrentUser()
        {
            return _userProvider.GetUser();
        }


        public string CurrentUserId()
        {
            return _userProvider.GetUserId();
        }


        public bool HasValidUser()
        {
            return _userProvider.HasUser();
        }

        public string GetNewShortGuid()
        {
            var sguid = new ShortGuid(Guid.NewGuid());
            return sguid.Value;
        }


        public void SetCurrentUser(User user)
        {
            _userProvider.SetUser(user);
        }
    }
}
