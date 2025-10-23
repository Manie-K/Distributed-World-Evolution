using Server.Core.Exceptions;
using Server.Core.Services;

namespace Server.Core.Behaviours.Implementations
{
    public class TEMPDEMOMoreComplicatedAttackBehaviour : AttackBehaviourBase
    {
        public override int DatabaseID => 1;
        public override EntityTypeEnum Type => EntityTypeEnum.Animal;

        public override string Description => "whatever";

        public override bool CanExecute(WorldEntity attacker, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                var attackerModule = ModuleService.Instance.GetModuleById(attacker.ModuleID);
                var targetModule = ModuleService.Instance.GetModuleById(target.ModuleID);
                return attackerModule.Agression > targetModule.Agression && attacker.State.Health > targetModule.Damage;
            }
            catch (ModuleNotFoundException)
            {
                return false;
            }
        }
    }
}
