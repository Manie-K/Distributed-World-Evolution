using System.Numerics;
using Server.Core;

namespace Server.Core.Modules
{
    public interface IMoveBehaviour : IBehaviour
    {
        //TODO: Implement map object
        public void Move(WorldEntity entity, Vector2 newPosition, object map);
    }
}
