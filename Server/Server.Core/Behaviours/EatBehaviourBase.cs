namespace Server.Core.Behaviours
{
    public abstract class EatBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal;


        /// <inheritdoc/>
        public abstract void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

    }
}
