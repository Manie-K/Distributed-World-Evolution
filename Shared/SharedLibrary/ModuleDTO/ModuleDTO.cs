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
    }
}