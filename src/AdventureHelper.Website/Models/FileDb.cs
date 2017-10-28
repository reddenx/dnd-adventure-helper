using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public static class FileDb
    {
        public static T[] Get<T>(string filepath)
        {
            if (File.Exists(filepath))
            {
                var rawData = File.ReadAllText(filepath);
                var data = JsonConvert.DeserializeObject<T[]>(rawData);
                return data;
            }
            else
            {
                return new T[] { };
            }
        }

        public static void Save(string filepath, object[] data)
        {
            var rawData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filepath, rawData);
        }
    }
}