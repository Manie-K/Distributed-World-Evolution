using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    internal class ReproduceCausingDamageBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 406;
        /// <inheritdoc/>
        public override string Description => "Reproduction cause damage.";
        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            base.Execute(entity, target, moduleService, otherParams);

            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            entity.State.Health -= entityModule.Damage;
            if (target != null)
            {
                target.State.Health -= entityModule.Damage;
            }
        }

    }
}
