using System.Reflection;

namespace Server.Core.Behaviours
{
    /// <summary>
    /// Defines an in-memory database for storing and retrieving behaviour instances.
    /// </summary>  
    public class BehaviourInMemoryDB
    {
        /// Instance of the BehaviourInMemoryDB singleton.
        public static BehaviourInMemoryDB Instance = new BehaviourInMemoryDB();

        /// Behaviour instances stored in the in-memory database, keyed by their DatabaseID.
        private readonly Dictionary<int, IBehaviour> behaviourInstances;

        /// Private constructor.
        private BehaviourInMemoryDB()
        {
            // Initialize the in-memory database with all behaviours.
            behaviourInstances = new Dictionary<int, IBehaviour>();

            List<Type> types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass && typeof(IBehaviour).IsAssignableFrom(t))
                .ToList();

            foreach (var implementingType in types)
            {
                try
                {
                    IBehaviour behaviourInstance = Activator.CreateInstance(implementingType) as IBehaviour ?? throw new Exception($"Type {implementingType.FullName} doesn't cast to IBehaviour");
                    int id = (int?)implementingType
                        .GetProperty("DatabaseID", BindingFlags.Public | BindingFlags.Instance)?
                        .GetValue(behaviourInstance)
                        ?? throw new Exception($"Behaviour {implementingType.FullName} does not have a valid DatabaseID field.");

                    behaviourInstances.Add(id, behaviourInstance);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error instantiating behaviour type {implementingType.FullName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Retrieves a behaviour instance by its DatabaseID.
        /// </summary>
        /// <param name="id"> The ID of the behaviour to retrieve. </param>
        /// <returns> The behaviour instance if found; otherwise, null. </returns>
        public IBehaviour? GetInstanceByID(int id)
        {
            behaviourInstances.TryGetValue(id, out IBehaviour? value);
            return value;
        }

        /// <summary>
        /// Retrieves all behaviour instances stored in the in-memory database.
        /// </summary>
        /// <returns> A list of all behaviour instances. </returns>
        public List<IBehaviour> GetAllInstances() 
        {
            return behaviourInstances.Values.ToList();
        }

    }

}