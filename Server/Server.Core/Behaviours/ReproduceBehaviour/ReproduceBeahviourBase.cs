using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    public abstract class ReproduceBehaviourBase : IBehaviour
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
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
            EntityTypeEnum entityType = entityModule.Type;

            ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj)
                && lobbyObj is ILobby l ? l :
                throw new ArgumentNullException("Lobby parameter is required for reproduction behaviour.");

            bool[][] walkableTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_WALKABLE_PARAM, out object? walkableTilesObj)
                && walkableTilesObj is bool[][] wt ? wt :
                throw new ArgumentNullException("Walkable tiles parameter is required for reproduction behaviour.");

            Position2D? position = null;
            
            int loopSafetyCounter = 0;
            while (loopSafetyCounter < 100)
            {
                loopSafetyCounter++;
                int x = entity.State.Position.X;
                int y = entity.State.Position.Y;

                if (walkableTiles[x][y] == false)
                {
                    continue; //Entity is on a non-walkable tile, we skip reproduction attempt
                }

                x = new Random().Next(2) == 0 ? x + new Random().Next(4) : x - new Random().Next(4);
                y = new Random().Next(2) == 0 ? y + new Random().Next(4) : y - new Random().Next(4);

                if(lobby.IsPositionFree(new Position2D(x, y)))
                {
                    position = new Position2D(x, y);
                    break;
                }
            }

            if (position == null)
            {
                return; //We didn't find a free position, so we do not spawn a child
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

        /// <inheritdoc/>
        public virtual bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID)?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            
            bool canReproduce = false;

            if (entity.ModuleID == target.ModuleID && target != entity)
            {
                Random random = new Random();
                int chance = random.Next(1, 11); 
                canReproduce = chance <= entityModule.ReproductionNeed;
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