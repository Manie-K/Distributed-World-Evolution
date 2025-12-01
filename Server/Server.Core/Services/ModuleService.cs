using System.Diagnostics;
using Server.Core.Data;
using Server.Core.Modules;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Services
{
    public class ModuleService : IModuleService
    {
        public static IModuleService Instance = new ModuleService();

        private ModuleService() { }

        private readonly Dictionary<int, Module> moduleCache = new Dictionary<int, Module>();

        public Module? GetModuleById(int id)
        {
            if (moduleCache.TryGetValue(id, out Module? value))
            {
                return value;
            }

            using (var dbContext = new ApplicationDBContext())
            {
                ModuleDBEntity? moduleDBEntity = dbContext.Modules.Find(id);
                if (moduleDBEntity == null)
                {
                    return null;
                }

                Module module = Module.CreateFromDBEntity(moduleDBEntity);
                moduleCache.Add(id, module);
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
                    dto.Official, dto.Name, dto.Damage, dto.Aggression, dto.ReproductionNeed, dto.MaxHunger, dto.MaxHealth, dto.Type, dto.GraphicalRepresentationID, dto.BehaviourIDs.ToArray()
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