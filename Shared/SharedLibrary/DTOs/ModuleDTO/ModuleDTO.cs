using Server.Core;

namespace SharedLibrary.DTOs.ModuleDTO
{
    /// <summary>
    /// DTO representing a module.
    /// </summary>
    public class ModuleDTO
    {
        /// <summary>
        /// Database ID of the module.
        /// </summary>
        public int DatabaseID { get; init; }

        /// <summary>
        /// Name of the module.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Determines if the module is official.
        /// </summary>
        public bool IsOfficialModule { get; init; }

        /// <summary>
        /// Damage value of the module.
        /// </summary>
        public int Damage { get; init; }

        /// <summary>
        /// Aggresion level of the module.
        /// </summary>
        public int Aggresion { get; init; }

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
        /// Behaviours of the module.
        /// </summary>
        public IEnumerable<BehaviourDTO> Behaviours { get; init; }

        /// <summary>
        /// Type of the module.
        /// </summary>
        public EntityTypeEnum Type { get; init; }

        /// <summary>
        /// ID of the graphical representation of the module.
        /// </summary>
        public int GraphicalRepresentationID { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="databaseID"> Database ID of the module. </param>   
        /// <param name="name"> Name of the module. </param>    
        /// <param name="isOfficialModule"> Determines if the module is official. </param>
        /// <param name="damage"> Damage value of the module. </param>
        /// <param name="aggresion"> Aggresion level of the module. </param>
        /// <param name="reproductionNeed"> Reproduction need of the module. </param>
        /// <param name="maxHunger"> Maximum hunger of the module. </param>
        /// <param name="maxHealth"> Maximum health of the module. </param>
        /// <param name="behaviours"> Behaviours of the module. </param>
        /// <param name="type"> Type of the module. </param>
        /// <param name="graphicalRepresentationID"> ID of the graphical representation of the module. </param>
        public ModuleDTO(int databaseID, string name, bool isOfficialModule, int damage, int aggresion, int reproductionNeed, int maxHunger, int maxHealth, IEnumerable<BehaviourDTO> behaviours, EntityTypeEnum type, int graphicalRepresentationID)
        {
            DatabaseID = databaseID;
            Name = name;
            IsOfficialModule = isOfficialModule;
            Damage = damage;
            Aggresion = aggresion;
            ReproductionNeed = reproductionNeed;
            MaxHunger = maxHunger;
            MaxHealth = maxHealth;
            Behaviours = behaviours;
            Type = type;
            GraphicalRepresentationID = graphicalRepresentationID;
        }

    }

}