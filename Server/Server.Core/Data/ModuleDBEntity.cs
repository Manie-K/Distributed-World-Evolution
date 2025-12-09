using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Data
{
    /// <summary>
    /// Database entity representing a Module.
    /// Seperate class from Module.cs in case we want to change the DB schema without affecting the in-memory Module representation.
    /// </summary>
    public class ModuleDBEntity
    {
        /// <summary>
        /// ID of the module.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Determines if the module is official or user-created.
        /// </summary>
        public bool Official { get; set; }

        /// <summary>
        /// Name of the module.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Damage value of the module.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Aggression level of the module.
        /// </summary>
        public int Agression { get; set; }

        /// <summary>
        /// Reproduction need of the module.
        /// </summary>
        public int ReproductionNeed { get; set; }

        /// <summary>
        /// Maximum hunger value of the module.
        /// </summary>
        public int MaxHunger { get; set; }

        /// <summary>
        /// Maximum health value of the module.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Type of the entity.
        /// </summary>
        public EntityTypeEnum Type { get; set; }

        /// <summary>
        /// ID of the graphical representation associated with the module.
        /// </summary>
        public int GraphicalRepresentationID { get; set; }

        /// <summary>
        /// Behaviour IDs associated with the module.
        /// </summary>
        public int[] BehaviourIDs { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="official"> Determines if the module is official or user-created. </param>
        /// <param name="name"> Name of the module. </param>
        /// <param name="damage"> Damage value of the module. </param>
        /// <param name="agression"> Aggression level of the module. </param>
        /// <param name="reproductionNeed"> Reproduction need of the module. </param>
        /// <param name="maxHunger"> Maximum hunger value of the module. </param>
        /// <param name="maxHealth"> Maximum health value of the module. </param>
        /// <param name="type"> Type of the entity. </param>
        /// <param name="graphicalRepresentationID"> ID of the graphical representation associated with the module. </param>
        /// <param name="behaviourIDs"> Behaviour IDs associated with the module. </param>
        public ModuleDBEntity(bool official, string name, int damage, int agression, int reproductionNeed, int maxHunger, int maxHealth, EntityTypeEnum type, int graphicalRepresentationID, int[] behaviourIDs)
        {
            Official = official;
            Name = name;
            Damage = damage;
            Agression = agression;
            ReproductionNeed = reproductionNeed;
            MaxHunger = maxHunger;
            MaxHealth = maxHealth;
            Type = type;
            GraphicalRepresentationID = graphicalRepresentationID;
            BehaviourIDs = behaviourIDs;
        }

    }

}