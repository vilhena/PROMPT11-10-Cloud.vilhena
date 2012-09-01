using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;
using Microsoft.Samples.Common.Storage;

namespace Ana.Contracts.Repository
{
    public interface IUserRepository : IAzureTable<User>
    {

    }
}
