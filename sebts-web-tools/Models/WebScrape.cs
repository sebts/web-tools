using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace sebts_web_tools.Models
{
    [DataContract]
    public class WebScrape
    {
        [DataMember(Name = "html", IsRequired = true)]
        public string Html { get; set; }
    }
}