using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;

namespace Ana.Contracts.Business
{
    public interface IManager
    {
        User CurrentUser();
        string CurrentUserId();
        bool HasValidUser();
        void SetCurrentUser(User user);
    }
}
