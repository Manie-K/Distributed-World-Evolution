using Server.UI.Models;
using Server.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Server.UI
{
    internal class TabTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ServerTemplate { get; set; }
        public DataTemplate LobbyTemplate { get; set; }

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
