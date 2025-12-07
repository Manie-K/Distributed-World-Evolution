using System.Collections.Immutable;
using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Services;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.MoveBehaviour
{
    /// <inheritdoc/>
    public class BasicMoveBehaviour : MoveBehaviourBase
    {
        /// <inheritdoc>
        public override int DatabaseID => 103;

        /// <inheritdoc>
        public override string Description => "Won't go to occupied tiles";

        /// <inheritdoc>
        public override (int, int) GetNextMovement(WorldEntity entity, Span<WorldEntity> otherEntites)
        {
            int x = new Random().Next(3) - 1; // -1, 0, 1
            int y = new Random().Next(3) - 1; // -1, 0, 1
            return (x, y);
        }
    }
}
