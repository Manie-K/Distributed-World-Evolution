using Server.Core.Services;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    /// <summary>
    /// Reproduce behaviour that only allows reproduction when the entity is healthy.
    /// </summary>
    public class ReproduceWhenHealthyBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 403;
        /// <inheritdoc/>
        public override string Description => "Reproduces only when healthy.";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (base.CanExecute(entity, target, moduleService, otherParams))
            {
                var module = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
                if (entity.State.Health > module.MaxHealth * 0.5)
                {
                    return true;
                }
            }
            return false;
        }

    }

}