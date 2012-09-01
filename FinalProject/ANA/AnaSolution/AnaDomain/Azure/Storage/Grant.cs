using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ana.Domain
{
    public class Grant: TableServiceEntity
    {
        //partitionkey token, row key is token

        public string user { get; set; }
        public string client_id { get; set; }
        public string code { get; set; }
        public DateTime created { get; set; }
        public string token { get; set; }
        public DateTime expires { get; set; }
    }
}
