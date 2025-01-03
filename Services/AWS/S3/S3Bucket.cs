using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace Services.AWS.S3
{
    public static class S3Bucket
    {
        private const string S3BucketName = "infor-dde-dev-appdata-us-east-1";
        private const string S3BucketRoot = "erpsl/Software/knie/";

        private static readonly Amazon.RegionEndpoint
            Endpoint = Amazon.RegionEndpoint.GetBySystemName("us-east-1");

        private static readonly Lazy<AmazonS3Client> LazyAmazonS3Client =
            new(() => new AmazonS3Client(Endpoint));

        private static readonly Lazy<TransferUtility> LazyTransferUtility =
            new(() => new TransferUtility(AmazonS3Client));

        private static AmazonS3Client AmazonS3Client => LazyAmazonS3Client.Value;
        private static TransferUtility TransferUtility => LazyTransferUtility.Value;


        /// <summary>
        /// Shows how to list the objects in an Amazon S3 bucket.
        /// </summary>
        /// <returns>A boolean value indicating the success or failure of the
        /// copy operation.</returns>
        public static async Task<List<string>> ListBucketRootContentsAsync()
        {
            try
            {
                var contents = new List<string>();
                var request = new ListObjectsV2Request
                {
                    BucketName = S3BucketName,
                    Prefix = S3BucketRoot,
                    MaxKeys = 5,
                };
                ListObjectsV2Response response;
                do
                {
                    response = await AmazonS3Client.ListObjectsV2Async(request);
                    response.S3Objects
                        .ForEach(
                            x => contents.Add(
                                $"{x.Key,-35}{x.LastModified.ToShortDateString(),10}{x.Size,10}"));

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                return contents;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(
                    $"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                return new List<string>();
            }
        }

        public static async Task<(bool IsSuccess, string Message)> UploadAsync(
            MemoryStream memoryStream,
            string fileName)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                await TransferUtility.UploadAsync(
                    memoryStream,
                    S3BucketName,
                    S3BucketRoot + fileName);

                watch.Stop();
                var elapsed = watch.ElapsedMilliseconds;
                Console.WriteLine($"Task for {fileName} cost {elapsed} ms.");

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"S3Bucket.UploadAsync() ERROR for {fileName}: {ex.Message}.");
            }
        }

        public static async Task<(bool IsSuccess, string Message)> UploadAsync(
            MemoryStream memoryStream,
            string fileName,
            int delayTime)
        {
            try
            {
                using (var cancellationToken = new CancellationTokenSource(delayTime))
                {
                    await TransferUtility.UploadAsync(
                        memoryStream,
                        S3BucketName,
                        S3BucketRoot + fileName,
                        cancellationToken.Token);
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"S3Bucket.UploadAsync() ERROR for {fileName}: {ex.Message}.");
            }
        }
    }
}