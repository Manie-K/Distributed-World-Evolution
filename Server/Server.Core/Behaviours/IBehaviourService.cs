namespace Server.Core.Behaviours
{
    public interface IBehaviourService
    {
        public IBehaviour GetBehaviourInstanceByID(int databaseID);
        public IEnumerable<IBehaviour> GetAllBehaviours();
    }
}