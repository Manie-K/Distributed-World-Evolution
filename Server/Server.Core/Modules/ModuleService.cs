using Server.Core.Behaviours;
using Server.Core.Exceptions;
using SharedLibrary;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Modules
{
    public class ModuleService : IModuleService
    {
        public static IModuleService Instance = new ModuleService(BehaviourService.Instance);

        private IBehaviourService behaviourService;

        public ModuleService(IBehaviourService behaviourService)
        {
            this.behaviourService = behaviourService;
        }


        public Module GetModuleById(int id)
        {
            throw new ModuleNotFoundException();
        }

        public Module CreateModuleInstance(CreateModuleDTO dto)
        {
            var behaviours = dto.BehaviourIDs.Select(behaviourService.GetBehaviourInstanceByID).ToList();

            Module module = new Module.ModuleBuilder()
                                .WithName(dto.Name)
                                .IsOfficial(false)
                                .WithDamage(dto.Damage)
                                .WithAggresion(dto.Aggresion)
                                .WithReproductionNeed(dto.ReproductionNeed)
                                .WithBehaviours(behaviours)
                                .Create();

            return module;
        }

        public IEnumerable<Module> GetAllModules()
        {
            throw new NotImplementedException();
        }
    }
}