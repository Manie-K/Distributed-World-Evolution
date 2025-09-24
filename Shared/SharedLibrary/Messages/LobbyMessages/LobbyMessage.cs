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
        public override MessageTypeEnum MessageType => MessageTypeEnum.LobbyData;

        public LobbyDTO Lobby { get; init; }
        public int LobbyID { get; init; }


        public LobbyMessage(LobbyDTO lobby, int lobbyID)
        {
            Lobby = lobby;
            LobbyID = lobbyID;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Lobby = this.Lobby,
                LobbyID = this.LobbyID
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
