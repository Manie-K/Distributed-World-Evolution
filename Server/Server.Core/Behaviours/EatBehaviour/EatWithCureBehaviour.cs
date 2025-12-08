using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    /// <summary>
    /// Eat behaviour that increases health when eating, but decreases health twice if the food is poisonous.
    public class EatWithCureBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 304;
        
        /// <inheritdoc/>
        public override string Description => "Eats and increase health at the same time, but decrease health twice if plan is poisonous";
        
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (target == null)
            {
                return;
            }

            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

            if (targetModule.Damage > 0)
            {
                entity.State.Health -= 2 * targetModule.Damage;
                entity.State.Hunger += targetModule.MaxHunger;
            }
            else
            {
                entity.State.Hunger += targetModule.MaxHunger;
                entity.State.Health += targetModule.MaxHunger;
            }

            entity.State.Hunger = Math.Clamp(entity.State.Hunger, 0, entityModule.MaxHunger);

            target.Die(moduleService);
            if(entity.State.Health <= 0)
            {
                entity.Die(moduleService);
            }
        }
        
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return true;
        }

    }

}