using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SB01.Core.Logging;

namespace SB01.WPF
{
    public class WPFLogger : ILogger
    {
        public void LogInfo(string message)
        {
            ConsoleWindow.AddConsoleText("/n" + "INFO     " + message);
        }

        public void LogError(string message)
        {
            ConsoleWindow.AddConsoleText("/n" + "ERROR    " + message);
        }
    }
}
