namespace SharedLibrary.Logging
{
    /// <summary>
    /// On log event arguments.
    /// </summary>
    public class OnLogEventArgs
    {
        /// <summary>
        /// Content of the log entry.
        /// </summary>
        public string Content { get; init; }

        /// <summary>
        /// Log level of the entry.
        /// </summary>
        public LogLevelEnum LogLevel { get; init; }

        /// <summary>
        /// Timestamp of when the log entry was created.
        /// </summary>
        public DateTime Timestamp { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public OnLogEventArgs(string content, LogLevelEnum logLevel)
        {
            Content = content;
            LogLevel = logLevel;
            Timestamp = DateTime.Now;
        }

    }

}