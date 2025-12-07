using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours.EatBehaviour
{
    public abstract class EatBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal;

        /// <inheritdoc/>
        public abstract string Description { get; }

        /// <inheritdoc/>
        public virtual void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (target == null)
            {
                return;
            }

            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new ModuleNotFoundException($"Module with ID={entity.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new ModuleNotFoundException($"Module with ID={target.ModuleID} not found!");

            entity.State.Health -= targetModule.Damage;
            entity.State.Hunger += targetModule.MaxHunger;
            entity.State.Hunger = Math.Clamp(entity.State.Hunger, 0, entityModule.MaxHunger);

            target.Die(moduleService);
            if (entity.State.Health <= 0)
            {
                entity.Die(moduleService);
            }
        }

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehaviourDTO ToDTO()
        {
            return new BehaviourDTO(DatabaseID, Description, Type, BehaviourInteractionTypeEnum.Eat);
        }

    }
}
