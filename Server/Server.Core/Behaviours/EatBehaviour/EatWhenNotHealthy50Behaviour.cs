using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    /// <summary>
    /// Eat behaviour that allows eating when health is below 50% of maximum health.
    /// </summary>
    public class EatWhenNotHealthy50Behaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 302;
        
        /// <inheritdoc/>
        public override string Description => "Eats when health below 50% of organism maximum health";
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");

            if ((float)entity.State.Health/entityModule.MaxHealth < 0.5)
            {
                return true;
            }

            return false;
        }

    }

}