using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.LobbyDTO
{
    public class LobbyDTO
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public int MapID { get; init; }
        public ICollection<ModuleDTO.ModuleDTO> Modules { get; init; }

        public LobbyDTO(int id, string name, int maxPlayers, int currentPlayers, int mapID, ICollection<ModuleDTO.ModuleDTO> modules)
        {
            ID = id;
            Name = name;
            MaxPlayers = maxPlayers;
            CurrentPlayers = currentPlayers;
            MapID = mapID;
            Modules = modules;
        }

    }
}
