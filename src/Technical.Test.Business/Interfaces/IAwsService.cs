using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace Technical.Test.Business.Interfaces
{
    public interface IAwsService
    {
        Task S3UploadBinary(string videoId, byte[] bytes);

        Task<string> S3DownloadBinary(string videoId);

        Task<SendMessageResponse> SQSSendMessage(object message);
    }
}
