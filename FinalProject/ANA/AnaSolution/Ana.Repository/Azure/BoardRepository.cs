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
    public class BoardRepository : AzureTable<Board>, IBoardRepository
    {
        public BoardRepository(CloudStorageAccount account)
            : base(account)
        {
        }

    }
}
