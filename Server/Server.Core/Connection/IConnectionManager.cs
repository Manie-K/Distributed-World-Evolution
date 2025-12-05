using Server.Core.Services;
using SharedLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Connection
{
    public interface IConnectionManager
    {

        /// <summary>
        /// Starts the connection manager to accept client connections.
        /// </summary>
        public Task StartAsync(string[] args);

    }
}
