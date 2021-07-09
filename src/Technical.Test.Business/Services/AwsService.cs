using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;

namespace Technical.Test.Business.Services
{
    public class AwsService : IAwsService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonSQS _sqsClient;

        private const string S3_BUCKET = "bucket-technical-test-seventh";
        private const string SQS_QUEUE_URL = "https://sqs.sa-east-1.amazonaws.com/351694744067/delete_binary";

        public AwsService(IAmazonS3 s3Client,
            IAmazonSQS sqsClient)
        {
            _s3Client = s3Client;
            _sqsClient = sqsClient;
        }

        public async Task S3UploadBinary(string videoId, byte[] bytes)
        {
            var transferUtility = new TransferUtility(_s3Client);

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                var transferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = S3_BUCKET,
                    InputStream = memoryStream,
                    Key = videoId,
                    CannedACL = S3CannedACL.AuthenticatedRead
                };

                await transferUtility.UploadAsync(transferUtilityRequest);
            }
        }

        public async Task<string> S3DownloadBinary(string videoId)
        {
            string base64Content = string.Empty;

            GetObjectRequest request = new GetObjectRequest 
            {
                BucketName = S3_BUCKET,
                Key = videoId
            };

            GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            using (Stream stream = response.ResponseStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    base64Content = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            return base64Content;
        }

        public async Task<SendMessageResponse> SQSSendMessage(object message)
        {
            var request = new SendMessageRequest
            {
                MessageBody = JsonConvert.SerializeObject(message),
                QueueUrl = SQS_QUEUE_URL
            };

            return await _sqsClient.SendMessageAsync(request).ConfigureAwait(false);
        }
    }
}
