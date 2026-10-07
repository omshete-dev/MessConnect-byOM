using System.Configuration;

public static class DbConfig
{
    public static string GetConnectionString(string name)
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
        if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            throw new ConfigurationErrorsException("Missing connection string: " + name);

        return settings.ConnectionString;
    }
}