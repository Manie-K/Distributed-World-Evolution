using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.MapSelection
{
    public class SelectedMapData
    {
        public int index;
        public string name;

        public SelectedMapData()
        {
            this.index = 0;
            this.name = "Day Forest";
        }


        public void ChangeData(int index, string name)
        {
            this.index = index;
            this.name = name;
        }
    }
}
