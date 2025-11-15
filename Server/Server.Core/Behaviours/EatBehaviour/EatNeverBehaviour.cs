using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class EatNeverBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 306;
        /// <inheritdoc/>
        public override string Description => "Eats never";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return;
        }
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }
    }
}
