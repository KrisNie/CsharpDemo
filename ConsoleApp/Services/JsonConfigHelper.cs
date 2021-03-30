using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services
{
    public class Config
    {
        public string SqlConnectionString;
    }

    public class JsonConfigHelper
    {
        public Config Config { get; }

        public JsonConfigHelper(string path)
        {
            using var r = new StreamReader(path);
            var json = r.ReadToEnd();
            this.Config = JsonConvert.DeserializeObject<Config>(json);
        }
    }
}