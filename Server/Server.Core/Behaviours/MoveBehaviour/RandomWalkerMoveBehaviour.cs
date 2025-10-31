using System.Numerics;
using Server.Core.Helpers;
using Server.Core.Services;

namespace Server.Core.Behaviours.MoveBehaviour
{
    public class RandomWalkerMoveBehaviour : MoveBehaviourBase
    {
        public override int DatabaseID => 101;
        public override string Description => "Basic move behaviour, random 8-sided movement";

        public override (int, int) GetNextMovement(WorldEntity entity)
        {
            int x = 0, y = 0;
            while (x == 0 && y == 0)
            {
                x = new Random().Next(3) - 1; // -1, 0, 1
                y = new Random().Next(3) - 1; // -1, 0, 1
            }
            return (x, y);
        }
    }
}