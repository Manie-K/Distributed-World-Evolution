using System.Diagnostics;
using System.Runtime.Serialization;

namespace Server.Core.Exceptions
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class BehaviourImplementationNotFoundException : Exception
    {
        public BehaviourImplementationNotFoundException()
        {
        }

        public BehaviourImplementationNotFoundException(string? message) : base(message)
        {
        }

        public BehaviourImplementationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
