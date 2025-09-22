using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Messages
{
    public class LobbiesMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.Lobbies;

        public IEnumerable<LobbyDTO> Lobbies { get; init; }

        public LobbiesMessage(IEnumerable<LobbyDTO> lobbies)
        {
            Lobbies = lobbies;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                Lobbies = Lobbies
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
