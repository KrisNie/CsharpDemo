using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services.Finance;
using Services.Utilities;

namespace Services
{
    public class UnbelievableClass
    {
        private static readonly Random Random = new Random();

        public static void UnbelievableMethod()
        {
            var aa = DateTime.TryParseExact("",
                new[] {"yyyy-MM-ddTHH:mm:ss.ffK"},
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out var dateTime)
                ? dateTime
                : (object) "";

            var fileNameList = new List<string> {"u_m-DALS-86-1.csv", "u_m-DALS-86-2.csv", "xxx.txt"};

            var a = fileNameList.AsEnumerable().Select(Converter.SettleFileName).Where(fileInfo => fileInfo.Count > 0)
                .ToList();

            // var xmlList = Converter.ConvertDataTableToXMl(new UnbelievableClass().GetDataSet().Tables[0]);
            // Console.WriteLine(xmlList.First());
            //
            //
            // var dictionary = Converter.ConvertXMlToDictionary(xmlList.First());
            //
            // foreach (var (key, value) in dictionary)
            // {
            //     Console.WriteLine($"Key = {key}, Value = {value}");
            // }
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
        /// EightLeggedEssayGenerator
        /// </summary>
        /// <returns></returns>
        public static string EightLeggedEssayGenerator()
        {
            var verbList =
                @"皮实、复盘、赋能、加持、沉淀、倒逼、落地、串联、协同、反哺、兼容、包装、重组、履约、响应、量化、发力、布局、联动、细分、梳理、输出、加速、共建、共创、支撑、融合、解耦、聚合、集成、对标、对齐、聚焦、抓手、拆解、拉通、抽象、摸索、提炼、打通、吃透、迁移、分发、分层、封装、辐射、围绕、复用、渗透、扩展、开拓、给到、死磕、破圈"
                    .Split("、");
            var twoWordsNounList =
                @"漏斗、中台、闭环、打法、纽带、矩阵、刺激、规模、场景、维度、格局、形态、生态、话术、体系、认知、玩法、体感、感知、调性、心智、战役、合力、赛道、基因、因子、模型、载体、横向、通道、补位、链路、试点"
                    .Split("、");
            var threeWordsNounList =
                "新生态、感知度、颗粒度、方法论、组合拳、引爆点、点线面、精细化、差异化、平台化、结构化、影响力、耦合性、易用性、便捷性、一致性、端到端、短平快、护城河".Split("、");
            var fourWordsNounList =
                "底层逻辑、顶层设计、交付价值、生命周期、价值转化、强化认知、资源倾斜、完善逻辑、抽离透传、复用打法、商业模式、快速响应、定性定量、关键路径、去中心化、结果导向、垂直领域、归因分析、体验度量、信息屏障"
                    .Split("、");
            var verbListLength = verbList.Length;
            var twoWordsNounListLength = twoWordsNounList.Length;
            var threeWordsNounListLength = threeWordsNounList.Length;
            var fourWordsNounListLength = fourWordsNounList.Length;

            // TODO: It can be refactoring
            var eightLeggedEssay =
                $@"{fourWordsNounList[Random.Next(fourWordsNounListLength)]}是{verbList[Random.Next(verbListLength)]}{
                    fourWordsNounList[Random.Next(fourWordsNounListLength)]}，{verbList[Random.Next(verbListLength)]}行业{
                        threeWordsNounList[Random.Next(threeWordsNounListLength)]}。{
                            fourWordsNounList[Random.Next(fourWordsNounListLength)]}是{
                                verbList[Random.Next(verbListLength)]}{
                                    twoWordsNounList[Random.Next(twoWordsNounListLength)]
                                }{fourWordsNounList[Random.Next(fourWordsNounListLength)]}，通过{
                                    threeWordsNounList[Random.Next(threeWordsNounListLength)]}和{
                                        threeWordsNounList[Random.Next(threeWordsNounListLength)]}达到{
                                            threeWordsNounList[Random.Next(threeWordsNounListLength)]}。{
                                                fourWordsNounList[Random.Next(fourWordsNounListLength)]}是在{
                                                    fourWordsNounList[Random.Next(fourWordsNounListLength)]}采用{
                                                        twoWordsNounList[Random.Next(twoWordsNounListLength)]}打法达成{
                                                            fourWordsNounList[Random.Next(fourWordsNounListLength)]}。{
                                                                fourWordsNounList[Random.Next(fourWordsNounListLength)]
                                                            }{
                                                                fourWordsNounList[Random.Next(fourWordsNounListLength)]
                                                            }作为{
                                                                twoWordsNounList[Random.Next(twoWordsNounListLength)]
                                                            }为产品赋能，{
                                                                fourWordsNounList[Random.Next(fourWordsNounListLength)]
                                                            }作为{
                                                                twoWordsNounList[Random.Next(twoWordsNounListLength)]
                                                            }的评判标准。亮点是{
                                                                twoWordsNounList[Random.Next(twoWordsNounListLength)]
                                                            }，优势是{
                                                                twoWordsNounList[Random.Next(twoWordsNounListLength)]}。{
                                                                    verbList[Random.Next(verbListLength)]
                                                                }整个{fourWordsNounList[
                                                                    Random.Next(fourWordsNounListLength)]}，{
                                                                    verbList[Random.Next(verbListLength)]}{
                                                                        twoWordsNounList[
                                                                            Random.Next(twoWordsNounListLength)]}{
                                                                            verbList[Random.Next(verbListLength)]}{
                                                                                fourWordsNounList[
                                                                                    Random.Next(
                                                                                        fourWordsNounListLength)]
                                                                            }。{threeWordsNounList[
                                                                                Random.Next(threeWordsNounListLength)]
                                                                            }是{
                                                                                threeWordsNounList[
                                                                                    Random.Next(
                                                                                        threeWordsNounListLength)]}达到{
                                                                                        threeWordsNounList[
                                                                                            Random.Next(
                                                                                                threeWordsNounListLength)]
                                                                                    }标准。";

            return eightLeggedEssay;
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
    }
}