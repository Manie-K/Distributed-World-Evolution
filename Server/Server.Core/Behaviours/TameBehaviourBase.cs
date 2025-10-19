using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours
{
    public abstract class TameBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal;

        /// <inheritdoc/>
        public abstract string Description { get; }

        /// <inheritdoc/>
        public abstract void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehviourDTO ToDTO()
        {
            return new BehviourDTO(this.DatabaseID, this.Description, this.Type);
        }
    }
}