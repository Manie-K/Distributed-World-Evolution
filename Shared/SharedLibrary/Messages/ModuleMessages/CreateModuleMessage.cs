using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
                Modules = this.Module
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
