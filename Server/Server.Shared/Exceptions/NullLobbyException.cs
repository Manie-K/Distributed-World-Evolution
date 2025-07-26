using System.Diagnostics;
using System.Runtime.Serialization;

namespace Server.Shared.Exceptions
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class NullLobbyException : Exception
    {
        public NullLobbyException()
        {
        }

        public NullLobbyException(string? message) : base(message)
        {
        }

        public NullLobbyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NullLobbyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
