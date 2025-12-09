using Server.Core.Behaviours;

namespace Server.Core.Services
{
    /// <summary>
    /// Singleton class to manage behaviours.
    /// </summary>
    public class BehaviourService : IBehaviourService
    {
        /// <summary>
        /// Instance of the BehaviourService singleton.
        /// </summary>
        public static IBehaviourService Instance = new BehaviourService();


        /// Private constructor to enforce singleton pattern.
        private BehaviourService()
        {

        }

        /// <inheritdoc/>
        public IEnumerable<IBehaviour> GetAllBehaviours()
        {
            IEnumerable<IBehaviour> behaviours = BehaviourInMemoryDB.Instance.GetAllInstances();
            return behaviours;
        }

        /// <inheritdoc/>
        public IBehaviour GetBehaviourInstanceByID(int id)
        {
            IBehaviour? instance = BehaviourInMemoryDB.Instance.GetInstanceByID(id) ?? 
                throw new ArgumentException($"No behaviour found with ID {id}");

            return instance;
        }

    }

}