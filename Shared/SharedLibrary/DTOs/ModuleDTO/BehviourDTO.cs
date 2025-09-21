namespace SharedLibrary.DTOs.ModuleDTO
{
    public class BehviourDTO
    {
        public int DatabaseID { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        public BehviourDTO(int databaseID, string name, string description)
        {
            DatabaseID = databaseID;
            Name = name;
            Description = description;
        }
    }
}