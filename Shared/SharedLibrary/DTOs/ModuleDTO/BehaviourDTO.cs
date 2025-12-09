using Server.Core;

namespace SharedLibrary.DTOs.ModuleDTO
{
    /// <summary>
    /// DTO representing a behaviour.
    /// </summary>
    public class BehaviourDTO
    {
        /// <summary>
        /// Identifier of the behaviour in the database.
        /// </summary>
        public int DatabaseID { get; init; }

        /// <summary>
        /// Description of the behaviour.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Tyepe of entity the behaviour is associated with.
        /// </summary>
        public EntityTypeEnum Type { get; init; }

        /// <summary>
        /// Type of interaction the behaviour entails.
        /// </summary>
        public BehaviourInteractionTypeEnum InteractionType { get; init; }


        /// <summary>
        /// Constructor.
        /// </summary>
        public BehaviourDTO(int databaseID, string description, EntityTypeEnum type, BehaviourInteractionTypeEnum interactionType)
        {
            DatabaseID = databaseID;
            Description = description;
            Type = type;
            InteractionType = interactionType;
        }

    }

}