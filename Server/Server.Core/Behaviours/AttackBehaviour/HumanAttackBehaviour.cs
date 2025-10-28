using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class HumanAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 211;

        /// <inheritdoc/>
        public override string Description => "Attack behaviour used by humans";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id;
        }
    }
}
