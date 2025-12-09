using System.Collections.Generic;

namespace Client.UI.CreateLobby.Parameters
{
    public class ModuleData
    {
        public string ModuleName { set; get; }
        public int ModuleID { set; get; }
        public int GraphicIndex { set; get; }
        public bool IsOfficial { set; get; }      
        public List<ModuleParametersData> ModuleParameters { set; get; }

        public ModuleData(string moduleName, int moduleID, int graphicIndex, bool isOfficial, List<ModuleParametersData> moduleParameters)
        {
            ModuleName = moduleName;
            ModuleID = moduleID;
            GraphicIndex = graphicIndex;
            IsOfficial = isOfficial;
            ModuleParameters = moduleParameters;
        }
    }
}
