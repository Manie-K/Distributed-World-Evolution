using SharedLibrary.DTOs.LobbyDTO;
using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class LobbyDataMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.LobbyData;

        public LobbyDTO Lobby { get; init; }
        public int LobbyID { get; init; }


        public LobbyDataMessage(LobbyDTO lobby, int lobbyID)
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
