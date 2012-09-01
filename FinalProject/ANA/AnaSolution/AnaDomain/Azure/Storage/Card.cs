using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Domain
{
    public class Card : TableServiceEntity
    {
        //partition key is BoarId
        //row key is guid

        public string Name { get; set; }
        public string UrlName { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
