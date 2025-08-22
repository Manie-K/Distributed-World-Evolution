using System.Text.Json;

namespace SharedLibrary
{
    public class CreateLobbyMessage : MessageBase
    {

        public string LobbyName { get; private set; }
        public int MaxPlayers { get; private set; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;
        //TODO: change when modules are implemented
        Dictionary<string, string> Modules { get; set; } = new Dictionary<string, string>();


        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyName = LobbyName,
                MaxPlayers = MaxPlayers,
                Modules = Modules
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
