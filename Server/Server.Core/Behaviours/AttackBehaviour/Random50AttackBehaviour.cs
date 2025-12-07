using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <summary>
    /// Random 50% chance to attack behaviour.
    /// </summary>
    public class Random50AttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 205;

        /// <inheritdoc/>
        public override string Description => "50% chance to attack. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return target != null && attacker.Id != target.Id && Random.Shared.NextDouble() < 0.5;
        }

    }

}