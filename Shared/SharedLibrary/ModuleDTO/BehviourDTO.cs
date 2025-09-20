namespace SharedLibrary
{
    public class BehviourDTO 
    { 
        public int DatabaseID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BehviourDTO(int databaseID, string name, string description)
        {
            DatabaseID = databaseID;
            Name = name;
            Description = description;
        }
    }
}