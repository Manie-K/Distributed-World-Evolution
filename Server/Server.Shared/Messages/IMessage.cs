using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public interface IMessage
    {
        IMessageType MessageType { get; }
        string BuildJson();
    }
}
