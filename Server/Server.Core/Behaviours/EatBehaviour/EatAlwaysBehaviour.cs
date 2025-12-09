using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    /// <summary>
    /// Eat behaviour that always allows eating.
    /// </summary>
    public class EatAlwaysBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 307;
        
        /// <inheritdoc/>
        public override string Description => "Eats always";
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return true;
        }

    }

}
