namespace Server.Shared.Modules
{
    public partial class BehaviourService : IBehaviourService
    {
        public IBehaviour GetBehaviourInstanceByID(int databaseID)
        {
            //Here we will connect to database, for now we will have dictionary in memory.
            var inMemoryDB = new BehaviourInMemoryDB();
            Type? type = inMemoryDB.GetTypeByID(databaseID) ?? 
                throw new ArgumentException($"No behaviour found with DatabaseID {databaseID}");

            return new BehaviourFactory().CreateBehaviourOfType(type);
        }
    }
}