using System.Collections.Immutable;
using Server.Core.Helpers;
using Server.Core.Lobby;
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
        public virtual void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (otherParams?.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) == true && value is Position2D nextPosition)
            {
                if (otherParams.TryGetValue(CustomBehaviourParams.ENTITIES_MAP_PARAM, out object? entitiesMapObj)
                    && entitiesMapObj is Dictionary<(int, int), WorldEntity?> entitiesMap)
                {
                    if (otherParams.TryGetValue(CustomBehaviourParams.ENTITIES_MAP_LOCK_PARAM, out object? entitiesMapLock) && entitiesMapLock != null)
                    {
                        lock (entitiesMapLock)
                        {
                            entitiesMap[(entity.State.Position.X, entity.State.Position.Y)] = null;
                            entitiesMap[(nextPosition.X, nextPosition.Y)] = entity;
                            entity.State.Position = nextPosition;
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (otherParams == null)
            {
                Console.WriteLine("[MoveBehaviour] otherParams is null, cannot determine movement validity.");
                return false;
            }

            Position2D nextPos = otherParams.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) && value is Position2D pos
                ? pos : entity.State.Position;

            bool[][]? walkableTiles = otherParams.TryGetValue(CustomBehaviourParams.MAP_WALKABLE_PARAM, out object? walkableTilesObj)
                && walkableTilesObj is bool[][] tiles ? tiles : null;

            ILobby lobby = otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj)
                && lobbyObj is ILobby l ? l : throw new ArgumentNullException("Lobby parameter is required for AvoiderMoveBehaviour");

            if (walkableTiles == null || lobby == null 
                || nextPos.X < 0 || nextPos.Y < 0 
                || nextPos.X >= walkableTiles.Length || nextPos.Y >= walkableTiles[nextPos.X].Length)
            {
                return false;
            }

            if (!walkableTiles[nextPos.X][nextPos.Y]) return false;
            if (!lobby.IsPositionFree(nextPos)) return false;

            return true;
        }

        /// TODO: Optimize if needed
        /// <summary>
        /// Returns the next movement vector as (x, y).
        /// </summary>
        /// <param name="entity"> Entity </param>
        /// <param name="otherEntites"> Other world entites </param>
        /// <returns> Next movement candidate vector (x,y) </returns>       
        /// <remarks> 
        /// If the creation of immutable list from normal list and then traversing through all world entites will be too time consuming,
        /// we should change the data structure to something more optimized, 2d spatial map etc.Marked it as possible TODO.    
        /// </remarks>
        public abstract (int, int) GetNextMovement(WorldEntity entity, Span<WorldEntity> otherEntites);

        /// <inheritdoc/>
        public BehaviourDTO ToDTO()
        {
            return new BehaviourDTO(DatabaseID, Description, Type, BehaviourInteractionTypeEnum.Move);
        }
    }
}
