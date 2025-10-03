using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PlantData : ICreatureData
    {
        public int ContactDamage { get; set; } 
        public int ToxicityDamage { get; set; }  

        public bool IsFertile { get; set; }  
        public bool IsToxicOnContact { get; set; } 
        public bool IsToxicWhenEaten { get; set; }  

        public PlantData(
            string name,
            int reproduceCooldown,
            int contactDamage,
            int toxicityDamage,
            bool canReproduce,
            bool isToxicOnContact,
            bool isToxicWhenEaten,
            bool isOfficial,
            int graphicIndex)
        {
            this.Type = CreatureType.Plant;
            this.Name = name;
            this.BreedingCooldown = reproduceCooldown;
            this.ContactDamage = contactDamage;
            this.ToxicityDamage = toxicityDamage;
            this.IsFertile = canReproduce;
            this.IsToxicOnContact = isToxicOnContact;
            this.IsToxicWhenEaten = isToxicWhenEaten;
            IsOfficial = isOfficial;
            GraphicIndex = graphicIndex;
        }
    }
}
