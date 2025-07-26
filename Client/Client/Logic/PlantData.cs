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

        public PlantData(string name, int reproducecooldown, int toxicitydamage)
        {
            this.type = Type.Plant;
            this.name = name;
            this.ReproduceCooldown = reproducecooldown;
            this.ToxicityDamage = toxicitydamage;
        }
    }
}
