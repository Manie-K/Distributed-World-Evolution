using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information needed to create a new game lobby.
    /// </summary>
    public class CreateLobbyMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;
        /// <summary>
        /// Lobby name to be created.
        /// </summary>
        public string LobbyName { get; init; }
        /// <summary>
        /// Maximum number of players allowed in the lobby.
        /// </summary>
        public int MaxPlayers { get; init; }
        /// <summary>
        /// Map ID for the lobby.
        /// </summary>
        public int MapID { get; init; }
        /// <summary>
        /// IDs of modules to be included in the lobby.
        /// </summary>
        public IEnumerable<int> ModuleIDs { get; init; }
        /// <summary>
        /// Constructor for CreateLobbyMessage.
        /// </summary>
        public CreateLobbyMessage(string lobbyName, int maxPlayers, int mapID, IEnumerable<int> moduleIDs)
        {
            LobbyName = lobbyName;
            MaxPlayers = maxPlayers;
            MapID = mapID;
            ModuleIDs = moduleIDs;
        }
        /// <inheritdoc/>
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
