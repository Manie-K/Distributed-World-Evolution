using System.Text.Json;
using SharedLibrary.DTOs.ModuleDTO;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information about creating a new module.
    /// </summary>
    public class CreateModuleMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateModule;
        /// <summary>
        /// Module information to be created.
        /// </summary>
        public ModuleDTO Module { get; init; }
        /// <summary>
        /// Constructor for CreateModuleMessage.
        /// </summary>
        public CreateModuleMessage(ModuleDTO module)
        {
            Module = module;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Module = this.Module
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
