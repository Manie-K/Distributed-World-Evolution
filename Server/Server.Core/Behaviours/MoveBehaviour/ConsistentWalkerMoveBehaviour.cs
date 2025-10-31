using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Services;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.MoveBehaviour
{
    public class ConsistentWalkerMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => 102;
        public override string Description => "Consistent move behaviour, moves in a set direction with 80% consistency";
        public override (int, int) GetNextMovement(WorldEntity entity)
        {
            Position2D lastMovementVector = entity.State.LastMovementVector ?? new Position2D(0, 0);
            int x = 0, y = 0;

            bool shouldContinueSameDirection = new Random().NextDouble() < 0.8;

            if (shouldContinueSameDirection && (lastMovementVector.X != 0 || lastMovementVector.Y != 0))
            {
                x = lastMovementVector.X;
                y = lastMovementVector.Y;
            }
            else
            {
                while (x == 0 && y == 0)
                {
                    x = new Random().Next(3) - 1; // -1, 0, 1
                    y = new Random().Next(3) - 1; // -1, 0, 1
                }
            }

            return (x, y);
        }
    }

    public class AvoiderMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => 103;

        public override string Description => "Won't go to occupied tiles";

        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (!base.CanExecute(entity, target, moduleService, otherParams)) return false;

            Position2D nextPos = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) && value is Position2D pos
                ? pos : entity.State.Position;

            ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj) 
                && lobbyObj is ILobby l ? l : 
                    throw new ArgumentNullException("Lobby parameter is required for AvoiderMoveBehaviour");

            if(!lobby.IsPositionFree(nextPos))
            {
                return false;
            }

            return true;
        }

        public override (int, int) GetNextMovement(WorldEntity entity)
        {
            int x = new Random().Next(3) - 1; // -1, 0, 1
            int y = new Random().Next(3) - 1; // -1, 0, 1
            return (x, y);
        }
    }
}
