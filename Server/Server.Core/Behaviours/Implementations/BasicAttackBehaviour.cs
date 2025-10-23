using Server.Core.Modules;

namespace Server.Core.Behaviours.Implementations
{
    public class BasicAttackBehaviour : AttackBehaviourBase
    {
        public override int DatabaseID => 3;

        public override string Description => "Here we will hard-code descriptions";


        public override bool CanExecute(WorldEntity attacker, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id;
        }
    }
}
