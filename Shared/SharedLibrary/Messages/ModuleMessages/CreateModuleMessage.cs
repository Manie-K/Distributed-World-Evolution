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
        /// ModuleDTO information to be created.
        /// </summary>
        public CreateModuleDTO ModuleDTO { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="moduleDTO"> ModuleDTO information to be created. </param>
        public CreateModuleMessage(CreateModuleDTO moduleDTO)
        {
            ModuleDTO = moduleDTO;
        }

        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                ModuleDTO = this.ModuleDTO
            };

            return JsonSerializer.Serialize(payload);
        }

    }

}
