using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    public class PlantAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 213;
        
        /// <inheritdoc/>
        public override string Description => throw new NotImplementedException();
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            throw new NotImplementedException();
        }
    }
}
