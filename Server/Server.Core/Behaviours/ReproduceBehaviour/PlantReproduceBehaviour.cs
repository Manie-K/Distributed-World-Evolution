using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    public class PlantReproduceBehaviour : ReproduceBehaviourBase
    {
        //<inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;
        ///<inheritdoc/>
        public override int DatabaseID => 401;
        ///<inheritdoc/>
        public override string Description => "Plants reproduce behaviour according to their reproduction need.";
        ///<inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
            EntityTypeEnum entityType = entityModule.Type;

            ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj)
                && lobbyObj is ILobby l ? l :
                throw new ArgumentNullException("Lobby parameter is required for reproduction behaviour.");

            bool[][] walkableTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_WALKABLE_PARAM, out object? walkableTilesObj)
                && walkableTilesObj is bool[][] wt ? wt :
                throw new ArgumentNullException("Walkable tiles parameter is required for reproduction behaviour.");

            bool[][] fertileTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_FERTILE_PARAM, out object? fertileTilesObj)
              && fertileTilesObj is bool[][] ft ? ft :
              throw new ArgumentNullException("Fertile tiles parameter is required for plant reproduction behaviour.");

            Position2D? position = null;

            int x = entity.State.Position.X;
            int y = entity.State.Position.Y;

            //TODO: maybe better check this in CanExecute? @MaciejGórlaczyk
            if (walkableTiles[x][y] == false || fertileTiles[x][y] == false)
            {
                return; // Entity is on a non-walkable tile or on a non-fertile tile, so we skip reproduction attempt
            }

            var random = new Random();
            var possibleOffsets = new List<(int dx, int dy)>();

            for (int dx = -3; dx <= 3; dx++)
            {
                for (int dy = -3; dy <= 3; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }
                    possibleOffsets.Add((dx, dy));
                }
            }
            possibleOffsets = possibleOffsets.OrderBy(_ => random.Next()).ToList();

            foreach (var (dx, dy) in possibleOffsets)
            {
                int newX = x + dx;
                int newY = y + dy;

                if (newX < 0 || newY < 0 || newX >= walkableTiles.Length || newY >= walkableTiles[0].Length)
                    continue;

                if (walkableTiles[newX][newY] && fertileTiles[newX][newY] && lobby.IsPositionFree(new Position2D(newX, newY)))
                {
                    position = new Position2D(newX, newY);
                    break; // Found a free position
                }
            }

            if (position == null)
            {
                return; // Not fund a free position, so we do not spawn a child
            }

            WorldEntity child = WorldEntity.CreateWorldEntity(
                null,
                entity.ModuleID,
                new EntityState(
                    position: position,
                    health: entityModule.MaxHealth,
                    hunger: entityModule.MaxHunger
                ),
                lobby
            );

            lobby.AddWorldEntity(child);
        }
        ///<inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            EntityTypeEnum entityType = entityModule.Type;

            bool canReproduce = false;

            if (entityType == EntityTypeEnum.Plant)
            {
                Random random = new Random();
                int chance = random.Next(1, 11);
                canReproduce = chance <= entityModule.ReproductionNeed;
            }

            return canReproduce;
        }

    }
}
