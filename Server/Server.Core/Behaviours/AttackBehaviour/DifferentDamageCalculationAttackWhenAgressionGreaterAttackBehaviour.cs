using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <summary>
    /// Different damage calculation attack behaviour that attacks only when the attacker's aggression is greater than the target's.
    /// </summary>
    public class DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 210;

        /// <inheritdoc/>
        public override string Description => "Attacks only when the aggresion is greater than target's. Gives Damage to target according to formula: damage * ((agression - enemy_aggresion) / agression)";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                if (target == null)
                {
                    throw new ArgumentNullException(nameof(target), "Target cannot be null for attack behaviour execution.");
                }

                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
                Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

                target.State.Health -= (int)(attackerModule.Damage * ((attackerModule.Agression - targetModule.Agression) / attackerModule.Agression) );

                if (target.State.Health <= 0)
                {
                    target.Die(moduleService);
                }

                attacker.State.LastAttackedEntityId = target.Id;
            }
            catch (ModuleNotFoundException ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (target == null) {
                return false;
            }

            Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && attackerModule.Agression > targetModule.Agression;
        }

    }

}