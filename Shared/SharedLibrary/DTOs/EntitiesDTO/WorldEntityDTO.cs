using System;
using Server.Core;

namespace SharedLibrary.DTOs.EntitiesDTO
{
    public class WorldEntityDTO
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public EntityStateDTO State { get; private set; }
        public int ModuleID { get; set; }

        public WorldEntityDTO(string? name, Guid id, EntityStateDTO state, int moduleID)
        {
            Name = name;
            Id = id;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
            ModuleID = moduleID;
        }
    }
}
