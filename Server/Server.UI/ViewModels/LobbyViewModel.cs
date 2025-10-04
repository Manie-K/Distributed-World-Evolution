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
        public int ID { get; init; }
        public string Name { get; init; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public int MapID { get; init; }

        public LobbyViewModel(int id, string name, int maxPlayers, int currentPlayer, int mapID)
        {
            Header = name;
            ID = id;
            Name = name;
            MaxPlayers = maxPlayers;
            CurrentPlayers = currentPlayer;
            MapID = mapID;
            Info = $"ID: {ID}, Max players: {MaxPlayers}, Current Players: {CurrentPlayers}, MapID: {MapID}";
        }
    }
}
