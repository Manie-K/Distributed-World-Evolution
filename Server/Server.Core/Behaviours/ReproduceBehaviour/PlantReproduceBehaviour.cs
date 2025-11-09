using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    internal class PlantReproduceBehaviour : ReproduceBehaviourBase
    {
        //<inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;
        ///<inheritdoc/>
        public override int DatabaseID => 403;
        ///<inheritdoc/>
        public override string Description => "Plant reproduce behaviour.";
    }
}
