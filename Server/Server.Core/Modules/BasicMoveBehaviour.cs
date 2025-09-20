using System.Numerics;

namespace Server.Core.Modules
{
    public class BasicMoveBehaviour : IMoveBehaviour
    {
        public int DatabaseID => 222;

        public void Move(WorldEntity entity, Vector2 newPosition, object map)
        {
            entity.State.Position = newPosition;
        }

    }
}
