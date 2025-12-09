using System.Text.Json.Serialization;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Info message type enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InfoMessageTypeEnum
    {
        Info,
        Warning,
        Error,
        LobbyJoined,
        LobbyDisjoined,
        LobbyNotJoined,
        LobbyNotDisjoined,
        LobbyCreated,
        LobbyNotCreated,
        ServerConnected,
        ServerNotConnected,
        ServerDisconnected,
        ModuleCreated,
        ModuleNotCreated,
    }

}
