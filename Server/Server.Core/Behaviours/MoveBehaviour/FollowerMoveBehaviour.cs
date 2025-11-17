using System.Collections.Immutable;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.MoveBehaviour
{
    public class FollowerMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => 105;
        public override string Description => "Moves the entity towards an entity, if one is present on neighbouring tiles.";
        public override (int, int) GetNextMovement(WorldEntity entity, ImmutableList<WorldEntity> otherEntites)
        {
            foreach (WorldEntity ent in otherEntites)
            {
                if(ent.Id == entity.Id) continue;
                
                Position2D entityPos = entity.State.Position;
                int deltaX = ent.State.Position.X - entityPos.X;
                int deltaY = ent.State.Position.Y - entityPos.Y;

                if (deltaX >= -1 && deltaX <= 1 && deltaY >= -1 && deltaY <= 1)
                {
                    return (deltaX, deltaY); //Diagonal movement allowed
                }
            }

            // No nearby entity found, move randomly
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
