using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Newtonsoft.Json;

namespace Services.Utilities
{
    public record Modules
    {
        public string irn { get; init; }
        public string uniqueName { get; init; }
        public string displayName { get; init; }
        public int displaySequence { get; init; }
        public List<Configuration> configurations { get; set; }
    }

    public record Configuration
    {
        public string irn { get; init; }
        public string uniqueName { get; init; }
        public string displayName { get; init; }
        public int displaySequence { get; init; }
    }

    public static class Excel2Json
    {
        public static string ConvertExcel2Json()
        {
            var pathToExcel =
                @"C:\Users\knie\OneDrive - Infor\Documents\Infor\Tasks\T2V\Feature Jira ID CSIB-61292\Modules.xlsx";
            var sheets = new List<string>
            {
                "Financial Management",
                "Human Resource Management",
                "Production Control",
                "Projects and Resource Planning",
                "Sales and order Management",
                "Services",
                "Supply Chain Management"
            };

            var connectionString = $@"
            Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source={pathToExcel};
            Extended Properties=""Excel 12.0;HDR=No;IMEX=1;"" ";

            using var conn = new OleDbConnection(connectionString);
            conn.Open();

            var modules = new List<Modules>();
            var moduleDisplaySequence = 0;

            foreach (var sheet in sheets)
            {
                var configurations = new List<Configuration>();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $@"SELECT * FROM [{sheet}$]";

                using var reader = cmd.ExecuteReader();

                var configurationDisplaySequence = -4;
                while (reader.Read())
                {
                    var configuration = new Configuration
                    {
                        irn = reader.GetValue(1).ToString(),
                        uniqueName = reader.GetValue(2).ToString(),
                        displayName = reader.GetValue(3).ToString(),
                        displaySequence = configurationDisplaySequence++
                    };

                    configurations.Add(configuration);
                }

                configurations.RemoveAt(3);
                configurations.RemoveAt(1);
                configurations.RemoveAt(0);
                var module = new Modules
                {
                    irn = configurations[0].irn,
                    uniqueName = configurations[0].uniqueName,
                    displayName = configurations[0].displayName,
                    displaySequence = moduleDisplaySequence++
                };
                configurations.RemoveAt(0);
                module.configurations = configurations;
                modules.Add(module);
            }

            Console.WriteLine(JsonConvert.SerializeObject(modules));

            return string.Empty;
        }
    }
}