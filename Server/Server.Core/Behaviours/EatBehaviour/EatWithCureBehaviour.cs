using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class EatWithCureBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 304;
        /// <inheritdoc/>
        public override string Description => "Eats and increase health at the same time, but decrease health twice if plan is poisonous";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

            if (targetModule.Damage > 0)
            {
                entity.State.Health -= 2 * targetModule.Damage;
            }
            else
            {
                entity.State.Hunger += targetModule.MaxHunger;
                entity.State.Health += targetModule.MaxHunger;
            }

            target.Die(moduleService);
        }
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return true;
        }
    }
}
