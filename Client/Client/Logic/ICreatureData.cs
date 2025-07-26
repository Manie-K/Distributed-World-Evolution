using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client
{
    public enum Type{
        Plant,
        Animal
    }


    [JsonDerivedType(typeof(AnimalData))]
    [JsonDerivedType(typeof(PlantData))]
    public abstract class ICreatureData
    {

        public string name { set; get; }
        public Type type { set; get; }

        public int ReproduceCooldown { get; set; }

    }
}
