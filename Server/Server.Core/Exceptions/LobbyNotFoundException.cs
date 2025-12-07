using System.Diagnostics;

namespace Server.Core.Exceptions
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class LobbyNotFoundException : Exception
    {
        public LobbyNotFoundException()
        {
        }

        public LobbyNotFoundException(string? message) : base(message)
        {
        }

        public LobbyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }


        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
