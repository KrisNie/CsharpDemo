using System.IO;
using Newtonsoft.Json;

namespace Services.Utilities
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
            Config = JsonConvert.DeserializeObject<Config>(json);
        }
    }
}