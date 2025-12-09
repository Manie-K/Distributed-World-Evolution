using SharedLibrary.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Server.UI.ViewModels
{
    /// <summary>
    /// Base class for tab view models in the server UI.
    /// </summary>
    internal class BaseTabViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Observable collection of logs associated with the tab.
        /// </summary>
        public ObservableCollection<Log> Logs { get; } = new();

        /// <summary>
        /// ID of the tab.
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Header of the tab.
        /// </summary>
        public string Header { get; protected set; }

        /// Info text of the tab.
        private string _info;

        /// <summary>
        /// Info text of the tab.
        /// </summary>
        public string Info
        {
            get => _info;
            set
            {
                if (_info != value)
                {
                    _info = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"> The ID of the tab. </param>
        /// <param name="header"> The header of the tab. </param>
        /// <param name="info"> The info text of the tab. </param>
        public BaseTabViewModel(int id, string header, string info)
        {
            ID = id;
            Header = header;
            _info = info;
        }

        /// <summary>
        /// Appends a log entry to the Logs collection.
        /// </summary>
        /// <param name="log"> The log entry to append. </param>
        public void AppendLog(Log log)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Logs.Add(log);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        /// <summary>
        /// Event triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}