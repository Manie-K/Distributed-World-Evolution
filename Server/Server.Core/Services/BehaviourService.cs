using Server.Core.Behaviours;

namespace Server.Core.Services
{
    public class BehaviourService : IBehaviourService
    {
        public static IBehaviourService Instance = new BehaviourService();
        private BehaviourService(){}

        private BehaviourService()
        {
        }

        public IEnumerable<IBehaviour> GetAllBehaviours()
        {
            IEnumerable<IBehaviour> behaviours = BehaviourInMemoryDB.Instance.GetAllInstances();
            return behaviours;
        }

        public IBehaviour GetBehaviourInstanceByID(int id)
        {
            IBehaviour? instance = BehaviourInMemoryDB.Instance.GetInstanceByID(id) ?? 
                throw new ArgumentException($"No behaviour found with ID {id}");

            return instance;
        }
    }
}