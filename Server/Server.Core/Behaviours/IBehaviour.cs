using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours
{
    /// <summary>
    /// Interface representing a behaviour that can be executed by entities in the game world.
    /// </summary>
    public interface IBehaviour
    {
        /// <summary>
        /// The ID inside the database.
        /// </summary>
        public int DatabaseID { get; }

        /// <summary>
        /// The type of entity this behaviour is available for.
        /// Used for filtering behaviours on client side.
        /// </summary>
        public EntityTypeEnum Type { get; }

        /// <summary>
        /// Will be displayed on client side to describe the behaviour.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Executes the behaviour.
        /// </summary>
        /// <param name="entity"> The entity executing the behaviour. </param>
        /// <param name="target"> The target entity of the behaviour, if applicable. </param>
        /// <param name="moduleService"> The module service for accessing module data. </param>
        /// <param name="otherParams"> Additional parameters for behaviour execution. </param>
        public void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <summary>
        /// Checks if the behaviour can be executed.
        /// </summary>
        /// <param name="entity"> The entity attempting to execute the behaviour. </param>
        /// <param name="target"> The target entity of the behaviour, if applicable. </param>
        /// <param name="moduleService"> The module service for accessing module data. </param>
        /// <param name="otherParams"> Additional parameters for behaviour execution. </param>
        /// <returns> True if the behaviour can be executed; otherwise, false. </returns>
        public bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <summary>
        /// Converts the behaviour to a DTO.
        /// </summary>
        /// <returns> The BehaviourDTO representation of the behaviour. </returns>
        public BehaviourDTO ToDTO();
    }

}