using Server.Core;

namespace SharedLibrary.DTOs.ModuleDTO
{
    public class BehviourDTO
    {
        public int DatabaseID { get; init; }
        public string Description { get; init; }
        public EntityTypeEnum Type { get; init; }

        public BehviourDTO(int databaseID, string description, EntityTypeEnum type)
        {
            DatabaseID = databaseID;
            Description = description;
            Type = type;
        }
    }
}