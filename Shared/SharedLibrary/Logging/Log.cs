namespace SharedLibrary.Logging
{
    /// <summary>
    /// Class representing a log entry.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Content of the log entry.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Log level of the entry.
        /// </summary>
        public LogLevelEnum LogLevel { get; set; }

        /// <summary>
        /// Timestamp of when the log entry was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Content of the log entry.</param>
        /// <param name="logLevel">Log level of the entry.</param>
        public Log(string content, LogLevelEnum logLevel)
        {
            Content = content;
            LogLevel = logLevel;
            Timestamp = DateTime.Now;
        }

    }

}
