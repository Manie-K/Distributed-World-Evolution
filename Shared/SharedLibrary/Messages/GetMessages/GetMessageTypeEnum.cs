using System.Text.Json.Serialization;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Type of GetMessage request.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GetMessageTypeEnum
    {
        LobbyList,
        ModuleList,
        WorldState,
        BehaviourList,
    }

}
