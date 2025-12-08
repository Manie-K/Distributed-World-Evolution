using Server.Core.Behaviours;

namespace Server.Core.Services
{
    /// <summary>
    /// Interface for behaviour service to manage behaviours.
    /// </summary>
    public interface IBehaviourService
    {
        /// <summary>
        /// Retrieves a behaviour instance by its database ID.
        /// </summary>
        /// <param name="id"> The ID of the behaviour. </param>
        /// <returns> The behaviour instance. </returns>
        public IBehaviour GetBehaviourInstanceByID(int id);

        /// <summary>
        /// Retrieves all available behaviours.
        /// </summary>
        /// <returns> A collection of all behaviours. </returns>
        public IEnumerable<IBehaviour> GetAllBehaviours();
    
    }

}