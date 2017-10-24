using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public static class DocumentMapper
    {
        public static DocumentDto ToDto(this Document doc)
        {
            return new DocumentDto()
            {
                Body = doc.Body,
                MetaData = doc.MetaData,
                Name = doc.Name
            };
        }
    }
}