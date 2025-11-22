using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class OnlyAttackSelfSpiecesAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 209;

        /// <inheritdoc/>
        public override string Description => "Only attacks organisms of the same species. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return target != null && attacker.Id != target.Id && attacker.ModuleID == target.ModuleID;
        }
    }
}
