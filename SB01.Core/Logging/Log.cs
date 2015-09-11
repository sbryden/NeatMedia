namespace SB01.Core.Logging
{
    public static class Log
    {
        private static ILogger Logger;

        static Log()
        {
            Logger = new DefaultLogger();
        }

        public static void InitLogger(ILogger logger)
        {
            Logger = logger;
        }

        public static void Info(string message)
        {
            Logger.LogInfo(message);
        }

        public static void Error(string message)
        {
            Logger.LogError(message);
        }
    }
}
