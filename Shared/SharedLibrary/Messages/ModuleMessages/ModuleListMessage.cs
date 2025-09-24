using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SharedLibrary.DTOs.ModuleDTO;

namespace SharedLibrary.Messages
{
    public class ModuleListMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.ModuleList;

        public IEnumerable<ModuleDTO> Modules { get; init; }

        public ModuleListMessage(IEnumerable<ModuleDTO> modules)
        {
            Modules = modules;
        }

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
