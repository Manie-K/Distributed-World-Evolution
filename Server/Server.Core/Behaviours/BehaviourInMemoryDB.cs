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

            foreach (var implementation in types)
            {
                try
                {
                    IBehaviour behaviourInstance = BehaviourFactory.CreateBehaviourOfType(implementation);
                    int id = (int?)implementation
                        .GetProperty("DatabaseID", BindingFlags.Public | BindingFlags.Instance)?
                        .GetValue(behaviourInstance)
                        ?? throw new Exception($"Behaviour {implementation.FullName} does not have a valid DatabaseID.");

                    behaviourInstances.Add(id, behaviourInstance);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing behaviour type {implementation.FullName}: {ex.Message}");
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

        private static class BehaviourFactory
        {
            public static IBehaviour CreateBehaviourOfType(Type type)
            {
                try
                {
                    var instance = Activator.CreateInstance(type) as IBehaviour ?? throw new Exception($"Type {type.FullName} doesn't cast to IBehaviour");
                    return instance;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating behaviour of type {type.FullName}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}