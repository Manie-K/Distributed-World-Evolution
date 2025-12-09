using Server.Core.Services;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    /// <summary>
    /// Reproduce behaviour that only allows reproduction when the entity is not hungry.
    /// </summary>
    public class ReproduceWhenNotHungryBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 404;
        
        /// <inheritdoc/>
        public override string Description => "Reproduces only when not hungry.";
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (base.CanExecute(entity, target, moduleService, otherParams))
            {
                var module = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
                if (entity.State.Hunger > module.MaxHunger * 0.5)
                {
                    return true;
                }
            }
            return false;
        }

    }

}