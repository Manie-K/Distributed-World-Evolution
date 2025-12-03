using SharedLibrary.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Server.UI.ViewModels
{
    class BaseTabViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Log> Logs { get; } = new();
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
            set { _info = value; OnPropertyChanged(); }
        }

        public BaseTabViewModel(int id, string header, string info)
        {
            ID = id;
            _header = header;
            _info = info;
        }

        public void AppendLog(Log log)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Logs.Add(log);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
