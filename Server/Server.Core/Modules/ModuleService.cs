using SharedLibrary;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Modules
{
    public class ModuleService : IModuleService
    {
        private IBehaviourService behaviourService;
        public ModuleService(IBehaviourService behaviourService)
        {
            this.behaviourService = behaviourService;
        }

        public Module GetModuleById(int id)
        {
            throw new NotImplementedException();
        }

        public Module CreateModuleInstance(CreateModuleDTO dto)
        {
            var behaviours = dto.BehaviourIDs.Select(behaviourService.GetBehaviourInstanceByID).ToList();

            Module module = new Module.ModuleBuilder()
                                .WithName(dto.Name)
                                .WithAuthor(dto.Author)
                                .WithVersion(dto.Version)
                                .WithStats(dto.Stats)
                                .WithBehaviours(behaviours)
                                .Create();

            return module;
        }
    }
}