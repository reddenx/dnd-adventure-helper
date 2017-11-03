using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class JournalLinksDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }

        public bool Shared { get; set; }
        public Guid OwnerId { get; set; }

        public JournalLinksDto(Guid? id, string name, string type, string body, bool shared, Guid ownerId)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Body = body;
            this.Shared = shared;
            this.OwnerId = OwnerId;
        }
    }
}