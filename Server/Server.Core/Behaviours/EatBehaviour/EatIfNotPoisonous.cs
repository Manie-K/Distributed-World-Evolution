using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class EatIfNotPoisonous : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 308;
        /// <inheritdoc/>
        public override string Description => "Eats only if plant is not poisonous";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if(target == null)
            {
                return false;
            }

            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

            if (targetModule.Damage > 0)
            {
                return false;
            }

            return true;
        }
    }
}
