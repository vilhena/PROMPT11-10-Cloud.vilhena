using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Domain.Azure.Storage
{
    public class BoardUserShare : TableServiceEntity
    {
        //partitionkey is boardId, rowkey is userId

        public string UserName { get; set; }
    }
}
