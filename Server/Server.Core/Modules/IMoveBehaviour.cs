using Server.Shared;

namespace Server.Core.Modules
{
    public interface IMoveBehaviour : IBehaviour
    {
        //TODO: Implement map object
        public void Move(WorldEntity entity, object map);
    }
}
