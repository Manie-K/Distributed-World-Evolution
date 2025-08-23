using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.UI.Models;

namespace Server.UI.ViewModels
{
    internal class MainViewModel
    {
        public ObservableCollection<BaseTabViewModel> Tabs { get; } = new ObservableCollection<BaseTabViewModel>();

        public MainViewModel()
        {
            Tabs.Add(new ServerViewModel());
            var lobbies = new[] { "Lobby 1", "Lobby 2", "Lobby 3" };
            foreach (var l in lobbies)
            {
                Tabs.Add(new LobbyViewModel(l, $"Info about {l}"));
            }
        }

    }
}
