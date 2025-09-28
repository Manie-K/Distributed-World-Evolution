namespace Server.Core.Behaviours
{
    public interface IReproduceBehaviour : IBehaviour
    {
        public bool CanReproduce(WorldEntity entity, WorldEntity partner);
        public void Reproduce(WorldEntity entity, WorldEntity partner, bool[,] walkableTiles);
    }
}
