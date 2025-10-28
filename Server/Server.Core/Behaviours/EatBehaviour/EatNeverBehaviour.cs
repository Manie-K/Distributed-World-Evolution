using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Behaviours.EatBehaviour
{
    internal class EatNeverBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 306;
        /// <inheritdoc/>
        public override string Description => "Eats never";
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
