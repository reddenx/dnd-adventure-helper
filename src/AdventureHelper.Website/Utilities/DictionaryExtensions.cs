using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website
{
    public static class DictionaryExtensions
    {
        public static void UpdateMergeWith(this Dictionary<string, string> data, Dictionary<string, string> newData)
        {
            foreach (var entry in newData)
            {
                data[entry.Key] = entry.Value;
            }
        }
    }
}