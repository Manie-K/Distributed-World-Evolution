using Server.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.UI.Models
{
    internal class ServerViewModel : BaseTabViewModel
    {
        public ServerViewModel()
        {
            Header = "Server";
            Info = "Info about server";
        }
    }

}
