using SharedLibrary.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Server.UI.ViewModels
{
    class BaseTabViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OnLogEventArgs> Logs { get; } = new();
        public int ID { get; protected set; }
        
        protected string _header;
        public string Header
        {
            get => _header;
            protected set { _header = value; OnPropertyChanged(); }
        }

        protected string _info;
        public string Info
        {
            get => _info;
            protected set { _info = value; OnPropertyChanged(); }
        }

        public BaseTabViewModel(int id, string header, string info)
        {
            ID = id;
            _header = header;
            _info = info;
        }

        public void AppendLog(OnLogEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Logs.Add(e);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
