using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    public class ReproduceAlwaysBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 401;
        /// <inheritdoc/>
        public override string Description => "Reproduces whenever possible.";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return true;
        }
    }
}
