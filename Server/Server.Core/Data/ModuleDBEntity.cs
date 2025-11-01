using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Data
{
    /// <summary>
    /// Database entity representing a Module.
    /// Seperate class from Module.cs in case we want to change the DB schema without affecting the in-memory Module representation.
    /// </summary>
    public class ModuleDBEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public bool Official { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Agression { get; set; }
        public int ReproductionNeed { get; set; }
        public int MaxHunger { get; set; }
        public int MaxHealth { get; set; }
        public EntityTypeEnum Type { get; set; }
        public int GraphicalRepresentationID { get; set; }
        public int[] BehaviourIDs { get; set; }

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
