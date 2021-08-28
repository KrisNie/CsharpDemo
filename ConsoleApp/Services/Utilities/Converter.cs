using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Services.Utilities
{
    public class Converter
    {
        /// <summary>
        /// Convert DataSet to XML
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSetToXml(DataSet ds)
        {
            using var memoryStream = new MemoryStream();
            using TextWriter streamWriter = new StreamWriter(memoryStream);
            var xmlSerializer = new XmlSerializer(typeof(DataSet));
            xmlSerializer.Serialize(streamWriter, ds);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static string ToLiteral(string input)
        {
            using var writer = new StringWriter();
            using var provider = CodeDomProvider.CreateProvider("CSharp");
            provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
            return writer.ToString();
        }

        public static List<string> ConvertDataTableToXMl(DataTable dataTable)
        {
            var xmlList = new List<string>();
            var ds = new DataSet();
            ds.Tables.Add(dataTable.Rows[0].Table.Clone());

            var builder = new StringBuilder();
            var settings = new XmlWriterSettings()
            {
                Indent = false,
                NewLineChars = "",
                ConformanceLevel = ConformanceLevel.Fragment,
                OmitXmlDeclaration = false
            };

            using var xmlWriter = XmlWriter.Create(builder, settings);
            foreach (DataRow row in dataTable.Rows)
            {
                ds.Tables[0].ImportRow(row);
                ds.Tables[0].WriteXml(xmlWriter);

                xmlList.Add(builder.ToString());
                // Clear StringBuilder/DataSet/XmlWriter, enter into a next cycle
                builder.Clear();
                ds.Clear();
            }

            return xmlList;
        }


        public static Dictionary<string,string> SettleFileName(string fileName)
        {
            var fileNameDictionary = new Dictionary<string, string>();
            var fileNameAnalysis = Regex.Split(fileName, @"(\.)|(-)")
                .Where(m => !string.IsNullOrEmpty(m) && !m.Contains("-") && !m.Contains("."))
                .ToList();
            if (fileNameAnalysis.Count == 5)
            {
                fileNameDictionary.Add("TableName",fileNameAnalysis[0]);
                fileNameDictionary.Add("Site",fileNameAnalysis[1]);
                fileNameDictionary.Add("DBLevel",fileNameAnalysis[2]);
                fileNameDictionary.Add("TaskNum",fileNameAnalysis[3]);
                fileNameDictionary.Add("FileType",fileNameAnalysis[4]);
                
                Console.WriteLine($"TableName: {fileNameAnalysis[0]}");
                Console.WriteLine($"Site: {fileNameAnalysis[1]}");
                Console.WriteLine($"DBLevel: {fileNameAnalysis[2]}");
                Console.WriteLine($"TaskNum: {fileNameAnalysis[3]}");
                Console.WriteLine($"FileType: {fileNameAnalysis[4]}");
            }
            
            return fileNameDictionary;
        }

        public static Dictionary<string, object> ConvertXMlToDictionary(string xmlString)
        {
            var xmlElement = XElement.Parse(xmlString);
            var rootElement = xmlElement.Descendants().First();

            return rootElement.Elements()
                .ToDictionary<XElement, string, object>(el => el.Name.LocalName, el => el.Value);
        }
    }

    internal class XmlToCsv
    {
        /// <summary>
        /// Row
        /// </summary>
        public enum RowDelimit
        {
            Default,
            NewLine,
            Space,
            Ambescent,
            Dollar
        }

        /// <summary>
        /// Column
        /// </summary>
        public enum ColumnDelimit
        {
            Default,
            Comma,
            TabSpace,
            Percentage,
            OrSymbol,
            Dot
        }

        /// <summary>
        /// Use for XDocument
        /// </summary>
        public enum DataArrange
        {
            Element,
            Attribute
        }

        /// <summary>
        /// Row
        /// </summary>
        private static RowDelimit RowDelimiter { set; get; }

        /// <summary>
        /// Column
        /// </summary>
        private static ColumnDelimit ColumnDelimiter { set; get; }

        /// <summary>
        /// Get Row Separater
        /// </summary>
        /// <param name="delimit"></param>
        /// <param name="separater"></param>
        private static void FetchRowSeparater(RowDelimit delimit, out string separater)
        {
            switch (delimit)
            {
                case RowDelimit.NewLine:
                case RowDelimit.Default:
                    separater = Environment.NewLine;
                    break;
                case RowDelimit.Space:
                    separater = " ";
                    break;
                case RowDelimit.Dollar:
                    separater = "$";
                    break;
                case RowDelimit.Ambescent:
                    separater = "&";
                    break;
                default:
                    separater = Environment.NewLine;
                    break;
            }
        }

        /// <summary>
        /// Get Column Separater
        /// </summary>
        /// <param name="delimit"></param>
        /// <param name="separater"></param>
        private static void FetchColumnSeparater(ColumnDelimit delimit, out string separater)
        {
            switch (delimit)
            {
                case ColumnDelimit.Comma:
                case ColumnDelimit.Default:
                    separater = ",";
                    break;
                case ColumnDelimit.Dot:
                    separater = ".";
                    break;
                case ColumnDelimit.OrSymbol:
                    separater = "|";
                    break;
                case ColumnDelimit.Percentage:
                    separater = "%";
                    break;
                case ColumnDelimit.TabSpace:
                    separater = "\t";
                    break;
                default:
                    separater = ",";
                    break;
            }
        }

        /// <summary>
        /// Convert XML to SVC from file table by table
        /// </summary>
        /// <param name="xmlfilepath">xml file path</param>
        /// <param name="csvpath">csv path</param>
        /// <param name="datatag">Table Name</param>
        /// <param name="arrange">Element or Attribute</param>
        /// <param name="rdelimit">Row separater</param>
        /// <param name="cdelimit">Column separater</param>
        public static void Convert(string xmlfilepath, string csvpath, string datatag, DataArrange arrange,
            RowDelimit rdelimit, ColumnDelimit cdelimit)
        {
            try
            {
                var builder = new StringBuilder();

                var doc = XDocument.Load(xmlfilepath);
                FetchRowSeparater(rdelimit, out var rowSeparater);

                FetchColumnSeparater(cdelimit, out var columnseparater);

                foreach (var data in doc.Descendants(datatag))
                {
                    if (arrange == DataArrange.Element)
                    {
                        foreach (var xElement in data.Elements())
                        {
                            builder.Append(xElement.Value);
                            builder.Append(columnseparater);
                        }
                    }
                    else
                    {
                        foreach (var xAttribute in data.Attributes())
                        {
                            builder.Append(xAttribute.Value);
                            builder.Append(columnseparater);
                        }
                    }

                    // Remove the last Columnseparater
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(rowSeparater);
                }

                File.AppendAllText(csvpath, builder.ToString());
            }
            catch (Exception exception)
            {
                throw new Exception("Convert fail", exception);
            }
        }

        /// <summary>
        /// Convert XML to SVC row by row
        /// </summary>
        /// <param name="xmlString">XML with only one node</param>
        /// <param name="rdelimit">Row separater</param>
        /// <param name="cdelimit">Column separater</param>
        /// <param name="dbLevel"></param>
        /// <param name="timeStamp"></param>
        private static void Convert(string xmlString, RowDelimit rdelimit, ColumnDelimit cdelimit, string dbLevel,
            string timeStamp)
        {
            try
            {
                #region ReadXML

                var builder = new StringBuilder();
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xmlString);
                FetchRowSeparater(rdelimit, out var rowSeparater);
                FetchColumnSeparater(cdelimit, out var columnSeparater);
                // Set default Header separater to TabSpace.
                FetchColumnSeparater(ColumnDelimit.TabSpace, out var headerSeparater);
                var root = xDoc.FirstChild;

                string tableName;
                if (xDoc.DocumentElement != null)
                {
                    tableName = xDoc.DocumentElement.Name;
                }
                else
                {
                    throw new Exception("Incorrect XML format");
                }

                if (xDoc.FirstChild == null) return;
                var site = xDoc.FirstChild["site_ref"]?.InnerText;

                #endregion

                #region Create folder and CSV file

                var fileFlag = false;
                const string folderName = @"C:\Development";
                var pathString = Path.Combine(folderName, "SubFolder");
                Directory.CreateDirectory(pathString);

                //string fileName = System.IO.Path.GetRandomFileName();
                var fileName = tableName + dbLevel + site + timeStamp;
                pathString = Path.Combine(pathString, fileName);

                // Check that the file doesn't already exist. If it doesn't exist, create.
                if (!File.Exists(pathString))
                {
                    // Create file.
                    using (File.Create(pathString))
                    {
                        // First reading Flag.
                        fileFlag = true;
                    }
                }

                #endregion

                #region Write

                //Display the contents of the child nodes.
                if (root != null && root.HasChildNodes)
                {
                    // Write node name at first reading.
                    if (fileFlag)
                    {
                        for (int i = 0; i < root.ChildNodes.Count; i++)
                        {
                            builder.Append(root.ChildNodes[i]?.Name);
                            builder.Append(headerSeparater);
                        }

                        builder.Remove(builder.Length - 1, 1);
                        builder.Append(rowSeparater);
                    }

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        builder.Append(root.ChildNodes[i]?.InnerText);
                        builder.Append(columnSeparater);
                    }

                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(rowSeparater);
                }

                File.AppendAllText(pathString, builder.ToString());

                #endregion
            }
            catch (Exception exception)
            {
                throw new Exception("Convert fail", exception);
            }
        }

        /// <summary>
        /// Convert XML to SVC By batch
        /// </summary>
        /// <param name="dataTable">Staging table</param>
        /// <param name="arrange"></param>
        /// <param name="rdelimit">Row separater</param>
        /// <param name="cdelimit">Column separater</param>
        public static void Convert(DataTable dataTable, DataArrange arrange, RowDelimit rdelimit,
            ColumnDelimit cdelimit)
        {
            try
            {
                // select SchemaLevel from [dbo].[ProductSchemaInfo]
                var dbLevel = "77";

                foreach (DataRow data in dataTable.Rows)
                {
                    var xmlString = data["row_data"].ToString();
                    long timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    Convert(xmlString, rdelimit, cdelimit, dbLevel, timeStamp.ToString());
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Convert fail", exception);
            }
        }
    }
}