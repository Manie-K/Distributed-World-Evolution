using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.DTOs.LobbyDTO
{
    /// <summary>
    /// DTO representing a game lobby.
    /// </summary>
    public class LobbyDTO
    {
        /// <summary>
        /// Identifier of the lobby.
        /// </summary>
        public int ID { get; init; }

        /// <summary>
        /// Name of the lobby.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Maximum number of players allowed in the lobby.
        /// </summary>
        public int MaxPlayers { get; init; }

        /// <summary>
        /// Current number of players in the lobby.
        /// </summary>
        public int CurrentPlayers { get; init; }

        /// <summary>
        /// Identifier of the map associated with the lobby.
        /// </summary>  
        public int MapID { get; init; }

        /// <summary>
        /// Identifiers of the modules associated with the lobby.
        /// </summary>
        public IEnumerable<int> ModulesIDs { get; init; }

        /// <summary>
        /// World entities present in the lobby.
        /// </summary>
        public IEnumerable<WorldEntityDTO> WorldEntities { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LobbyDTO(int id, string name, int maxPlayers, int currentPlayers, int mapID, IEnumerable<int> modulesIDs, IEnumerable<WorldEntityDTO> worldEntities)
        {
            ID = id;
            Name = name;
            MaxPlayers = maxPlayers;
            CurrentPlayers = currentPlayers;
            MapID = mapID;
            ModulesIDs = modulesIDs;
            WorldEntities = worldEntities;
        }

    }

}
