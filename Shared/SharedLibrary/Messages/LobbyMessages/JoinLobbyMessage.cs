using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class JoinLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.JoinLobby;

        public int LobbyID { get; init; }

        public JoinLobbyMessage(int lobbyID)
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
