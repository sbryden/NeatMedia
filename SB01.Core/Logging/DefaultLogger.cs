using System;

namespace SB01.Core.Logging
{
    class DefaultLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine("INFO     " + message + Environment.NewLine);
        }

        public void LogError(string message)
        {
            Console.WriteLine("ERROR    " + message + Environment.NewLine);
        }
    }
}
