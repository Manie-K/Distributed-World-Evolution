using System.Numerics;
using Server.Core.Helpers;
using SharedLibrary.Helpers;

namespace Server.Core.Behaviours.Implementations
{
    public class BasicMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => 2;
        public override EntityTypeEnum Type => EntityTypeEnum.Animal;
        public override string Description => "Basic move behaviour";

        public BasicMoveBehaviour()
        {
        }


        public override bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null)
        {
            Position2D nextPos = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.NEW_POS_PARAM, out object? value) && value is Position2D pos ? pos : entity.State.Position;
            bool[,] walkableTiles = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.MAP_PARAM, out object? walkableTilesObj) && walkableTilesObj is bool[,] tiles ? tiles : new bool[0, 0];
            if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X >= walkableTiles.GetLength(0) || nextPos.Y >= walkableTiles.GetLength(1))
            {
                return false;
            }

            return walkableTiles[nextPos.X, nextPos.Y];
        }

        public override (int, int) GetNextMovement(WorldEntity entity)
        {
            return (1,1);
        }
    }
}
