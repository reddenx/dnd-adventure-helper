using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class DocumentDto
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
    }
}