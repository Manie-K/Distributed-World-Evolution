using Server.Core.Behaviours;
using Server.Core.Data;
using Server.Core.Exceptions;
using Server.Core.Modules;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Services
{
    public class ModuleService : IModuleService
    {
        public static IModuleService Instance = new ModuleService(BehaviourService.Instance);

        private IBehaviourService behaviourService;

        public ModuleService(IBehaviourService behaviourService)
        {
            this.behaviourService = behaviourService;
        }


        public Module? GetModuleById(int id)
        {
            using(var dbContext = new ApplicationDBContext())
            {
                ModuleDBEntity? moduleEntity = dbContext.Modules.Find(id);
                if (moduleEntity == null)
                {
                    return null;
                }

                List<IBehaviour> behaviours = moduleEntity.BehaviourIDs
                                    .Select(behaviourService.GetBehaviourInstanceByID)
                                    .Where(b => b != null)
                                    .ToList();

                Module module = new Module.ModuleBuilder()
                                    .WithName(moduleEntity.Name)
                                    .IsOfficial(moduleEntity.Official)
                                    .OfType(moduleEntity.Type)
                                    .WithDamage(moduleEntity.Damage)
                                    .WithAgression(moduleEntity.Agression)
                                    .WithReproductionNeed(moduleEntity.ReproductionNeed)
                                    .WithGraphicsId(moduleEntity.GraphicalRepresentationID)
                                    .WithBehaviours(behaviours)
                                    .Create();
                return module;
            }
        }

        public IEnumerable<Module> GetAllModules()
        {
            using (var dbContext = new ApplicationDBContext())
            {
                List<Module> modules = new List<Module>();

                foreach (ModuleDBEntity moduleEntity in dbContext.Modules)
                {
                    List<IBehaviour> behaviours = moduleEntity.BehaviourIDs
                                        .Select(behaviourService.GetBehaviourInstanceByID)
                                        .Where(b => b != null)
                                        .ToList();

                    Module module = new Module.ModuleBuilder()
                                        .WithName(moduleEntity.Name)
                                        .IsOfficial(moduleEntity.Official)
                                        .OfType(moduleEntity.Type)
                                        .WithDamage(moduleEntity.Damage)
                                        .WithAgression(moduleEntity.Agression)
                                        .WithReproductionNeed(moduleEntity.ReproductionNeed)
                                        .WithGraphicsId(moduleEntity.GraphicalRepresentationID)
                                        .WithBehaviours(behaviours)
                                        .Create();
                    modules.Add(module);
                }

                return modules;
            }
        }

        public void CreateModule(CreateModuleDTO dto)
        {
            using (var dbContext = new ApplicationDBContext())
            {
                ModuleDBEntity moduleDBEntity = new ModuleDBEntity
                    (
                        dto.Official, dto.Name, dto.Damage, dto.Agression, dto.ReproductionNeed, dto.Type, dto.GraphicalRepresentationID, dto.BehaviourIDs
                    );

                dbContext.Modules.Add(moduleDBEntity);
                dbContext.SaveChanges();
            }
        }
    }
}