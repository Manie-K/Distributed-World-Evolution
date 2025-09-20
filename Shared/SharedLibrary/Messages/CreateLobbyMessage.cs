using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class CreateLobbyMessage : MessageBase
    {

        public string LobbyName { get; init; }
        public int MaxPlayers { get; init; }

        public int MapID { get; init; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;

        public IEnumerable<int> ModuleIDs { get; init; }

        public CreateLobbyMessage(string lobbyName, int maxPlayers, int mapID, IEnumerable<int> moduleIDs)
        {
            LobbyName = lobbyName;
            MaxPlayers = maxPlayers;
            MapID = mapID;
            ModuleIDs = moduleIDs;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyName = LobbyName,
                MaxPlayers = MaxPlayers,
                MapID = MapID,
                ModuleIDs = ModuleIDs
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
