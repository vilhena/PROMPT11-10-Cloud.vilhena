using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Repository.Entities.Azure
{
    public class BoardAzureModel: TableServiceEntity
    {
        
        public BoardAzureModel()
        {
            
        }

        public BoardAzureModel(string partkey, string rowkey)
            :base(partkey,rowkey)
        {
            
        }

        public string UserName { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
