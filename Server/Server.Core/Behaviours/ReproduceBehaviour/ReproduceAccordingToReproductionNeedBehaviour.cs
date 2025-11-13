using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    public class ReproduceAccordingToReproductionNeedBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 402;
        /// <inheritdoc/>
        public override string Description => "Reproduction depends on an animal's reproduction need.";
    }
}
