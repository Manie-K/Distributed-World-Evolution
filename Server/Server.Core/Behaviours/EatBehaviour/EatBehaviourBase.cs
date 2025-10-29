using Server.Core.Helpers;
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
        public virtual void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

            if (targetModule.Damage > 0)
            {
                entity.State.Health -= targetModule.Damage;
            }
            else
            {
                entity.State.Hunger += targetModule.MaxHunger;
            }

            target.Die();
        }

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehviourDTO ToDTO()
        {
            return new BehviourDTO(DatabaseID, Description, Type);
        }

    }
}
