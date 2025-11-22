using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class GreaterAggressionAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 204;

        /// <inheritdoc/>
        public override string Description => "Attack only when the aggresion is greater than target's. Gives Damage to target, doesn't take any";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                if (target == null)
                {
                    throw new ArgumentNullException(nameof(target), "Target cannot be null for attack behaviour execution.");
                }
                
                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new ModuleNotFoundException($"Module with ID={attacker.ModuleID} not found!");

                target.State.Health -= attackerModule.Damage;
                if (target.State.Health <= 0)
                {
                    target.Die(moduleService);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if(target == null)
            {
                return false;
            }

            Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && attackerModule.Agression > targetModule.Agression;
        }
    }
}
