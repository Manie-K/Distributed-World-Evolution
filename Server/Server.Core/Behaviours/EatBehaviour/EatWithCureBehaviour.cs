using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.EatBehaviour
{
    public class EatWithCureBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 304;
        /// <inheritdoc/>
        public override string Description => "Eats and increase health at the same time, but decrease health twice if plan is poisonous";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

            if (targetModule.MaxHunger > 0)
            {
                entity.State.Hunger += targetModule.MaxHunger;
                entity.State.Health += targetModule.MaxHunger;
            }
            else
            {
                entity.State.Health -= 2* targetModule.Damage;
            }

            target.Die();
        }
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return true;
        }
    }
}
