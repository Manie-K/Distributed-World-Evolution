using Server.Core.Helpers;
using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class EatWhenHungry50Behaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 301;
        /// <inheritdoc/>
        public override string Description => "Eats when hunger below 50% of organism maximum hunger";
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found!");

            if ((float)entity.State.Hunger/entityModule.MaxHunger < 0.5)
            {
                return true;
            }

            return false;
        }

    }
}
