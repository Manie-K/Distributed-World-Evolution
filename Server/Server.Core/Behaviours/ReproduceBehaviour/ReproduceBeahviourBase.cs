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
        public virtual void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");
            EntityTypeEnum entityType = entityModule.Type;

            //TODO: find position to spawn a new entity
            Position2D position;
            if (entityType == EntityTypeEnum.Plant)
            {
                position = new Position2D(entity.State.Position.X, entity.State.Position.Y);
            }
            else
            {
                position = new Position2D(entity.State.Position.X, entity.State.Position.Y);
            }

            WorldEntity child = WorldEntity.CreateWorldEntity(
                name: null,
                moduleId: entity.ModuleID,
                state: new EntityState(
                    health: entityModule.MaxHealth,
                    position: position,
                    hunger: entityModule.MaxHunger,
                    interactionFramesLeft: 5
                )
            );

            ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj) && lobbyObj is ILobby l ? l : throw new ArgumentNullException("Lobby parameter is required for reproduction behaviour.");

            lobby.AddWorldEntity(child);
        }

        /// <inheritdoc/>
        public virtual bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule= moduleService.GetModuleById(entity.ModuleID)?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            EntityTypeEnum entityType = entityModule.Type;
            
            bool canReproduce = false;

            if (target == null && entityType == EntityTypeEnum.Plant)
            {
                canReproduce = true;
            }
            else if (target != null)
            {
                EntityTypeEnum targetType = moduleService.GetModuleById(target.ModuleID)?.Type ?? throw new Exception($"Module with ID={target.ModuleID} not found");
                canReproduce = entity.ModuleID == target.ModuleID && target != entity;
            }

            if (canReproduce)
            {
                Random random = new Random();
                int chance = random.Next(1, 11); 
                canReproduce = chance <= entityModule.ReproductionNeed;
            }

            return canReproduce;
        }

        /// <inheritdoc/>
        public BehviourDTO ToDTO()
        {
            return new BehviourDTO(DatabaseID, Description, Type);
        }
    }
}