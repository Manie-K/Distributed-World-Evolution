using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.EatBehaviour
{
    internal class EatWithCureBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 304;
        /// <inheritdoc/>
        public override string Description => "Eats and increase health at the same time";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            base.Execute(entity, target, otherParams);
            
            Module targetModule = ModuleService.Instance.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");
            
            entity.State.Health += targetModule.MaxHunger;
        }
        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            return true;
        }
    }
}
