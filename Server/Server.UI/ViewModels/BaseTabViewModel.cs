using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.UI.ViewModels
{
    class BaseTabViewModel : INotifyPropertyChanged
    {
        public string Header { get; protected set; }
        public string Info { get; protected set; }

        private string _consoleText;
        public string ConsoleText
        {
            get => _consoleText;
            set
            {
                _consoleText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
