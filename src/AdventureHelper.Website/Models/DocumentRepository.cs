using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class DocumentRepository
    {
        private readonly string FilePath;
        public DocumentRepository(string filePath)
        {
            FilePath = filePath;
        }

        public Document[] GetAllDocuments()
        {
            if (File.Exists(FilePath))
            {
                var rawData = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<Document[]>(rawData);
                return data;
            }
            else
            {
                return new Document[0];
            }
        }

        public void UpdateDocuments(Document[] documents)
        {
            var rawData = JsonConvert.SerializeObject(documents);
            File.WriteAllText(FilePath, rawData);
        }

        public void UpdateDocument(Document document)
        {
            var documents = GetAllDocuments();
            if(!document.MetaData.ContainsKey("date-created"))
            {
                document.MetaData["date-created"] = DateTime.Now.ToString("O");
            }

            document.MetaData["date-last-updated"] = DateTime.Now.ToString("O");

            var replacedDocuments = documents
                .Where(d => d.Name != document.Name)
                .Concat(new[] { document })
                .ToArray();

            UpdateDocuments(replacedDocuments);
        }
    }
}