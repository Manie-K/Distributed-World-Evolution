namespace SharedLibrary.DTOs.EntitiesDTO
{
    /// <summary>
    /// DTO representing a world entity.
    /// </summary>
    public class WorldEntityDTO
    {
        /// <summary>
        /// ID of the world entity.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Name of the world entity.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// State of the world entity.
        /// </summary>
        public EntityStateDTO State { get; set; }

        /// <summary>
        /// ID of the module the entity belongs to.
        /// </summary>
        public int ModuleID { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>  
        /// <param name="name"> Name of the world entity. </param>
        /// <param name="id"> ID of the world entity. </param>
        /// <param name="state"> State of the world entity. </param>
        /// <param name="moduleID"> ID of the module the entity belongs to. </param>
        public WorldEntityDTO(string? name, Guid id, EntityStateDTO state, int moduleID)
        {
            Name = name;
            Id = id;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
            ModuleID = moduleID;
        }

    }

}
