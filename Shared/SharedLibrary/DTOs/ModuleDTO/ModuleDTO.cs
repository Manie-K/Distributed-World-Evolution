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
        public int Damage { get; private set; }
        public int Aggresion { get; private set; }
        public int ReproductionNeed { get; private set; }
        public List<BehviourDTO> Behaviours { get; init; }

        public ModuleDTO(int databaseID, string name, bool official, int damage, int aggresion, int reproductionNeed, List<BehviourDTO> behaviours)
        {
            DatabaseID = databaseID;
            Name = name;
            IsOfficialModule = official;
            Damage = damage;
            Aggresion = aggresion;
            ReproductionNeed = reproductionNeed;
            Behaviours = behaviours;
        }
    }
}