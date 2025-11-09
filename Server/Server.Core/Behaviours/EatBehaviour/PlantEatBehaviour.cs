using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class PlantEatBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 309;
        /// <inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;
        /// <inheritdoc/>
        public override string Description => "Plants eath behaviour.";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return;
        }
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }
    }
}
