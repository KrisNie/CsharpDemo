using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace Services
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
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, ds);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
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
                StringBuilder builder = new StringBuilder();

                XDocument doc = XDocument.Load(xmlfilepath);
                string Rowseparater = string.Empty;
                FetchRowSeparater(rdelimit, out Rowseparater);

                string Columnseparater = string.Empty;
                FetchColumnSeparater(cdelimit, out Columnseparater);

                foreach (XElement data in doc.Descendants(datatag))
                {
                    if (arrange == DataArrange.Element)
                    {
                        foreach (XElement innnerval in data.Elements())
                        {
                            builder.Append(innnerval.Value);
                            builder.Append(Columnseparater);
                        }
                    }
                    else
                    {
                        foreach (XAttribute innerval in data.Attributes())
                        {
                            builder.Append(innerval.Value);
                            builder.Append(Columnseparater);
                        }
                    }

                    // Remove the last Columnseparater
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(Rowseparater);
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
        public static void Convert(string xmlString, RowDelimit rdelimit, ColumnDelimit cdelimit, string dbLevel,
            string timeStamp)
        {
            try
            {
                #region ReadXML

                StringBuilder builder = new StringBuilder();
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xmlString);
                string Rowseparater = string.Empty;
                FetchRowSeparater(rdelimit, out Rowseparater);
                string Columnseparater = string.Empty;
                FetchColumnSeparater(cdelimit, out Columnseparater);
                // Set default Header separater to TabSpace.
                string Headerseparater = string.Empty;
                FetchColumnSeparater(ColumnDelimit.TabSpace, out Headerseparater);
                XmlNode root = xDoc.FirstChild;

                string tableName = string.Empty;
                if (xDoc.DocumentElement != null)
                {
                    tableName = xDoc.DocumentElement.Name;
                }
                else
                {
                    throw new Exception("Incorrect XML format");
                }

                string site = xDoc.FirstChild["site_ref"]?.InnerText;

                #endregion

                #region Create folder and CSV file

                bool fileFlag = false;
                string folderName = @"C:\Development";
                string pathString = System.IO.Path.Combine(folderName, "SubFolder");
                System.IO.Directory.CreateDirectory(pathString);

                //string fileName = System.IO.Path.GetRandomFileName();
                string fileName = tableName + dbLevel + site + timeStamp;
                pathString = System.IO.Path.Combine(pathString, fileName);

                // Check that the file doesn't already exist. If it doesn't exist, create.
                if (!System.IO.File.Exists(pathString))
                {
                    // Create file.
                    using (System.IO.File.Create(pathString))
                    {
                        // First reading Flag.
                        fileFlag = true;
                    }
                }

                #endregion

                #region Write

                //Display the contents of the child nodes.
                if (root.HasChildNodes)
                {
                    // Write node name at first reading.
                    if (fileFlag)
                    {
                        for (int i = 0; i < root.ChildNodes.Count; i++)
                        {
                            builder.Append(root.ChildNodes[i].Name);
                            builder.Append(Headerseparater);
                        }

                        builder.Remove(builder.Length - 1, 1);
                        builder.Append(Rowseparater);
                    }

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        builder.Append(root.ChildNodes[i].InnerText);
                        builder.Append(Columnseparater);
                    }

                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(Rowseparater);
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
        /// <param name="rdelimit">Row separater</param>
        /// <param name="cdelimit">Column separater</param>
        public static void Convert(DataTable dataTable, DataArrange arrange, RowDelimit rdelimit,
            ColumnDelimit cdelimit)
        {
            try
            {
                string dbLevel = string.Empty;

                // select SchemaLevel from [dbo].[ProductSchemaInfo]
                dbLevel = "77";

                foreach (DataRow data in dataTable.Rows)
                {
                    string xmlString = string.Empty;
                    xmlString = data["row_data"].ToString();
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

    internal class DatatableToXml
    {
        public static string Convert(DataTable dataTable)
        {
            // Load Table Data

            #region Load DataTable

            #endregion

            string originalXmlString = string.Empty;
            DataSet ds = new DataSet();
            ds.Tables.Add(dataTable.Rows[0].Table.Clone());
            ds.Tables[0].ImportRow(dataTable.Rows[0]);

            using (StringWriter sw = new StringWriter())
            {
                ds.Tables[0].WriteXml(sw);
                originalXmlString = sw.ToString();
            }

            return originalXmlString;
        }
    }
}