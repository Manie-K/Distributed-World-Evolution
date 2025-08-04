using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public abstract class MessageBase
    {
        public MessageType MessageType { get; }
        public abstract string BuildJson();
    }
}
