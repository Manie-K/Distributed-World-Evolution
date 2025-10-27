namespace Server.Core.Behaviours
{
    public class BehaviourFactory 
    {
        public static BehaviourFactory Instance = new BehaviourFactory();

        private BehaviourFactory()
        {
        }

        public IBehaviour CreateBehaviourOfType(Type type)
        {
            try
            {
                var instance = Activator.CreateInstance(type) as IBehaviour ?? throw new Exception($"Type {type.FullName} doesn't cast to IBehaviour");
                return instance;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error creating behaviour of type {type.FullName}: {ex.Message}");
                throw;
            }
        }
    }
}