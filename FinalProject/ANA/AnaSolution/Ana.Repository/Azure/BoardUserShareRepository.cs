using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Repository;
using Ana.Domain;
using Microsoft.Samples.Common.Storage;
using Microsoft.WindowsAzure;
using Ana.Domain.Azure.Storage;

namespace Ana.Repository.Azure
{
   
    public class BoardUserShareRepository : AzureTable<BoardUserShare>, IBoardUserShareRepository
    {
        public BoardUserShareRepository(CloudStorageAccount account)
            : base(account)
        {
        }

    }
}
