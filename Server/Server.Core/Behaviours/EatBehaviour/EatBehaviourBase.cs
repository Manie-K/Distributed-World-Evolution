using Server.Core.Helpers;
using Server.Core.Modules;
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
        public void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            if (otherParams == null || !otherParams.TryGetValue(CustomBehaviourParams.TARGET_MODULE_PARAM, out var moduleObj))
                throw new ArgumentException("Missing targetModule in parameters");

            var targetModule = moduleObj as Module
                ?? throw new ArgumentException("Invalid targetModule type");

            if (targetModule.Damage > 0)
            {
                entity.State.Hunger = entity.State.Hunger - targetModule.Damage;
            }
            else
            {
                entity.State.Hunger = entity.State.Hunger + targetModule.MaxHealth;
            }
        }

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehviourDTO ToDTO()
        {
            return new BehviourDTO(DatabaseID, Description, Type);
        }

    }
}
