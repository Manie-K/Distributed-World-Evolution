using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Messages.ModuleMessages
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
                MessageType = MessageType,
                Modules = Module
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
