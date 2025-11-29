using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class EnemyLowHealthAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 208;

        /// <inheritdoc/>
        public override string Description => "Attacks only if target health is below 30%. Gives Damage to target, takes rounded 25% of target's damage back";

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
                Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new ModuleNotFoundException($"Module with ID={attacker.ModuleID} not found!");

                target.State.Health -= attackerModule.Damage;
                attacker.State.Health -= (int)(targetModule.Damage * 0.25f);

                if (target.State.Health <= 0)
                {
                    target.Die(moduleService);
                }
                if (attacker.State.Health <= 0)
                {
                    attacker.Die(moduleService);
                }

                attacker.State.LastAttackedEntityId = target.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            if (target == null)
            {
                return false;
            }

            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && target.State.Health < (int)(0.3f * targetModule.MaxHealth);
        }
    }
}
