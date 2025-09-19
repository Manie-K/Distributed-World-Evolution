using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class CreateLobbyMessage : MessageBase
    {

        public string LobbyName { get; init; }
        public int MaxPlayers { get; init; }

        public int MapID { get; init; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;
        //TODO: change when modules are implemented
        Dictionary<string, string> Modules { get; init; } = new Dictionary<string, string>();

        public CreateLobbyMessage(string lobbyName, int maxPlayers, int mapID, Dictionary<string, string> modules)
        {
            LobbyName = lobbyName;
            MaxPlayers = maxPlayers;
            MapID = mapID;
            Modules = modules;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyName = LobbyName,
                MaxPlayers = MaxPlayers,
                MapID = MapID,
                Modules = Modules
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
