using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.Samples.Common.Storage;

namespace Ana.Domain
{
    public class UserNotification : AzureQueueMessage
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
