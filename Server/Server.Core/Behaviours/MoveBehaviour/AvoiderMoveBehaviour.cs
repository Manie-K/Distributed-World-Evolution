using System.Collections.Immutable;
using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Services;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.MoveBehaviour
{
    /// <inheritdoc/>
    public class AvoiderMoveBehaviour : MoveBehaviourBase
    {
        /// <inheritdoc>
        public override int DatabaseID => 103;

        /// <inheritdoc>
        public override string Description => "Won't go to occupied tiles";

        /// <inheritdoc>
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

        /// <inheritdoc>
        public override (int, int) GetNextMovement(WorldEntity entity, ImmutableList<WorldEntity> otherEntites)
        {
            int x = new Random().Next(3) - 1; // -1, 0, 1
            int y = new Random().Next(3) - 1; // -1, 0, 1
            return (x, y);
        }
    }
}
