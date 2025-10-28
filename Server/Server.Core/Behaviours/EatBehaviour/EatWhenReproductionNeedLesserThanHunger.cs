using Server.Core.Helpers;
using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.EatBehaviour
{
    internal class EatWhenReproductionNeedLesserThanHunger : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 303;
        /// <inheritdoc/>
        public override string Description => "Eats when reproduction need is lesser than hunger need";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = ModuleService.Instance.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");

            if (entity.State.Hunger/entityModule.MaxHunger < entityModule.ReproductionNeed/ModulePropertiesLimits.MAX_REPRODUCTION_NEED)
            {
                return true;
            }

            return false;
        }
    }
}
