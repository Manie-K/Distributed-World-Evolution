namespace SharedLibrary
{
    public class OnLogEventArgs
    {
        public string Message { get; }
        public LogLevelEnum LogLevel { get; }
        public DateTime Timestamp { get; }
    
        public OnLogEventArgs(string message, LogLevelEnum logLevel)
        {
            Message = message;
            LogLevel = logLevel;
            Timestamp = DateTime.Now;
        }
    }
}
