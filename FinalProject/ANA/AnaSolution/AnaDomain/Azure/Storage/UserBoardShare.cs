using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Domain.Azure.Storage
{
    public class UserBoardShare : TableServiceEntity
    {
        //row key is boardId, partitonkey is userId
        public string UserName { get; set; }
        public string BoardName { get; set; }
        public string BoardUrlName { get; set; }
    }
}
