namespace Server.Core.Modules
{
    public interface IBehaviourService
    {
        public IBehaviour GetBehaviourInstanceByID(int databaseID);
    }
}