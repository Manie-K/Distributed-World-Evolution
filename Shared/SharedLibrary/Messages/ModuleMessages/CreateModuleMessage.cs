using System.Text.Json;
using SharedLibrary.DTOs.ModuleDTO;

namespace SharedLibrary.Messages
{
    public class CreateModuleMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateModule;

        public ModuleDTO Module { get; init; }

        public CreateModuleMessage(ModuleDTO module)
        {
            Module = module;
        }

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
