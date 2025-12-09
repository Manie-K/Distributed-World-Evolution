using System.Diagnostics;

namespace Server.Core.Exceptions
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class ModuleNotFoundException : Exception
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