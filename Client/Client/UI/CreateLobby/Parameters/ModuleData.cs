using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateLobby.Parameters
{
    public class ModuleData
    {
        public string ModuleName { set; get; }
        public int GraphicIndex { set; get; }
        public bool IsOfficial { set; get; }      
        public List<ModuleParametersData> ModuleParameters { set; get; }


        public ModuleData(string moduleName, int graphicIndex, bool isOfficial, List<ModuleParametersData> moduleParameters)
        {
            ModuleName = moduleName;
            GraphicIndex = graphicIndex;
            IsOfficial = isOfficial;
            ModuleParameters = moduleParameters;
        }
    }
}
