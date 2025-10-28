using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class AlwaysAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 201;

        /// <inheritdoc/>
        public override string Description => "Most basic attack implementation. Always attacks other, gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id;
        }
    }

    /// <inheritdoc/>
    public class AlwaysAttackAnimalsBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 202;

        /// <inheritdoc/>
        public override string Description => "Attacks only animals. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && targetModule.Type == EntityTypeEnum.Animal;
        }
    }

    /// <inheritdoc/>
    public class AlwaysAttackHumanBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 203;

        /// <inheritdoc/>
        public override string Description => "Attacks only humans. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && targetModule.Type == EntityTypeEnum.Human;
        }
    }

    /// <inheritdoc/>
    public class GreaterAggresionAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 204;

        /// <inheritdoc/>
        public override string Description => "Attack only when the aggresion is greater than target's. Gives Damage to target, doesn't take any";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");

                target.State.Health -= attackerModule.Damage;
                if (target.State.Health <= 0)
                {
                    target.Die();
                }
            }
            catch (ModuleNotFoundException ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && attackerModule.Agression > targetModule.Agression;
        }
    }

    /// <inheritdoc/>
    public class Random50AttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 205;

        /// <inheritdoc/>
        public override string Description => "50% chance to attack. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id && Random.Shared.NextDouble() < 0.5;
        }
    }

    /// <inheritdoc/>
    public class Random75AttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 206;

        /// <inheritdoc/>
        public override string Description => "75% chance to attack. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id && Random.Shared.NextDouble() < 0.75;
        }
    }

    /// <inheritdoc/>
    public class Random25AttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 207;

        /// <inheritdoc/>
        public override string Description => "25% chance to attack. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id && Random.Shared.NextDouble() < 0.25;
        }
    }

    /// <inheritdoc/>
    public class EnemyLowHealthAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 208;

        /// <inheritdoc/>
        public override string Description => "Attacks only if target health is below 30%. Gives Damage to target, takes rounded 25% of target's damage back";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
                Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");

                target.State.Health -= attackerModule.Damage;
                attacker.State.Health -= (int)(targetModule.Damage * 0.25f);

                if (target.State.Health <= 0)
                {
                    target.Die();
                }
                if (attacker.State.Health <= 0)
                {
                    attacker.Die();
                }
            }
            catch (ModuleNotFoundException ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && target.State.Health < (int)(0.3f * targetModule.MaxHealth);
        }
    }

    /// <inheritdoc/>
    public class OnlyAttackSelfSpiecesBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 209;

        /// <inheritdoc/>
        public override string Description => "Only attacks organisms of the same species. Gives Damage to target, takes rounded half of target damage back";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id && attacker.ModuleID == target.ModuleID;
        }
    }

    /// <inheritdoc/>
    public class DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 210;

        /// <inheritdoc/>
        public override string Description => "Attacks only when the aggresion is greater than target's. Gives Damage to target according to formula: damage * ((agression - enemy_aggresion) / agression)";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
                Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

                target.State.Health -= (int)(attackerModule.Damage * ((attackerModule.Agression - targetModule.Agression) / attackerModule.Agression) );

                if (target.State.Health <= 0)
                {
                    target.Die();
                }
            }
            catch (ModuleNotFoundException ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && attackerModule.Agression > targetModule.Agression;
        }
    }

    /// <inheritdoc/>
    public class HumanAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 211;

        /// <inheritdoc/>
        public override string Description => "Attack behaviour used by humans";

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return attacker.Id != target.Id;
        }
    }

    /// <inheritdoc/>
    public class NeverAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 212;
        
        /// <inheritdoc/>
        public override string Description => "Does not attack ever";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            //noop
            return;
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            return false;
        }
    }
}
