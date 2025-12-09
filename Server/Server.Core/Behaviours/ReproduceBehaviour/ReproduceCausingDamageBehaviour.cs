using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    /// <summary>
    /// Reproduce behaviour that causes damage to the entity and its target upon reproduction.
    /// </summary>
    public class ReproduceCausingDamageBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 406;
        
        /// <inheritdoc/>
        public override string Description => "Reproduction cause damage.";
        
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            base.Execute(entity, target, moduleService, otherParams);

            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            entity.State.Health -= entityModule.Damage;
            if (target != null)
            {
                target.State.Health -= entityModule.Damage;
            }
        }

    }

}