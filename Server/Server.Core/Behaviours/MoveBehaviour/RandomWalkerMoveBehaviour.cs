using System.Collections.Immutable;
using System.Numerics;
using Server.Core.Helpers;
using Server.Core.Services;

namespace Server.Core.Behaviours.MoveBehaviour
{

    /// <inheritdoc>
    public class RandomWalkerMoveBehaviour : MoveBehaviourBase
    {
        /// <inheritdoc>
        public override int DatabaseID => 101;

        /// <inheritdoc>
        public override string Description => "Basic move behaviour, random 8-sided movement";
        
        /// <inheritdoc>
        public override (int, int) GetNextMovement(WorldEntity entity, ImmutableList<WorldEntity> otherEntites)
        {
            int x = 0, y = 0;
            while (x == 0 && y == 0)
            {
                x = new Random().Next(3) - 1; // -1, 0, 1
                y = new Random().Next(3) - 1; // -1, 0, 1
            }
            return (x, y);
        }
    }
}