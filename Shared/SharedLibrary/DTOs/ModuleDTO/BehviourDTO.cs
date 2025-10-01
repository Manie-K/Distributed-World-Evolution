namespace SharedLibrary.DTOs.ModuleDTO
{
    public class BehviourDTO
    {
        public int DatabaseID { get; init; }
        public string Description { get; init; }

        public BehviourDTO(int databaseID, string description)
        {
            DatabaseID = databaseID;
            Description = description;
        }
    }
}