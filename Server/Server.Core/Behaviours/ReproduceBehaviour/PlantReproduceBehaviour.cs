using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    internal class PlantReproduceBehaviour : ReproduceAccordingToReproductionNeedBehaviour
    {
        //<inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;
    }
}
