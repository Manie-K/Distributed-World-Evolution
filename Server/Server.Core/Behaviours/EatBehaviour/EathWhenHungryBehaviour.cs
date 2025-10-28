using Server.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Server.Core.Behaviours.EatBehaviour
{
    internal class EathWhenHungryBehaviour : EatBehaviourBase
    {
        public override int DatabaseID => 301;
        public override string Description => "Eats when hunger below 25% of organism maximum hunger";

        public override bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            if (otherParams == null || !otherParams.TryGetValue("entityModule", out var moduleObj))
                throw new ArgumentException("Missing entityModule in parameters");

            var entityModule = moduleObj as Module
                ?? throw new ArgumentException("Invalid entityModule type");

            if (entity.State.Hunger < 0.25 * entityModule.MaxHunger)
            {
                return true;
            }

            return false;
        }

    }
}
