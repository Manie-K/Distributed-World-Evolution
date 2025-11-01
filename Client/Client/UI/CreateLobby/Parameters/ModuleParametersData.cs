using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateLobby.Parameters
{
    public class ModuleParametersData
    {

        public string Name;
        public string Value;
        public int Type;

        public ModuleParametersData(string name, string value, int type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
