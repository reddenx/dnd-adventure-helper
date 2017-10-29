using SMT.Utilities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class Configuration : ConfigurationBase
    {
        [AppSettings("JournalFilePath")]
        public readonly string JournalFilePath;

        [AppSettings("LinkFilePath")]
        public readonly string LinkFilePath;
    }
}