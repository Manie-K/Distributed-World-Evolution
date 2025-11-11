using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    internal class ReproduceWithHungerBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 406;
        /// <inheritdoc/>
        public override string Description => "Reproduces with hunger.";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            base.Execute(entity, target, moduleService, otherParams);
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            entity.State.Hunger -= (int)0.1 * entityModule.MaxHunger;
            if (target != null)
            {
                target.State.Hunger -= (int)0.1 * entityModule.MaxHunger;
            }
        }
    }
}
