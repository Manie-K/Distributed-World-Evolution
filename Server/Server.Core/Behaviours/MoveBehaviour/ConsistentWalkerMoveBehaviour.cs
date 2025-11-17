using System.Collections.Immutable;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.MoveBehaviour
{
    public class ConsistentWalkerMoveBehaviour : MoveBehaviourBase
    {
        /// <inheritdoc>
        public override int DatabaseID => 102;

        /// <inheritdoc>
        public override string Description => "Consistent move behaviour, moves in a set direction with 80% consistency";

        /// <inheritdoc>
        public override (int, int) GetNextMovement(WorldEntity entity, ImmutableList<WorldEntity> otherEntites)
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
}
