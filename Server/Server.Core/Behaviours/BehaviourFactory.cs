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
            return Activator.CreateInstance(type) as IBehaviour ??
                throw new Exception($"Type {type.FullName} doesn't cast to IBehaviour");
        }
    }
}