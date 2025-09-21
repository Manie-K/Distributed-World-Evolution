namespace SharedLibrary.DTOs.ModuleDTO
{
    public class CreateModuleDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Version { get; init; }
        public string Author { get; init; }
        public object Stats { get; init; }
        public List<int> BehaviourIDs { get; init; }

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