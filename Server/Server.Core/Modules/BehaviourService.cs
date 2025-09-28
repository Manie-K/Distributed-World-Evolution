namespace Server.Core.Modules
{
    public partial class BehaviourService : IBehaviourService
    {
        public static IBehaviourService Instance = new BehaviourService();

        public IBehaviour GetBehaviourInstanceByID(int databaseID)
        {
            //Here we will connect to database, for now we will have dictionary in memory.
            var inMemoryDB = new BehaviourInMemoryDB();
            Type? type = inMemoryDB.GetTypeByID(databaseID) ?? 
                throw new ArgumentException($"No behaviour found with DatabaseID {databaseID}");

            return BehaviourFactory.Instance.CreateBehaviourOfType(type);
        }
    }
}