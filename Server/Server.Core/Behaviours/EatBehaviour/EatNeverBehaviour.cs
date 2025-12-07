using Server.Core.Services;

namespace Server.Core.Behaviours.EatBehaviour
{
    /// <summary>
    /// Eat behaviour that never allows eating.
    /// </summary>
    public class EatNeverBehaviour : EatBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 306;

        /// <inheritdoc/>
        public override string Description => "Never eats";

        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Console.WriteLine("We shouldn't be here. EatNeverBehaviour.Execute was called.");
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }

    }

}