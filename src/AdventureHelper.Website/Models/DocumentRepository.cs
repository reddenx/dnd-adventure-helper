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


        //public Document[] GetAllDocuments()
        //{
        //    if (File.Exists(FilePath))
        //    {
        //        var rawData = File.ReadAllText(FilePath);
        //        var data = JsonConvert.DeserializeObject<Document[]>(rawData);
        //        return data;
        //    }
        //    else
        //    {
        //        return new Document[0];
        //    }
        //}

        //public void UpdateDocuments(Document[] documents)
        //{
        //    var rawData = JsonConvert.SerializeObject(documents);
        //    File.WriteAllText(FilePath, rawData);
        //}

        //public void UpdateDocument(DocumentDto document)
        //{
        //    var documents = GetAllDocuments();
        //    var foundDocument = documents.FirstOrDefault(d => d.Id == document.Id);

        //    var updatedDocument = new Document
        //    {
        //        Body = document.Body,
        //        Id = foundDocument?.Id ?? Guid.NewGuid(),
        //        MetaData = MergeMetaData(foundDocument?.MetaData, document.MetaData),
        //        Name = document.Name
        //    };

        //    if (!updatedDocument.MetaData.ContainsKey("date-created"))
        //    {
        //        updatedDocument.MetaData["date-created"] = DateTime.Now.ToString("O");
        //    }

        //    updatedDocument.MetaData["date-last-updated"] = DateTime.Now.ToString("O");

        //    var replacedDocuments = documents
        //        .Where(d => d.Id != document.Id)
        //        .Concat(new[] { updatedDocument })
        //        .ToArray();

        //    UpdateDocuments(replacedDocuments);
        //}

        private Dictionary<string, string> MergeMetaData(Dictionary<string, string> oldData, Dictionary<string, string> newData)
        {
            var newStuff = oldData ?? new Dictionary<string, string>();

            foreach (var entry in newData)
            {
                newStuff[entry.Key] = entry.Value;
            }

            return newStuff;
        }
    }
}