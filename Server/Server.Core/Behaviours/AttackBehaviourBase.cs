using Server.Core.Exceptions;
using Server.Core.Services;

namespace Server.Core.Behaviours
{
    public abstract class AttackBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Human | EntityTypeEnum.Animal;


        /// <inheritdoc/>
        public virtual void Execute(WorldEntity attacker, WorldEntity target, Dictionary<string, object>? otherParams = null)
        { 
            try
            {
                int dmg = ModuleService.Instance.GetModuleById(attacker.ModuleID).Damage;
                target.State.Health -= dmg;
            }
            catch (ModuleNotFoundException ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity attacker, WorldEntity target, Dictionary<string, object>? otherParams = null);
    }
}
