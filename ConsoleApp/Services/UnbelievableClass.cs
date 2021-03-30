using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace Services
{
    public class UnbelievableClass
    {
        public static void UnbelievableMethod()
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var dataSet = DataBaseHelper.Query("SELECT TOP 10 * FROM item_mst;");
            var dataTable = dataSet.Tables[0];
            // var xmlList = dataTable.AsEnumerable().Select(x => x["row_data"]?.ToString()).ToList();

            var retOblRequest = (HttpWebRequest) WebRequest.Create("https://docs.microsoft.com");
            retOblRequest.Method = "GET";
            var accessToken = "";
            retOblRequest.Headers.Add($"Authorization: Bearer {accessToken}");
            retOblRequest.ContentType = "application/json";
            retOblRequest.Accept = "application/vnd.hmrc.1.0+json";
            GenerateFraudHeaders(retOblRequest);


            var tokenResponse = retOblRequest.GetResponse();
        }


        private static void GenerateFraudHeaders(HttpWebRequest webRequest)
        {
            var method = "1";
            webRequest.Headers.Add($"Gov-Client-Connection-Method: {method}");
        }


        /// <summary>
        /// ReadCsvAndConvert2Xml
        /// </summary>
        private void ReadCsvAndConvert2Xml()
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
        private DataSet GetDataSet()
        {
            var ds = new DataSet();
            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = "LOCALHOST", UserID = "sa", Password = "123456Zz", InitialCatalog = "CSI10"
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


        private void WebRequestGet(string url)
        {
            // Create a request for the URL.
            var request = WebRequest.Create(
                "https://docs.microsoft.com");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            var response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse) response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (var dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                var reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
            }

            // Close the response.
            response.Close();
        }
    }
}