using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Logging
{
    //TODO: Refactor
    public class Logger : ILogger
    {
        public void Log(string message, LogLevel level)
        {
            Console.WriteLine($"[{DateTime.Now}] [{level}]: {message}");
        }

        public void Log(Exception exception, LogLevel level)
        {
            Console.WriteLine($"[{DateTime.Now}] [{level}]: {exception.Message}");
        }
    }
}
