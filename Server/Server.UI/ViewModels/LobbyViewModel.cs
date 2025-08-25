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
        public int LobbyID { get; private set; }
        public LobbyViewModel(string name, string info, int lobbyID)
        {
            Header = name;
            Info = info;
            LobbyID = lobbyID;
        }
    }
}
