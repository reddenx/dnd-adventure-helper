using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }

    public class SimpleFileBank<T> where T : IIdentifiable
    {
        private readonly string Filepath;
        public SimpleFileBank(string filepath)
        {
            Filepath = filepath;
        }

        public T[] Get()
        {
            if (File.Exists(Filepath))
            {
                var rawData = File.ReadAllText(Filepath);
                var data = JsonConvert.DeserializeObject<T[]>(rawData);
                return data;
            }
            else
            {
                return new T[] { };
            }
        }

        public void Save(T data)
        {
            var updatedEntryList = Get().Where(e => e.Id != data.Id).Concat(new[] { data }).ToArray();

            var rawData = JsonConvert.SerializeObject(updatedEntryList);
            File.WriteAllText(Filepath, rawData);
        }
    }
}