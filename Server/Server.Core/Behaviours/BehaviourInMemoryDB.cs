using System.Reflection;

namespace Server.Core.Behaviours
{
    public class BehaviourInMemoryDB
    {
        public static BehaviourInMemoryDB Instance = new BehaviourInMemoryDB();

        private readonly Dictionary<int, IBehaviour> behaviourInstances;

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

        public IBehaviour? GetInstanceByID(int id)
        {
            behaviourInstances.TryGetValue(id, out IBehaviour? value);
            return value;
        }

        public List<IBehaviour> GetAllInstances() 
        {
            return behaviourInstances.Values.ToList();
        }
    }
}