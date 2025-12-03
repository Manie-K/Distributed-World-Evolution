using Server.Core.Modules;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Services
{
    public interface IModuleService
    {
        public Module? GetModuleById(int id);
        public int GetHumanModuleId();
        public IEnumerable<Module> GetAllModules();
        public void CreateModule(CreateModuleDTO dto);
    }
}