using System.Text.Json;

namespace Server.Shared.Messages
{
    public class JoinLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.JoinLobby;

        //TODO: add properties for lobby joining
        public int LobbyID { get; set; }


        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyID = LobbyID
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
