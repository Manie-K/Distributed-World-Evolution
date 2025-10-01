namespace Server.Core.Modules
{
    public interface IAttackBehaviour : IBehaviour
    {
        public void Attack(WorldEntity attacker, WorldEntity target);
        public bool ShouldAttack(WorldEntity attacker, WorldEntity target);
    }
}
