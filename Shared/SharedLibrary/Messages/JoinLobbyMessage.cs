using System.Text.Json;

namespace SharedLibrary
{
    public class JoinLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.JoinLobby;

        public int LobbyID { get; set; }


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
