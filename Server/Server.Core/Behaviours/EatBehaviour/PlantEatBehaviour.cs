using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    /// <summary>
    /// Plant eat behaviour that does nothing.
    /// </summary>
    public class PlantEatBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 309;
        
        /// <inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;
        
        /// <inheritdoc/>
        public override string Description => "Plants do not eat like an animals.";
        
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return;
        }
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }

    }

}