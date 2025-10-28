using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.EatBehaviour
{
    internal class EatRandomlyBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 305;

        /// <inheritdoc/>
        public override string Description => "Eats randomly regardless of hunger level"; // Make more desriptive

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Random rand = new Random();

            double value = rand.NextDouble();

            if (value <= 0.6)
            {
                return true;
            }

            return false;
        }

    }
}
