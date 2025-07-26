using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Logging
{
    public interface ILogger
    {
        public void Log(string message, LogLevel level);
        public void Log(Exception exception, LogLevel level);
    }
}
