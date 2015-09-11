namespace SB01.Core.Logging
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }
}
