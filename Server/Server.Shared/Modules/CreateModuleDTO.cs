namespace Server.Shared.Modules
{
    public class CreateModuleDTO 
    { 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public object Stats { get; set; }
        public List<int> BehaviourIDs { get; set; }
    }
}