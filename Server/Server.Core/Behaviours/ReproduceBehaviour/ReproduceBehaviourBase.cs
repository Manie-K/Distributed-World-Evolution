using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    /// <summary>
    /// Reproduce behaviour base class, which provides base logic for reproduction behaviours.
    /// </summary>
    public abstract class ReproduceBehaviourBase : IBehaviour
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
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
            EntityTypeEnum entityType = entityModule.Type;

            ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj)
                && lobbyObj is ILobby l ? l :
                throw new ArgumentNullException("Lobby parameter is required for reproduction behaviour.");

            bool[][] walkableTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_WALKABLE_PARAM, out object? walkableTilesObj)
                && walkableTilesObj is bool[][] wt ? wt :
                throw new ArgumentNullException("Walkable tiles parameter is required for reproduction behaviour.");

            Position2D? position = null;
            
            int x = entity.State.Position.X;
            int y = entity.State.Position.Y;

            const short radius = 3;
            var possibleOffsets = new (short, short)[(radius*2 + 1) * (radius*2 + 1)]; // Looks for a free tile in an area around the entity ({radius} tile radius around entity).

            for (short dx = -radius; dx <= radius; dx++)
            {
                for (short dy = -radius; dy <= radius; dy++)
                {
                    possibleOffsets[(dx + radius) * radius + (dy + radius)] = (dx, dy);
                }
            }

            possibleOffsets = possibleOffsets.OrderBy(_ => new Random().Next()).ToArray();
            foreach (var (dx, dy) in possibleOffsets)
            {
                if(dx == 0 && dy == 0)
                    continue; // Skip the entity's current position.

                int newX = x + dx;
                int newY = y + dy;

                if (newX < 0 || newY < 0 || newX >= walkableTiles.Length || newY >= walkableTiles[0].Length)
                    continue;

                if (walkableTiles[newX][newY] && lobby.IsPositionFree(new Position2D(newX, newY)))
                {
                    position = new Position2D(newX, newY);
                    break; // Found a free position.
                }
            }

            if (position == null)
            {
                return; // Didn't find a free position, so we do NOT spawn a child.
            }

            WorldEntity child = WorldEntity.CreateWorldEntity(
                null,
                entity.ModuleID,
                new EntityState(
                    position,
                    entityModule.MaxHealth,
                    entityModule.MaxHunger,
                    10
                ),
                lobby
            );

            lobby.AddWorldEntity(child);
        }

        /// <inheritdoc/>
        public virtual bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID)?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            
            bool canReproduce = false;

            if (target == null)
            {
                return false;
            }

            if (entity.ModuleID == target.ModuleID && target != entity)
            {
                int randomRoll = new Random().Next(1, 61);
                canReproduce = randomRoll <= entityModule.ReproductionNeed;
            }

            bool[][]? walkableTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_WALKABLE_PARAM, out object? walkableTilesObj)
               && walkableTilesObj is bool[][] tiles ? tiles : null;

            if (walkableTiles?[entity.State.Position.X][entity.State.Position.Y] == false) // It's current entity position, not next simulated one, so this should never be false, but just in case.
            {
                return false; // Entity is on a non-walkable tile, so we skip reproduction attempt (don't reprodue while on water .... but shouldn't be on water in first place anyway).
            }

            return canReproduce;
        }

        /// <inheritdoc/>
        public BehaviourDTO ToDTO()
        {
            return new BehaviourDTO(DatabaseID, Description, Type, BehaviourInteractionTypeEnum.Reproduce);
        }

    }

}