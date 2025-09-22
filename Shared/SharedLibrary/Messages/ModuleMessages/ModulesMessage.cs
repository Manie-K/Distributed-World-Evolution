using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Messages
{
    public class ModulesMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.Modules;

        public IEnumerable<ModuleDTO> Modules { get; init; }

        public ModulesMessage(IEnumerable<ModuleDTO> modules)
        {
            Modules = modules;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                Modules = Modules
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
