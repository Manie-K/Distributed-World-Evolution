using Server.Core.Modules;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Services
{
    /// <summary>
    /// Interface for managing modules.
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Retrieves a module by its ID.
        /// </summary>
        /// <param name="id"> The ID of the module. </param>   
        /// <returns> The module if found; otherwise, null. </returns>
        public Module? GetModuleById(int id);

        /// <summary>
        /// Retrieves the ID of the human module.
        /// </summary>
        /// <returns> The ID of the human module. </returns>
        public int GetHumanModuleId();

        /// <summary>
        /// Retrieves all available modules.
        /// </summary>
        /// <returns> A collection of all modules. </returns>   
        public IEnumerable<Module> GetAllModules();

        /// <summary>
        /// Creates a new module based on the provided DTO.
        /// </summary>
        /// <param name="dto"> The DTO containing module creation data. </param>
        public void CreateModule(CreateModuleDTO dto);

    }

}