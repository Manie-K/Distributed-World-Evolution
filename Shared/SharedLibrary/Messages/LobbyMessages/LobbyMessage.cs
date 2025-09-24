using SharedLibrary.DTOs.LobbyDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Messages
{
    public class LobbyMessage : MessageBase
    {
        public LobbyDTO Lobby { get; init; }
        public int LobbyID { get; init; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.Lobby;

        public LobbyMessage(LobbyDTO lobby, int lobbyID)
        {
            LobbyID = lobbyID;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                Lobby = Lobby,
                LobbyID = LobbyID
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
