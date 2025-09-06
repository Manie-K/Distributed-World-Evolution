namespace Server.Shared.Modules
{
    public class ModuleData
    {
        // We should try to place non-dynamic data here. All the dynamic data will be stored in WorldEntity object instances.
        public string Name { get; init; }
        public string Version { get; init; }
        public string Author { get; init; }
        public ModuleData(string name, string version, string author)
        {
            Name = name;
            Version = version;
            Author = author;
        }
    }
}