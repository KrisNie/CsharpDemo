using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services.Finance;
using Services.Utilities;

namespace Services
{
    public static class UnbelievableClass
    {
        public static void UnbelievableMethod()
        {
            PI.Pi();
        }

        private static void TestForDependencyInjection()
        {
            var ba = new CompositionRoot().GetService<IBankAccount>();
            if (ba == null) return;
            ba.Create("Mr. Bryan Walton", 11.99);
            ba.Credit(5.77);
            ba.Debit(11.22);
            Console.WriteLine("Current balance is ${0}", ba.Balance);
        }

        /// <summary>
        /// ReadCsvAndConvert2Xml
        /// </summary>
        private static void ReadCsvAndConvert2Xml()
        {
            var listXml = File.ReadLines(@"C:\Development\SubFolder\item_mst_DALS_77.csv").ToList();
            var stringXml = JsonConvert.SerializeObject(listXml.Take(100));
            stringXml = JsonConvert.ToString(stringXml);
            ClipboardOperator.CopyToClipboard(stringXml);
        }

        /// <summary>
        /// Connect to database demo
        /// </summary>
        /// <returns></returns>
        private static DataSet GetDataSet()
        {
            var ds = new DataSet();
            try

            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = "LOCALHOST", UserID = "sa", Password = "123456Zz",
                    InitialCatalog = "CSI10"
                };

                using var connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                var sql = "SELECT TOP 10 * FROM item_mst;";

                using var dataAdapter = new SqlDataAdapter(sql, connection);
                dataAdapter.Fill(ds);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return ds;
        }
    }
}