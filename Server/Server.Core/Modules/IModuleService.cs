using SharedLibrary;

namespace Server.Core.Modules
{
    public interface IModuleService 
    {
        // TODO: We need to add db.
        // Workflow could be as follow:
        // 1. Send dto from client to server with info about new module
        // 2. Server saves it to db
        // 3. Client saves info about which module to load
        // 4. Server gets info from db and create instance
        public Module CreateModuleInstance(CreateModuleDTO dto);
    }
}