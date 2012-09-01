using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ana.Web.Models
{
    public class BoardModel
    {
        public string id { get; set; }

        
        public string name { get; set; }
        public string url_name { get; set; }
        public string description { get; set; }

        public string created_by { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime created_at { get; set; }

        public string updated_by { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime updated_at { get; set; }


        public IEnumerable<CardModel> cards { get; set; }

        public IEnumerable<BoardResumeModel> boards { get; set; }
        public IEnumerable<BoardResumeModel> shared_boards { get; set; }

        public bool is_shared { get; set; }
    }

    public class ShareBoardModel
    {
        public string id { get; set; }
        public string url_name { get; set; }
        public string name { get; set; }

        public IEnumerable<UserModel> shared_users { get; set; }
    }

    public class BoardResumeModel
    {
        public string id { get; set; }
        public string url_name { get; set; }
        public string name { get; set; }
        public string user_name { get; set; }
    }

    public class BoardCreateOrEditModel
    {
        public string id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }
    }

}