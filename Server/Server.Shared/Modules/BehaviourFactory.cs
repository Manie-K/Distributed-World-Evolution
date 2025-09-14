namespace Server.Shared.Modules
{
    public partial class BehaviourService
    {
        public class BehaviourFactory 
        {
            public BehaviourFactory()
            {

            } 

            public IBehaviour CreateBehaviourOfType(Type type)
            {
                return Activator.CreateInstance(type) as IBehaviour ??
                    throw new Exception($"Type {type.FullName} doesn't cast to IBehaviour");
            }
        }
    }
}