
namespace Server.Core.Modules
{
    public class BasicAttackBehaviour : IAttackBehaviour
    {
        public int DatabaseID => 111;
        public EntityTypeEnum Type { get => EntityTypeEnum.Human | EntityTypeEnum.Animal; }


        public void Attack(WorldEntity attacker, WorldEntity target)
        {
            int dmg = ModuleService.Instance.GetModuleById(attacker.ModuleID).Damage;
            target.State.Health -= dmg;
        }

        public bool ShouldAttack(WorldEntity attacker, WorldEntity target)
        {
            return attacker.Id != target.Id;
        }
    }
}
