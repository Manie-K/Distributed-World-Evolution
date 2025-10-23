using Server.Core;

namespace SharedLibrary.DTOs.ModuleDTO
{
    public class CreateModuleDTO
    {
        public string Name { get; init; }
        public bool Official { get; init; }
        public int Damage { get; private set; }
        public int Agression { get; private set; }
        public int ReproductionNeed { get; private set; }
        public EntityTypeEnum Type { get; init; }
        public int GraphicalRepresentationID { get; set; }
        public List<int> BehaviourIDs { get; init; }

        public CreateModuleDTO(string name, bool official, int damage, int aggresion, int reproductionNeed, EntityTypeEnum type, int graphicsID, List<int> behaviourIDs)
        {
            Name = name;
            Official = official;
            Damage = damage;
            Agression = aggresion;
            ReproductionNeed = reproductionNeed;
            Type = type;
            GraphicalRepresentationID = graphicsID;
            BehaviourIDs = behaviourIDs;
        }
    }
}