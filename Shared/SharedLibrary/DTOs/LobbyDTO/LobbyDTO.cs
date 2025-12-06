using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.DTOs.LobbyDTO
{
    public class LobbyDTO
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public int MapID { get; init; }
        public ICollection<int> ModulesIDs { get; init; }
        public ICollection<WorldEntityDTO> WorldEntities { get; init; }

        public LobbyDTO(int id, string name, int maxPlayers, int currentPlayers, int mapID, ICollection<int> modulesIDs, ICollection<WorldEntityDTO> worldEntities)
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
