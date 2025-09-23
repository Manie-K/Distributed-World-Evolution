namespace Server.Core.Modules
{
    public class BasicAttackBehaviour : IAttackBehaviour
    {
        public int DatabaseID => 111;

        public void Attack(WorldEntity attacker, WorldEntity target)
        {
            Console.WriteLine($"{attacker.Name} attacks {target.Name} with a basic attack!");
        }
    }
}
