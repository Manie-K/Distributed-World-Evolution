using Server.Core.Helpers;
using Server.Core.Services;
using System.Collections.Immutable;

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
        public override string Description => "Plants never move.";

        /// <inheritdoc>
        public override bool CanExecute(WorldEntity entity, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }

        /// <inheritdoc>
        public override (int, int) GetNextMovement(WorldEntity entity, Span<WorldEntity> otherEntites)
        {
            return (0, 0);
        }
    }
}
