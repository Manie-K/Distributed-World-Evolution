namespace Server.Core.Modules
{
    public class TEMP_DEMO_MoreComplicatedAttackBehaviour : IAttackBehaviour
    {
        public int DatabaseID => 666;

        public void Attack(WorldEntity attacker, WorldEntity target)
        {
            int dmg = ModuleService.Instance.GetModuleById(attacker.ModuleID).Damage;
            target.State.Health -= dmg;
        }

        public bool ShouldAttack(WorldEntity attacker, WorldEntity target)
        {
            var attackerModule = ModuleService.Instance.GetModuleById(attacker.ModuleID);
            var targetModule = ModuleService.Instance.GetModuleById(target.ModuleID);

            return attackerModule.Aggresion > targetModule.Aggresion && attacker.State.Health > targetModule.Damage;

        }
    }
}
