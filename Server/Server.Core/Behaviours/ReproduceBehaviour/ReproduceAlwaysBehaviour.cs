namespace Server.Core.Behaviours.ReproduceBehaviour
{
    internal class ReproduceAlwaysBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 407;

        /// <inheritdoc/>
        public override string Description => "Always reproduces.";
    }
}
