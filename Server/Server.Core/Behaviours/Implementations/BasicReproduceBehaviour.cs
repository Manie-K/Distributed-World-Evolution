using Server.Core.Helpers;
using Server.Core.Lobby;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.Implementations
{
    public class BasicReproduceBehaviour : ReproduceBehaviourBase
    {
        public override int DatabaseID => 69;

        public override string Description => "Basic reproduction behaviour that creates a child entity";

        public override bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            return target != null && entity.ModuleID == target.ModuleID && target != entity;
        }

        public override void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            WorldEntity child = WorldEntity.CreateWorldEntity(
                name: $"{entity.Name}-child",
                moduleId: entity.ModuleID,
                state: new EntityState(
                    health: entity.State.Health / 2,
                    position: new Position2D(entity.State.Position.X, entity.State.Position.Y),
                    hunger: 10,
                    interactionFramesLeft: 5
                )
            );

            ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj) && lobbyObj is ILobby l ? l : throw new ArgumentNullException("Lobby parameter is required for reproduction behaviour.");

            lobby.AddWorldEntity(child);
        }
    }
}
