using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class Random25AttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 207;

        /// <inheritdoc/>
        public override string Description => "25% chance to attack. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id && Random.Shared.NextDouble() < 0.25;
        }
    }
}
