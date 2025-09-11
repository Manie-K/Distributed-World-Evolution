using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.UI.ViewModels
{
    class BaseTabViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OnLogEventArgs> Logs { get; } = new();
        public string Header { get; protected set; }
        public string Info { get; protected set; }

        public void AppendLog(OnLogEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Logs.Add(e);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
