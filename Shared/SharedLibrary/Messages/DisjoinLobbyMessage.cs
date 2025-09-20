using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Messages
{
    public class DisjoinLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.DisjoinLobby;

        public int LobbyID { get; init; }

        public DisjoinLobbyMessage(int lobbyID)
        {
            LobbyID = lobbyID;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyID = LobbyID
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
