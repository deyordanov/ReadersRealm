namespace ReadersRealm.Common;

using Microsoft.Extensions.Configuration;
using static Common.Constants.Constants.StripeSettings;

public class StripeSettings
{
    private readonly IConfiguration _configuration;

    public StripeSettings(IConfiguration configuration)
    {
        this._configuration = configuration;

        this.SecretKey = this._configuration[SecretKeyAsString];
        this.PublishableKey = this._configuration[PublishableKeyAsString];
    }

    public string? SecretKey { get; private set; }

    public string? PublishableKey { get; private set; }
}