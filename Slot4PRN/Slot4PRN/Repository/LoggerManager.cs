using Slot4PRN.IRepository;

namespace Slot4PRN.Repository
{
    public class LoggerManager : ILoggerManager
    {
        public void LogInfo(string message) => Console.WriteLine($"[INFO]  {message}");
        public void LogWarn(string message) => Console.WriteLine($"[WARN]  {message}");
        public void LogDebug(string message) => Console.WriteLine($"[DEBUG] {message}");
        public void LogError(string message) => Console.WriteLine($"[ERROR] {message}");
    }
}
