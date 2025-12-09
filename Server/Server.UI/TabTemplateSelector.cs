using Server.UI.Models;
using Server.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Server.UI
{
    /// <summary>
    /// Class for selecting the appropriate DataTemplate for tabs in the server UI.
    /// </summary>
    internal class TabTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Server tab DataTemplate.
        /// </summary>
        public required DataTemplate ServerTemplate { get; set; }

        /// <summary>
        /// Lobby tab DataTemplate.
        /// </summary>
        public required DataTemplate LobbyTemplate { get; set; }

        /// <summary>
        /// Selects the appropriate DataTemplate based on the type of the item.
        /// </summary>
        /// <param name="item"> The item for which to select the template. </param>
        /// <param name="container"> The container in which the item is displayed. </param>
        /// <returns> The selected DataTemplate. </returns>
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