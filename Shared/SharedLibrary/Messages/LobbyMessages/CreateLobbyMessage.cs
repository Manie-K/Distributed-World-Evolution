using System.Text.Json;

namespace SharedLibrary.Messages
{
    //TODO: Validate
    public class CreateLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;
        
        public string LobbyName { get; init; }
        public int MaxPlayers { get; init; }

        public int MapID { get; init; }

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
                MessageType = this.MessageType,
                LobbyName = this.LobbyName,
                MaxPlayers = this.MaxPlayers,
                MapID = this.MapID,
                ModuleIDs = this.ModuleIDs
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
