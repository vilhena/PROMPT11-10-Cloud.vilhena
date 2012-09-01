using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Domain
{
    public class User : TableServiceEntity
    {
        //uses "ana" application name as partitionkey, and uses userguid as id

        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public string IdentityProvider { get; set; }
    }
}
