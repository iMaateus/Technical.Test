using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Notifications;
using Technical.Test.Business.Services;
using Technical.Test.Data.Repository;

namespace Technical.Test.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMongoRepository, MongoRepository>();
            services.AddSingleton<IMongoSettings>(configuration.GetSection("MongoSettings").Get<MongoSettings>());

            AWSOptions aWSOptions = new AWSOptions
            {
                Region = RegionEndpoint.SAEast1,
                Credentials = new BasicAWSCredentials(configuration.GetSection("AwsSettings:AccessKeyId").Value, configuration.GetSection("AwsSettings:SecretAccessKey").Value)
            };

            services.AddAWSService<IAmazonS3>(aWSOptions);
            services.AddAWSService<IAmazonSQS>(aWSOptions);

            services.AddScoped<IAwsService, AwsService>();
            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IRecyclerService, RecyclerService>();

            return services;
        }
    }
}
