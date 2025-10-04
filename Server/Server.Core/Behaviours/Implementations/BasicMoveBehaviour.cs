using System.Numerics;
using Server.Core.Helpers;

namespace Server.Core.Behaviours.Implementations
{
    public class BasicMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => throw new NotImplementedException();
        public override EntityTypeEnum Type => EntityTypeEnum.Animal;

        public override void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            throw new NotImplementedException();
        }

        public override bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            var map = otherParams?[CustomBehaviourParams.MAP_PARAM];
            return true;
        }


    }
}
