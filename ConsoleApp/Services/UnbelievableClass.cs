using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Collections;
using Microsoft.Extensions.DependencyInjection;
using Services.Finance;

namespace Services
{
    public static class UnbelievableClass
    {
        public static void UnbelievableMethod()
        {
            var i = "AOE".IndexOf("A", 0, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine(i);
        }

        #region Dependency Injection Demo

        private static void DependencyInjectionDemo()
        {
            var ba = new CompositionRoot().GetService<IBankAccount>();
            if (ba == null) return;
            ba.Create("Mr. Bryan Walton", 11.99);
            ba.Credit(5.77);
            ba.Debit(11.22);
            Console.WriteLine("Current balance is ${0}", ba.Balance);
        }

        #endregion

        #region CircularBuffer Demo

        private static void CircularBufferDemo()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2 });
            Console.WriteLine("Initial buffer {0,1,2}:");
            PrintBuffer(buffer);

            for (int i = 3; i < 7; i++)
            {
                buffer.PushBack(i);
            }

            Console.WriteLine("\nAfter adding a 7 elements to a 5 elements capacity buffer:");
            PrintBuffer(buffer);

            buffer.PopFront();
            Console.WriteLine("\nbuffer.PopFront():");
            PrintBuffer(buffer);

            buffer.PopBack();
            Console.WriteLine("\nbuffer.PopBack():");
            PrintBuffer(buffer);

            for (int i = 2; i >= 0; i--)
            {
                buffer.PushFront(i);
            }

            Console.WriteLine("\nbuffer.PushFront() {2,1,0} respectively:");
            PrintBuffer(buffer);

            buffer.Clear();
            Console.WriteLine("\nbuffer.Clear():");
            PrintBuffer(buffer);
        }

        private static void PrintBuffer(CircularBuffer<int> buffer)
        {
            Console.WriteLine($"{{{string.Join(",", buffer.ToArray())}}}");
        }

        #endregion

        #region Asynchronous Programming Demo

        private static async Task<bool[]> GetAllSite()
        {
            var uris = new List<string>
            {
                "https://www.google.com/",
                "https://github.com/KrisNie",
                "https://developer.mozilla.org/en-US/",
                "https://leetcode.cn/"
            };

            var tasks = uris.Select(HttpGetAsync).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            foreach (var result in results) Console.WriteLine(result);
            return results;
        }

        private static async Task<bool> HttpGetAsync(string requestUri)
        {
            try
            {
                var client = new HttpClient();
                Console.WriteLine(requestUri);
                using var cancellationToken = new CancellationTokenSource(5000);
                using var response = await client.GetAsync(requestUri, cancellationToken.Token)
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                // var responseBody =
                //     await response.Content.ReadAsStringAsync(cancellationToken.Token)
                //         .ConfigureAwait(false);
                // Console.WriteLine(responseBody);
                Console.WriteLine("OK");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static void Upload()
        {
            AWS.S3.S3Bucket.ListBucketRootContentsAsync().GetAwaiter().GetResult();

            var files = new Dictionary<string, byte[]>
            {
                {
                    "L3-Apr22_2022_2_9.txt",
                    File.ReadAllBytes("C:\\Development\\installs\\L3-Apr22_2022_2_9.txt")
                },
                {
                    "L3-Dec22_2022_9_20 1.txt",
                    File.ReadAllBytes(
                        "C:\\Development\\installs\\L3-Dec22_2022_9_20 1.txt")
                }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var results = UploadAsync(files).GetAwaiter().GetResult();
            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"All tasks cost {elapsed} ms");

            if (results.Count > 0)
            {
                var message = string.Join(
                    " ",
                    results);
                Console.WriteLine($"ExportTenantData.Run(): Post files failed. {message}");
            }
        }

        private static async Task<HashSet<string>> UploadAsync(
            Dictionary<string, byte[]> files)
        {
            var tasks = (from key in files.Keys
                         let memoryStream =
                             new MemoryStream(files[key])
                         select AWS.S3.S3Bucket.UploadAsync(memoryStream, key))
                .ToList();

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.Where(x => x.IsSuccess == false).Select(f => f.Message).ToHashSet();
        }

        #endregion
    }
}