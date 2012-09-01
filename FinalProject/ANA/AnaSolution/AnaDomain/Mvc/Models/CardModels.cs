using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ana.Web.Models
{
    public class CardModel
    {
        public string id { get; set; }
        public string board_id { get; set; }

        public string name { get; set; }
        public string url_name { get; set; }
        public string text { get; set; }
        public bool is_done { get; set; }


        public string created_by { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime created_at { get; set; }
        public string updated_by { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime updated_at { get; set; }
    }

    public class CardResumeModel
    {
        public string id { get; set; }
        public string board_id { get; set; }

        public string name { get; set; }
        public string url_name { get; set; }
    }

    public class CardCreateOrEditModel
    {
        public string id { get; set; }
        public string board_id { get; set; }

        [Required]
        public string name { get; set; }
        public string text { get; set; }
    }
}