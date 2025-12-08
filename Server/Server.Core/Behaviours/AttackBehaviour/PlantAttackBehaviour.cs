using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    public class PlantAttackBehaviour : AttackBehaviourBase
    {
        /// <summary>
        /// Plant attack behaviour that does not allow plants to attack.
        /// </summary>
        public override int DatabaseID => 213;
        
        /// <inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;
        
        /// <inheritdoc/>
        public override string Description => "Plants cannot fight.";
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }
        
        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return; //no-op
        }

    }

}