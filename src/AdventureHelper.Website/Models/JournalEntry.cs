using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class JournalEntry
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }
    }

    public class JournalEntryDto
    {
        public static JournalEntryDto FromEntry(JournalEntry dto)
        {
            return new JournalEntryDto()
        }
    }
}