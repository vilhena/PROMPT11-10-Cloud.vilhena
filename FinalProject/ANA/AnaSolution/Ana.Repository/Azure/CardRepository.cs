using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Repository;
using Microsoft.WindowsAzure;
using Microsoft.Samples.Common.Storage;
using Ana.Domain;

namespace Ana.Repository.Azure
{
    public class CardRepository : AzureTable<Card>,  ICardRepository
    {
        public CardRepository(CloudStorageAccount account)
            : base(account)
        {
        }
    }
}
