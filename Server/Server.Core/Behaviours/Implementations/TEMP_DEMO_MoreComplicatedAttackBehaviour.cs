using Server.Core.Exceptions;
using Server.Core.Modules;

namespace Server.Core.Behaviours.Implementations
{
    public class TEMPDEMOMoreComplicatedAttackBehaviour : AttackBehaviourBase
    {
        public override int DatabaseID => throw new NotImplementedException();
        public override EntityTypeEnum Type => EntityTypeEnum.Animal;

        public override bool CanExecute(WorldEntity attacker, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                var attackerModule = ModuleService.Instance.GetModuleById(attacker.ModuleID);
                var targetModule = ModuleService.Instance.GetModuleById(target.ModuleID);
                return attackerModule.Aggresion > targetModule.Aggresion && attacker.State.Health > targetModule.Damage;
            }
            catch (ModuleNotFoundException)
            {
                return false;
            }
        }
    }
}
