using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Repository;
using Ana.Domain;
using Microsoft.Samples.Common.Storage;
using Microsoft.WindowsAzure;

namespace Ana.Repository.Azure
{
    public class UserNotificationRepository : AzureQueue<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(CloudStorageAccount account)
            : base(account)
        {
        }
    }
}
