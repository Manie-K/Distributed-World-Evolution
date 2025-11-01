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
                ModuleDBEntity? moduleDBEntity = dbContext.Modules.Find(id);
                if (moduleDBEntity == null)
                {
                    return null;
                }

                Module module = Module.CreateFromDBEntity(moduleDBEntity);
                return module;
            }
        }

        public IEnumerable<Module> GetAllModules()
        {
            using (var dbContext = new ApplicationDBContext())
            {
                List<Module> modules = dbContext.Modules.Select(dbEnt => Module.CreateFromDBEntity(dbEnt)).ToList();
                return modules;
            }
        }

        public void CreateModule(CreateModuleDTO dto)
        {
            using (var dbContext = new ApplicationDBContext())
            {
                ModuleDBEntity moduleDBEntity = new ModuleDBEntity
                (
                    dto.Official, dto.Name, dto.Damage, dto.Agression, dto.ReproductionNeed, dto.Type, dto.GraphicalRepresentationID, dto.BehaviourIDs.ToArray()
                );

                dbContext.Modules.Add(moduleDBEntity);
                dbContext.SaveChanges();
            }
        }

        public int GetHumanModuleId()
        {
            using (var dbContext = new ApplicationDBContext())
            {
                var modules = dbContext.Modules.Where(m => m.Type == EntityTypeEnum.Human);
                if (modules.Count() == 0)
                {
                    throw new Exception("Human module not found");
                }
                if(modules.Count() > 1)
                {
                    throw new Exception("Multiple human modules found");
                }

                return modules.First().ID;
            }
        }
    }
}