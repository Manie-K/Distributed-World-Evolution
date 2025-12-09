using System.Text.Json.Serialization;

namespace SharedLibrary.Logging
{
    /// <summary>
    /// Log level enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogLevelEnum
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

}
