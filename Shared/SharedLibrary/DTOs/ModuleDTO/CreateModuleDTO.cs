namespace SharedLibrary.DTOs.ModuleDTO
{
    public class CreateModuleDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public int Damage { get; private set; }
        public int Aggresion { get; private set; }
        public int ReproductionNeed { get; private set; }
        public List<int> BehaviourIDs { get; init; }

        public CreateModuleDTO(string name, string description, int damage, int aggresion, int reproductionNeed, List<int> behaviourIDs)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Aggresion = aggresion;
            ReproductionNeed = reproductionNeed;
            BehaviourIDs = behaviourIDs;
        }
    }
}