using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    /// <summary>
    /// Reproduce behaviour that reduces hunger of the entity and its target upon reproduction.
    /// </summary>
    public class ReproduceReducingHungerBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 405;
        
        /// <inheritdoc/>
        public override string Description => "Reproduction reduce hunger.";
        
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            base.Execute(entity, target, moduleService, otherParams);
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            entity.State.Hunger -= (int)0.1 * entityModule.MaxHunger;
            if (target != null)
            {
                target.State.Hunger -= (int)0.1 * entityModule.MaxHunger;
            }
        }

    }

}