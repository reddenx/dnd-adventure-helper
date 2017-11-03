using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models.Documents
{
    public class Document : IIdentifiable
    {
        public Guid Id { get; set; }

        public Guid OwnerId;

        public string Title;
        public string Body;
        public string DocumentType;

        public DateTime DateCreated;
        public DateTime DateLastModified;

        public Dictionary<string, string> MetaData;

        public Document(Guid id, Guid ownerId, string title, string body, string documentType, DateTime dateCreated, DateTime dateLastModified, Dictionary<string, string> metaData)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.Title = title;
            this.Body = body;
            this.DocumentType = documentType;
            this.DateCreated = dateCreated;
            this.DateLastModified = dateLastModified;
            this.MetaData = metaData;
        }
    }
}