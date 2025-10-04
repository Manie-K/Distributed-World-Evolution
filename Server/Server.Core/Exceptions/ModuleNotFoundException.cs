using System.Diagnostics;
using System.Runtime.Serialization;

namespace Server.Core.Exceptions
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    internal class ModuleNotFoundException : Exception
    {
        public ModuleNotFoundException()
        {
        }

        public ModuleNotFoundException(string? message) : base(message)
        {
        }

        public ModuleNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}