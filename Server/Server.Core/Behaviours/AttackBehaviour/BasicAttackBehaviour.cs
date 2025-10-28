using Server.Core.Modules;

namespace Server.Core.Behaviours.AttackBehaviour
{
    public class BasicAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 201;

        /// <inheritdoc/>
        public override string Description => "Most basic attack implementation. Always attacks other, gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id;
        }
    }
}
