using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Technical.Test.Api.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            MongoClientSettings mongo = MongoClientSettings.FromConnectionString(configuration.GetSection("MongoSettings:TechnicalTest:ConnectionString").Value);
            mongo.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

            services.AddScoped<IMongoClient>(c =>
            {
                return new MongoClient(mongo);
            });

            return services;
        }
    }
}
