using Server.Core.Helpers;
using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.EatBehaviour
{
    internal class EatWhenNotHealthyBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 302;
        /// <inheritdoc/>
        public override string Description => "Eats when health below 50% of organism maximum health";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");

            if (entity.State.Health / entityModule.MaxHealth < 0.5)
            {
                return true;
            }

            return false;
        }
    }
}
