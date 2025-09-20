namespace SharedLibrary
{
    public class CreateModuleDTO 
    { 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public object Stats { get; set; }
        public List<int> BehaviourIDs { get; set; }

        public CreateModuleDTO(string name, string description, string version, string author, object stats, List<int> behaviourIDs)
        {
            Name = name;
            Description = description;
            Version = version;
            Author = author;
            Stats = stats;
            BehaviourIDs = behaviourIDs;
        }
    }
}