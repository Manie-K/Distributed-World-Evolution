using Server.Core.Services;

namespace Server.Core.Behaviours.ReproduceBehaviour
{
    internal class ReproduceNeverBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 408;

        /// <inheritdoc/>
        public override string Description => "Never reproduces.";

        /// <inheritdoc/>
        public override void Execute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            // Do nothing
            return;
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }
    }
}
