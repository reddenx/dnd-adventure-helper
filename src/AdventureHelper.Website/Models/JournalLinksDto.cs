using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class JournalLinksDto : IIdentifiable
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
    }
}