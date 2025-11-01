using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours
{
    public interface IBehaviour
    {
        /// <summary>
        /// The ID inside the database.
        /// </summary>
        public int DatabaseID
        {
            get;
        }

        /// <summary>
        /// The type of entity this behaviour is available for.
        /// Used for filtering behaviours on client side.
        /// </summary>
        public EntityTypeEnum Type
        {
            get;
        }


        /// <summary>
        /// Will be displayed on client side to describe the behaviour.
        /// </summary>
        public string Description
        {
            get;
        }


        /// <summary>
        /// Executes the behaviour.
        /// </summary>
        public void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <summary>
        /// Checks if the behaviour can be executed.
        /// </summary>
        /// <returns></returns>
        public bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <summary>
        /// Converts the behaviour to a DTO.
        /// </summary>
        public BehaviourDTO ToDTO();
    }
}