using Server.Core;

namespace SharedLibrary.DTOs.ModuleDTO
{
    public class BehaviourDTO
    {
        public int DatabaseID { get; init; }
        public string Description { get; init; }
        public EntityTypeEnum Type { get; init; }
        public BehaviourInteractionTypeEnum InteractionType { get; init; }

        public BehaviourDTO(int databaseID, string description, EntityTypeEnum type, BehaviourInteractionTypeEnum interactionType)
        {
            DatabaseID = databaseID;
            Description = description;
            Type = type;
            InteractionType = interactionType;
        }
    }
}