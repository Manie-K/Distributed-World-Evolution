namespace Server.Core.Behaviours.MoveBehaviour
{
    /// <summary>
    /// Basic move behaviour, which randomly moves the entity by -1, 0, or 1 in both x and y directions.
    /// </summary>
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