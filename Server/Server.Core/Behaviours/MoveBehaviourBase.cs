namespace Server.Core.Behaviours
{
    public abstract class MoveBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal | EntityTypeEnum.Human;


        /// <inheritdoc/>
        public abstract void Execute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, Dictionary<string, object>? otherParams = null);
    }
}
