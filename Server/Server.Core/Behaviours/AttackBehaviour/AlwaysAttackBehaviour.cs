using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <summary>
    /// Attack behaviour that always allows attacking any target.
    /// </summary>
    public class AlwaysAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 201;

        /// <inheritdoc/>
        public override string Description => "Most basic attack implementation. Always attacks other, gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return target != null && attacker.Id != target.Id;
        }
    
    }

}