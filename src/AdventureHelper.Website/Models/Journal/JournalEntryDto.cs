using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class JournalEntryDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }

        public JournalEntryDto() { }

        public JournalEntryDto(Guid? id, string name, string body)
        {
            this.Id = id;
            this.Name = name;
            this.Body = body;
        }
    }
}