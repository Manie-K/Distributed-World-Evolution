using Server.Core.Exceptions;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <summary>
    /// Base class for attack behaviours. Implements basic attack execution logic.
    /// </summary>
    public abstract class AttackBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal | EntityTypeEnum.Human; //Human part - old code

        /// <inheritdoc/>
        public abstract string Description { get; }


        /// <inheritdoc/>
        /// Target's module will be of type Human or Animal.
        public virtual void Execute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                if (target == null)
                {
                    throw new ArgumentNullException(nameof(target), "Target cannot be null for attack behaviour execution.");
                }
                else if (attacker == target)
                {
                    throw new ArgumentException("Attacker and target cannot be the same entity.");
                }

                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new ModuleNotFoundException($"Module with ID={attacker.ModuleID} not found!");
                Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new ModuleNotFoundException($"Module with ID={attacker.ModuleID} not found!");
               
                target.State.Health -= attackerModule.Damage;
                attacker.State.Health -= (int)(targetModule.Damage * 0.5);

                if (target.State.Health > 0)
                {
                    if (attacker.State.Health > 0)
                    {
                        attacker.State.LastAttackedEntityId = target.Id;
                    }
                    else
                    {
                        attacker.Die(moduleService);
                    }
                }
                else
                {
                    target.Die(moduleService);
                    attacker.State.Hunger += 20;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        /// Target's module will be of type Human or Animal.
        public abstract bool CanExecute(WorldEntity attacker, WorldEntity? target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehaviourDTO ToDTO()
        {
            return new BehaviourDTO(DatabaseID, Description, Type, BehaviourInteractionTypeEnum.Attack);
        }

    }
}
