namespace Assignment.Singletons
{
    public interface IConfigurationManager
    {
        string GetSetting(string key);
        void SetSetting(string key, string value);
    }

    public class ConfigurationManager : IConfigurationManager
    {
        private static readonly Lazy<ConfigurationManager> _instance = 
            new Lazy<ConfigurationManager>(() => new ConfigurationManager());
        
        private readonly Dictionary<string, string> _settings;
        private readonly object _lock = new object();

        public static ConfigurationManager Instance => _instance.Value;

        private ConfigurationManager()
        {
            _settings = new Dictionary<string, string>
            {
                { "DatabaseConnection", "Server=localhost;Database=ECommerce;Trusted_Connection=true;" },
                { "ApiKey", "your-api-key-here" },
                { "MaxOrderItems", "50" },
                { "DefaultCurrency", "USD" },
                { "LogLevel", "Info" }
            };
        }

        public string GetSetting(string key)
        {
            lock (_lock)
            {
                return _settings.TryGetValue(key, out var value) ? value : string.Empty;
            }
        }

        public void SetSetting(string key, string value)
        {
            lock (_lock)
            {
                _settings[key] = value;
            }
        }
    }
}
