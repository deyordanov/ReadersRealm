namespace ReadersRealm.Web.Infrastructure.Settings.Contracts;

public interface IMongoDbSettings
{
    string? ConnectionString { get; }
    string? DatabaseName { get; }
}