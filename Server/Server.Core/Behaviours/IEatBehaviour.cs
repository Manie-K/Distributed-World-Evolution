namespace Server.Core.Behaviours
{
    public interface IEatBehaviour : IBehaviour
    {
        public bool CanEat(WorldEntity entity, WorldEntity food);
        public void Eat(WorldEntity entity, WorldEntity food);
    }
}
