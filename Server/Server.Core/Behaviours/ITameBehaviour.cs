namespace Server.Core.Behaviours
{
    public interface ITameBehaviour : IBehaviour
    {
        public void Tame(WorldEntity tamer, WorldEntity target);
        public bool CanTame(WorldEntity tamer, WorldEntity target);
    }
}