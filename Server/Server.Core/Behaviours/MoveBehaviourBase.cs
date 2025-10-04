using Server.Core.Helpers;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours
{
    public abstract class MoveBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal | EntityTypeEnum.Human;


        /// <inheritdoc/>
        public virtual void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            if(otherParams?.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) == true && value is Position2D nextPosition)
            {
                entity.State.Position = nextPosition;
            }
        }

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

        public abstract (int, int) GetNextMovement(WorldEntity entity);
    }
}
