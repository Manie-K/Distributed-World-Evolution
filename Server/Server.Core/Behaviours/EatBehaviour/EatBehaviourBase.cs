using Server.Core.Helpers;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Behaviours.EatBehaviour
{
    public abstract class EatBehaviourBase : IBehaviour
    {
        /// <inheritdoc/>
        public abstract int DatabaseID { get; }

        /// <inheritdoc/>
        public virtual EntityTypeEnum Type => EntityTypeEnum.Animal;

        /// <inheritdoc/>
        public abstract string Description { get; }


        /// <inheritdoc/>
        public virtual void Execute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={target.ModuleID} not found!");

            if (targetModule.Damage > 0)
            {
                entity.State.Health -= targetModule.Damage;
                target.Die();
            }
            else
            {
                entity.State.Hunger += targetModule.MaxHunger;
                target.Die();
                //Why not hunger? Max hunger in plants should be not relevant? Why this way? @FranciszekGwarek
                // Plant module need to have MaxHuger, otherwise how could we know the Hunger of a single entity and Hunger in plants does not decrease. @MaciejGóralczyk
            }
        }

        /// <inheritdoc/>
        public abstract bool CanExecute(WorldEntity entity, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null);

        /// <inheritdoc/>
        public BehviourDTO ToDTO()
        {
            return new BehviourDTO(DatabaseID, Description, Type);
        }

    }
}
