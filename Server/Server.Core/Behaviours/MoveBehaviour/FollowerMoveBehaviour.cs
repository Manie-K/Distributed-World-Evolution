namespace Server.Core.Behaviours.MoveBehaviour
{
    /// <summary>
    /// Follower move behaviour, which copies a random entity's movement.
    /// </summary>
    public class FollowerMoveBehaviour : MoveBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 105;

        /// <inheritdoc/>
        public override string Description => "Copies random entity's movement.";

        /// <inheritdoc/>
        public override (int, int) GetNextMovement(WorldEntity entity, Span<WorldEntity> otherEntites)
        {
            int index = new Random().Next(0, otherEntites.ToArray().Length);

            var direction = otherEntites[index].State.LastMovementVector;
            return (direction.X, direction.Y);
        }

    }

}