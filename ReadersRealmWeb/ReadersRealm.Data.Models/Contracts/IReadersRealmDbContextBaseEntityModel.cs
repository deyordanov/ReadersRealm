using System.Security.Cryptography;

namespace ReadersRealm.Data.Models.Contracts;

public interface IReadersRealmDbContextBaseEntityModel<TId>
{
    TId Id { get; }
}