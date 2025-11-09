using System.Collections.Immutable;
using Server.Core.Services;

namespace Server.Core.Behaviours.MoveBehaviour
{
    /// <inheritdoc>
    public class PlantMoveBehaviour : MoveBehaviourBase
    {
        /// <inheritdoc>
        public override int DatabaseID => 104;

        /// <inheritdoc/>
        public override EntityTypeEnum Type => EntityTypeEnum.Plant;

        /// <inheritdoc>
        public override string Description => "Plants do not move";

        /// <inheritdoc>
        public override void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            //noop
            return;
        }

        /// <inheritdoc>
        public override bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }

        /// <inheritdoc>
        public override (int, int) GetNextMovement(WorldEntity entity, ImmutableList<WorldEntity> otherEntites)
        {
            return (0, 0);
        }
    }
}
