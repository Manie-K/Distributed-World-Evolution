using Server.Core.Behaviours;

namespace Server.Core.Services
{
    public interface IBehaviourService
    {
        public IBehaviour GetBehaviourInstanceByID(int id);
        public IEnumerable<IBehaviour> GetAllBehaviours();
    }
}