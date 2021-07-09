namespace Technical.Test.Business.Interfaces
{
    public interface IMongoSettings
    {
        MongoHost TechnicalTest { get; set; }

        Collections Collections { get; set; }
    }

    public class MongoSettings : IMongoSettings
    {
        public MongoHost TechnicalTest { get; set; }

        public Collections Collections { get; set; }
    }

    public class MongoHost
    {
        public string Host { get; set; }

        public string Database { get; set; }

        public int ClusterId { get; set; }
    }

    public class Collections
    {
        public string Servers { get; set; }

        public string Videos { get; set; }

        public string Recycler { get; set; }
    }
}
