using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class AlwaysAttackAnimalsBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 202;

        /// <inheritdoc/>
        public override string Description => "Attacks only animals. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (target == null)
            {
                return false;
            }

            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && targetModule.Type == EntityTypeEnum.Animal;
        }
    }
}
