using Server.Core;

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
        public List<BehaviourDTO> Behaviours { get; init; }
        public EntityTypeEnum Type { get; init; }
        public int GraphicalRepresentationID { get; set; }

        public ModuleDTO(int databaseID, string name, bool isOfficialModule, int damage, int aggresion, int reproductionNeed, List<BehaviourDTO> behaviours, EntityTypeEnum type, int graphicalRepresentationID)
        {
            DatabaseID = databaseID;
            Name = name;
            IsOfficialModule = isOfficialModule;
            Damage = damage;
            Aggresion = aggresion;
            ReproductionNeed = reproductionNeed;
            Behaviours = behaviours;
            Type = type;
            GraphicalRepresentationID = graphicalRepresentationID;
        }
    }
}