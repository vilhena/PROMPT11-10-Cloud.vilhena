using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Samples.Common.Storage;
using Ana.Domain;

namespace Ana.Contracts.Repository
{
    public interface ICardRepository : IAzureTable<Card>
    {

    }
}
