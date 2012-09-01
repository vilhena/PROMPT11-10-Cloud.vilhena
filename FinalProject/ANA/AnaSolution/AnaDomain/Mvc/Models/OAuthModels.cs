using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ana.Web.Models
{
    public class OAuthApproveModel
    {
        [Display(Name = "Client Id")]
        public string client_id { get; set; }

        [Display(Name = "Redirect uri")]
        public string redirect_uri { get; set; }
    }

    public class OAuthModel
    {
        public string user_name { get; set; }

        [Display(Name = "Id")]
        public string id { get; set; }

        public bool enabled { get; set; }

        [Display(Name = "Application Name")]
        public string application_name { get; set; }

        [Display(Name = "Client Id")]
        public string client_id { get; set; }

        [Display(Name = "Redirect uri")]
        public string redirect_uri { get; set; }

        [Display(Name = "Secret")]
        public string secret { get; set; }
    }

    public class GrantModel
    {
        public string user { get; set; }
        public string client_id { get; set; }
        public string code { get; set; }
        public DateTime created { get; set; }
        public string token { get; set; }
        public DateTime expires { get; set; }
    }
}
