using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class JoinLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.JoinLobby;

        public int LobbyID { get; init; }

        public string UserName { get; init; }

        public JoinLobbyMessage(int lobbyID, string userName)
        {
            LobbyID = lobbyID;
            UserName = userName;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                LobbyID = this.LobbyID,
                UserName = this.UserName
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
