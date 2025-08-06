namespace Assignment.Singletons
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
        void LogWarning(string message);
    }

    public class Logger : ILogger
    {
        private static readonly Lazy<Logger> _instance = 
            new Lazy<Logger>(() => new Logger());
        
        private readonly object _lock = new object();
        private readonly List<string> _logs = new List<string>();

        public static Logger Instance => _instance.Value;

        private Logger() { }

        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        private void Log(string level, string message)
        {
            lock (_lock)
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                _logs.Add(logEntry);
                Console.WriteLine(logEntry);
            }
        }

        public List<string> GetLogs()
        {
            lock (_lock)
            {
                return new List<string>(_logs);
            }
        }

        public void ClearLogs()
        {
            lock (_lock)
            {
                _logs.Clear();
            }
        }
    }
}
