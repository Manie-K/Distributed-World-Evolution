using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Logging
{
    internal class OnLogEventArgs
    {
        public string Message { get; }
        public LogLevelEnum LogLevel { get; }
        public DateTime Timestamp { get; }
    
        public OnLogEventArgs(string message, LogLevelEnum logLevel)
        {
            Message = message;
            LogLevel = logLevel;
            Timestamp = DateTime.Now;
        }
    }
}
