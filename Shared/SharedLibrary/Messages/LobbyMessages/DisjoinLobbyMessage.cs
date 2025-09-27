using System.Text.Json;

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
                MessageType = this.MessageType,
                LobbyID = this.LobbyID
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
