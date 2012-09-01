using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Domain
{
    public class Developer : TableServiceEntity
    {
        //username is partitionkey, id is guid

        public bool enabled { get; set; }
        public string application_name { get; set; }
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
        public string secret { get; set; }
    }
}
