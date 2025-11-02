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
        public int MaxHunger { get; private set; }
        public int MaxHealth { get; private set; }
        public EntityTypeEnum Type { get; init; }
        public int GraphicalRepresentationID { get; set; }
        public List<int> BehaviourIDs { get; init; }

        public CreateModuleDTO(string name, bool official, int damage, int aggresion, int reproductionNeed, 
            int maxHunger, int maxHealth, EntityTypeEnum type, int graphicalRepresentationID, List<int> behaviourIDs)
        {
            Name = name;
            Official = official;
            Damage = damage;
            Agression = aggresion;
            ReproductionNeed = reproductionNeed;
            MaxHunger = maxHunger;
            MaxHealth = maxHealth;
            Type = type;
            GraphicalRepresentationID = graphicalRepresentationID;
            BehaviourIDs = behaviourIDs;
        }
    }
}