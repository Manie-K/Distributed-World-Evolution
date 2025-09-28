using System.Numerics;

namespace Server.Core.Modules
{
    public class BasicMoveBehaviour : IMoveBehaviour
    {
        public int DatabaseID => 222;
        public EntityTypeEnum Type => EntityTypeEnum.Animal;

        public void Move()
        {
        }

    }
}
