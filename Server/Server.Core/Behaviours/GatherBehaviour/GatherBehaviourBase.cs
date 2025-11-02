using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours.GatherBehaviour
{
    public abstract class GatherBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Human;

        /// <inheritdoc/>
        public abstract string Description { get; }


        /// <inheritdoc/>
        public abstract void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehaviourDTO ToDTO()
        {
            return new BehaviourDTO(DatabaseID, Description, Type, BehaviourInteractionTypeEnum.None);
        }
    }
}
