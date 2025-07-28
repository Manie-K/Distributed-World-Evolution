using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Frames
{
    public class MessageDataFrame : DataFrameBase
    {
        public byte[] Message { get; private set; }

        public MessageDataFrame(byte[] message)
        {
            Message = message;
        }
    }
}
