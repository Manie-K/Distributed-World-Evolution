using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Logging
{
    public class Log
    {
        public string Content { get; }
        public LogLevelEnum LogLevel { get; }
        public DateTime Timestamp { get; }

        public Log(string content, LogLevelEnum logLevel)
        {
            Content = content;
            LogLevel = logLevel;
            Timestamp = DateTime.Now;
        }
    }
}
