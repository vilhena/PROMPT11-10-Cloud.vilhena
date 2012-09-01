using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;

namespace Ana.Contracts.Business
{
    public interface IUserProvider
    {
        User GetUser();
        string GetUserId();
        bool HasUser();
        void SetUser(User user);
    }
}
