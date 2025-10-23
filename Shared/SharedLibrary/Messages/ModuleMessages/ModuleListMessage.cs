using System.Text.Json;
using SharedLibrary.DTOs.ModuleDTO;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains a list of modules.
    /// </summary>
    public class ModuleListMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.ModuleList;
        /// <summary>
        /// ModuleDTO list.
        /// </summary>
        public IEnumerable<ModuleDTO> Modules { get; init; }
        /// <summary>
        /// Constructor for ModuleListMessage.
        /// </summary>
        public ModuleListMessage(IEnumerable<ModuleDTO> modules)
        {
            Modules = modules;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Modules = this.Modules
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
