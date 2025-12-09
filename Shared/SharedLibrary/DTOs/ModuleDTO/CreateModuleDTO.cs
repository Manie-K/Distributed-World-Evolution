using Server.Core;

namespace SharedLibrary.DTOs.ModuleDTO
{
    /// <summary>
    /// DTO for creating a new Module
    /// </summary>
    public class CreateModuleDTO
    {
        /// <summary>
        /// Name of the module.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Determines if the module is official.
        /// </summary>
        public bool Official { get; init; }

        /// <summary>
        /// Damage value of the module.
        /// </summary>
        public int Damage { get; init; }

        /// <summary>
        /// Aggression level of the module.
        /// </summary>
        public int Aggression { get; init; }

        /// <summary>
        /// Reproduction need of the module.
        /// </summary>  
        public int ReproductionNeed { get; init; }

        /// <summary>
        /// Maximum hunger of the module.
        /// </summary>
        public int MaxHunger { get; init; }

        /// <summary>
        /// Maximum health of the module.
        /// </summary>
        public int MaxHealth { get; init; }

        /// <summary>
        /// Type of the module.
        /// </summary>
        public EntityTypeEnum Type { get; init; }

        /// <summary>
        /// ID of the graphical representation associated with the module.
        /// </summary>  
        public int GraphicalRepresentationID { get; init; }

        /// <summary>
        /// IDs of the behaviours associated with the module.
        /// </summary>
        public IEnumerable<int> BehaviourIDs { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"> Name of the module. </param>
        /// <param name="official"> Determines if the module is official. </param>
        /// <param name="damage"> Damage value of the module. </param>
        /// <param name="aggression"> Aggression level of the module. </param>
        /// <param name="reproductionNeed"> Reproduction need of the module. </param>
        /// <param name="maxHunger"> Maximum hunger of the module. </param>
        /// <param name="maxHealth"> Maximum health of the module. </param>
        /// <param name="type"> Type of the module. </param>
        /// <param name="graphicalRepresentationID"> ID of the graphical representation associated with the module. </param>
        /// <param name="behaviourIDs"> IDs of the behaviours associated with the module. </param>
        public CreateModuleDTO(string name, bool official, int damage, int aggression, int reproductionNeed, int maxHunger, int maxHealth, EntityTypeEnum type, int graphicalRepresentationID, IEnumerable<int> behaviourIDs)
        {
            Name = name;
            Official = official;
            Damage = damage;
            Aggression = aggression;
            ReproductionNeed = reproductionNeed;
            MaxHunger = maxHunger;
            MaxHealth = maxHealth;
            Type = type;
            GraphicalRepresentationID = graphicalRepresentationID;
            BehaviourIDs = behaviourIDs;
        }

    }

}