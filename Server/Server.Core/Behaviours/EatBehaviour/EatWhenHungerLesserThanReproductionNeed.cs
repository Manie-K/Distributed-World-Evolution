using Server.Core.Helpers;
using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class EatWhenHungerLesserThanReproductionNeed : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 303;
        /// <inheritdoc/>
        public override string Description => "Eats when reproduction need is lesser than hunger need";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");

            if ((float)entity.State.Hunger / entityModule.MaxHunger < (float)entityModule.ReproductionNeed/ModulePropertiesLimits.MAX_REPRODUCTION_NEED)
            {
                return true;
            }

            return false;
        }
    }
}
