using Server.UI.Models;
using Server.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Server.UI
{
    internal class TabTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate ServerTemplate { get; set; }
        public required DataTemplate LobbyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ServerViewModel)
                return ServerTemplate;
            else if (item is LobbyViewModel)
                return LobbyTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
