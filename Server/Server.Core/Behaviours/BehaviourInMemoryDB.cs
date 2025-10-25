using System.Reflection;

namespace Server.Core.Behaviours
{
    public class BehaviourInMemoryDB
    {
        public static BehaviourInMemoryDB Instance = new BehaviourInMemoryDB();

        private readonly Dictionary<int, Type> types;

        private BehaviourInMemoryDB()
        {
            // Initialize the in-memory database with all behaviour types.
            types = new Dictionary<int, Type>();

            List<Type> implementations = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass && typeof(IBehaviour).IsAssignableFrom(t))
                .ToList();

            foreach (var implementation in implementations)
            {
                var instance = Activator.CreateInstance(implementation);
                var id = (int?)implementation
                    .GetProperty("DatabaseID", BindingFlags.Public | BindingFlags.Instance)?
                    .GetValue(instance)
                    ?? throw new Exception($"Behaviour {implementation.FullName} does not have a valid DatabaseID.");

                types.Add(id, implementation);
            }
        }

        public Type? GetTypeByID(int id)
        {
            types.TryGetValue(id, out Type? value);
            return value;
        }

        public List<Type> GetAllTypes() 
        {
            return types.Values.ToList();
        }
    }
}