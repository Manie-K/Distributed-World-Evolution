using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    /// <summary>
    /// Eat behaviour that allows eating when hunger is below 50% of maximum hunger.
    /// </summary>
    public class EatWhenHungry50Behaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 301;
        
        /// <inheritdoc/>
        public override string Description => "Eats when hunger below 50% of organism maximum hunger";
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");

            if ((float)entity.State.Hunger/entityModule.MaxHunger < 0.5)
            {
                return true;
            }

            return false;
        }

    }

}