namespace SharedLibrary.DTOs.ModuleDTO
{
    /// <summary>
    /// Used for displaying information in Client.
    /// </summary>
    public class ModuleDTO
    {
        public int DatabaseID { get; init; }
        public string Name { get; init; }
        public bool IsOfficialModule { get; init; }
        public string Author { get; init; }
        public object Stats { get; init; }
        public List<BehviourDTO> Behaviours { get; init; }

        public ModuleDTO(int databaseID, string name, bool official, string author, object stats, List<BehviourDTO> behaviours)
        {
            DatabaseID = databaseID;
            Name = name;
            IsOfficialModule = official;
            Author = author;
            Stats = stats;
            Behaviours = behaviours;
        }
    }
}