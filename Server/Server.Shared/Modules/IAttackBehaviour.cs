namespace Server.Shared.Modules
{
    public interface IAttackBehaviour : IBehaviour
    {
        public void Attack(WorldEntity attacker, WorldEntity target);
    }
}
