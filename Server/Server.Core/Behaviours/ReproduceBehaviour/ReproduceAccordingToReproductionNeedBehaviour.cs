namespace Server.Core.Behaviours.ReproduceBehaviour
{
    /// <summary>
    /// Reproduce behaviour implementation based on reproduction need.
    /// </summary>
    public class ReproduceAccordingToReproductionNeedBehaviour : ReproduceBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 402;
        
        /// <inheritdoc/>
        public override string Description => "Reproduction depends on an animal's reproduction need.";
    }

}
