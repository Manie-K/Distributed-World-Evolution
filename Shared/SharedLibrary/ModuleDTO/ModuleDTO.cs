namespace SharedLibrary
{
    /// <summary>
    /// Used for displaying information in Client.
    /// </summary>
    public class ModuleDTO 
    { 
        public int DatabaseID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public object Stats { get; set; }
        public List<BehviourDTO> Behaviours { get; set; }

        public ModuleDTO(int databaseID, string name, string version, string author, object stats, List<BehviourDTO> behaviours)
        {
            DatabaseID = databaseID;
            Name = name;
            Version = version;
            Author = author;
            Stats = stats;
            Behaviours = behaviours;
        }
    }
}