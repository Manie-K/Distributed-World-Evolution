namespace Server.Core.Behaviours
{
    public interface IAttackBehaviour : IBehaviour
    {
        public void Attack(WorldEntity attacker, WorldEntity target);
        public bool CanAttack(WorldEntity attacker, WorldEntity target);
    }
}
