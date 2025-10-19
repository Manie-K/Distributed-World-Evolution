using Server.Core.Behaviours;
using Server.Core.Services;

namespace Server.Core.Services
{
    public class BehaviourService : IBehaviourService
    {
        public static IBehaviourService Instance = new BehaviourService(); //TODO: Dependency Injection

        public IEnumerable<IBehaviour> GetAllBehaviours()
        {
            IEnumerable<IBehaviour> behaviours = new List<IBehaviour>();

            BehaviourInMemoryDB.Instance.GetAllTypes().ForEach(type =>
            {
                IBehaviour behaviour = BehaviourFactory.Instance.CreateBehaviourOfType(type);
                behaviours = behaviours.Append(behaviour);
            });

            return behaviours;
        }

        public IBehaviour GetBehaviourInstanceByID(int id)
        {
            Type? type = BehaviourInMemoryDB.Instance.GetTypeByID(id) ?? 
                throw new ArgumentException($"No behaviour found with ID {id}");

            return BehaviourFactory.Instance.CreateBehaviourOfType(type);
        }
    }
}