using Server.Core.Helpers;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.MoveBehaviour
{
    /// <inheritdoc>
    public abstract class MoveBehaviourBase : IBehaviour
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
            if (otherParams?.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) == true && value is Position2D nextPosition)
            {
                entity.State.Position = nextPosition;
            }
        }

        /// <inheritdoc/>
        public virtual bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Position2D nextPos = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) && value is Position2D pos
                ? pos : entity.State.Position;

            bool[][]? walkableTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_PARAM, out object? walkableTilesObj)
                && walkableTilesObj is bool[][] tiles ? tiles : null;

            if (walkableTiles == null || nextPos.X < 0 || nextPos.Y < 0 || nextPos.X >= walkableTiles.Length || nextPos.Y >= walkableTiles[nextPos.X].Length)
            {
                return false;
            }

            return walkableTiles[nextPos.X][nextPos.Y];
        }

        /// <summary>
        /// Returns the next movement vector as (x, y).
        /// </summary>
        /// <param name="entity"> Entity </param>
        /// <returns> Next movement candidate vector (x,y) </returns>       
        public abstract (int, int) GetNextMovement(WorldEntity entity);

        /// <inheritdoc/>
        public BehaviourDTO ToDTO()
        {
            return new BehaviourDTO(DatabaseID, Description, Type, BehaviourInteractionTypeEnum.Move);
        }
    }
}
