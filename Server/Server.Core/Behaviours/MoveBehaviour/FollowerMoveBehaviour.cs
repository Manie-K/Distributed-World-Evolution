namespace Server.Core.Behaviours.MoveBehaviour
{
    public class FollowerMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => 105;
        public override string Description => "Copies random entity's movement.";
        public override (int, int) GetNextMovement(WorldEntity entity, Span<WorldEntity> otherEntites)
        {
            int index = new Random().Next(0, otherEntites.ToArray().Length);

            var direction = otherEntites[index].State.LastMovementVector;
            return (direction.X, direction.Y);
        }
    }
}
