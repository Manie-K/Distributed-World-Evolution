namespace Server.Shared.Modules
{
    public interface IBehaviourService
    {
        public IBehaviour GetBehaviourInstanceByID(int databaseID);
    }
}