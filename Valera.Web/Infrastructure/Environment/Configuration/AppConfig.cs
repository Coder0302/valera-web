namespace ValeraWeb.Infrastructure.Environment.Configuration;

public class AppConfig
{
    public ValeraConfig ValeraConfig { get; set; }

    public AppConfig(IConfiguration configuration)
    {
        ValeraConfig = new ValeraConfig(configuration.GetSection(nameof(ValeraConfig)));
    }

    public void Validate()
    {
        ValeraConfig.Validate();
    }
}
