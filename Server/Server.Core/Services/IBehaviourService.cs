using Server.Core.Behaviours;

namespace Server.Core.Services
{
    public interface IBehaviourService
    {
        //public IBehaviour GetBehaviourInstanceByID(int databaseID);
        public IEnumerable<IBehaviour> GetAllBehaviours();
    }
}