using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PlantData : ICreatureData
    {
        public int ToxicityDamage { get; set; }

        public PlantData(string name, int reproduceCooldown, int toxicityDamage)
        {
            this.Type = CreatureType.Plant;
            this.Name = name;
            this.ReproduceCooldown = reproduceCooldown;
            this.ToxicityDamage = toxicityDamage;
        }
    }
}
