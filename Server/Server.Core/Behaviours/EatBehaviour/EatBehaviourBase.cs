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
            //TODO: Hardcoded hunger increase, should be based on food nutrition value
            entity.State.Hunger = entity.State.Hunger + 1;
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
