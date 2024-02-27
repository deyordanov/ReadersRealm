namespace ReadersRealm.Web.Infrastructure.Settings;

using Contracts;

public class MongoDbSettings : IMongoDbSettings
{
    public string? ConnectionString { get; set; }

    public string? DatabaseName { get; set; }
}