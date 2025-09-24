using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SharedLibrary.DTOs.LobbyDTO;

namespace SharedLibrary.Messages
{
    public class LobbyListMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.LobbyList;

        public IEnumerable<LobbyDTO> Lobbies { get; init; }

        public LobbyListMessage(IEnumerable<LobbyDTO> lobbies)
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
