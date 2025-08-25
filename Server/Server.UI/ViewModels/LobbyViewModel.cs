using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Server.Core.Lobby;

namespace Server.UI.ViewModels
{
    internal class LobbyViewModel : BaseTabViewModel
    {
        public LobbyViewModel(string name, string info)
        {
            Header = name;
            Info = info;
            ConsoleText = "";
        }
    }
}
