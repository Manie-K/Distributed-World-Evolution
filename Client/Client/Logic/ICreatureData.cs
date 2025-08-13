using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client
{
    public enum CreatureType{
        Plant,
        Animal
    }


    [JsonDerivedType(typeof(AnimalData))]
    [JsonDerivedType(typeof(PlantData))]
    public abstract class ICreatureData
    {

        public string Name { set; get; }
        public CreatureType Type { set; get; }

        public int ReproduceCooldown { get; set; }

    }
}
